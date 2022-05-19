using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using TPW.Presentation.Model;

namespace TPW.Presentation.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ModelAPI model;
        private string inputText;
        public AsyncObservableCollection<BallPosition> Circles { get; set; }

        public int BallsCount
        {
            get { return model.GetBallsCount(); }
            set
            {
                if (value >= 0)
                {
                    model.SetBallNumber(value);
                    OnPropertyChanged();
                }
            }
        }
        public string numberOfBalls
        {
            get
            {
                return inputText;
            }
            set
            {
                inputText = value;
                OnPropertyChanged(nameof(numberOfBalls));
            }
        }
        public ICommand StartSimulationButton { get; }
        public ICommand StopSimulationButton { get; }
        public MainViewModel(ModelAPI baseModel)
        {
            this.model = baseModel;
        }
        public int readFromTextBox()
        {
            int number;
            if (Int32.TryParse(numberOfBalls, out number) && numberOfBalls != "0")
            {
                number = Int32.Parse(numberOfBalls);

                if (number > 1001)
                {
                    return 1001;
                }
                return number;
            }
            return 5;
        }
        public MainViewModel() : this(ModelAPI.CreateApi())
        {
            Circles = new AsyncObservableCollection<BallPosition>();

            BallsCount = 5;

            StartSimulationButton = new RelayCommand(() =>
            {
                model.SetBallNumber(readFromTextBox());

                for (int i = 0; i < BallsCount; i++)
                {
                    Circles.Add(new BallPosition());
                }

                model.BallPositionChange += (sender, argv) =>
                {
                    if(Circles.Count > 0)
                        Circles[argv.Id].ChangePosition(argv.Position);
                };
                model.StartSimulation();
            });

            StopSimulationButton = new RelayCommand(() =>
            {
               // model.StopSimulation();
                Circles.Clear();
                model.SetBallNumber(BallsCount);

            });
        }

        // Event for View update
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}


public class RelayCommand : ICommand
{
    private readonly Action handler;
    private bool isEnabled;

    public RelayCommand(Action handler)
    {
        this.handler = handler;
        IsEnabled = true;
    }

    public bool IsEnabled
    {
        get { return isEnabled; }
        set
        {
            if (value != isEnabled)
            {
                isEnabled = value;
                if (CanExecuteChanged != null)
                {
                    CanExecuteChanged(this, EventArgs.Empty);
                }
            }
        }
    }

    public bool CanExecute(object parameter)
    {
        return IsEnabled;
    }

    public event EventHandler CanExecuteChanged;

    public void Execute(object parameter)
    {
        handler();
    }
}

public class AsyncObservableCollection<T> : ObservableCollection<T>
{
    private SynchronizationContext _synchronizationContext = SynchronizationContext.Current;

    public AsyncObservableCollection()
    {
    }

    public AsyncObservableCollection(IEnumerable<T> list)
        : base(list)
    {
    }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        if (SynchronizationContext.Current == _synchronizationContext)
        {
            // Execute the CollectionChanged event on the current thread
            RaiseCollectionChanged(e);
        }
        else
        {
            // Raises the CollectionChanged event on the creator thread
            _synchronizationContext.Send(RaiseCollectionChanged, e);
        }
    }

    private void RaiseCollectionChanged(object param)
    {
        // We are in the creator thread, call the base implementation directly
        base.OnCollectionChanged((NotifyCollectionChangedEventArgs)param);
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        if (SynchronizationContext.Current == _synchronizationContext)
        {
            // Execute the PropertyChanged event on the current thread
            RaisePropertyChanged(e);
        }
        else
        {
            // Raises the PropertyChanged event on the creator thread
            _synchronizationContext.Send(RaisePropertyChanged, e);
        }
    }

    private void RaisePropertyChanged(object param)
    {
        // We are in the creator thread, call the base implementation directly
        base.OnPropertyChanged((PropertyChangedEventArgs)param);
    }
}

public class BallPosition : INotifyPropertyChanged
{
    private Vector2 pos;
    public float X
    {
        get { return pos.X; }
        set { pos.X = value; OnPropertyChanged(); }
    }
    public float Y
    {
        get { return pos.Y; }
        set { pos.Y = value; OnPropertyChanged(); }
    }

    public BallPosition(float x, float y)
	{
		X = x;
		Y = y;
    }
    public BallPosition(Vector2 position)
    {
        X = position.X;
        Y = position.Y;
    }

	public BallPosition()
	{
        X = 0;
        Y = 0;
	}

    public void ChangePosition(Vector2 position)
	{
        this.X = position.X;
        this.Y = position.Y;
	}

	public override string ToString()
	{
        return $"({X}, {Y})";
	}

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string caller = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
    }
}