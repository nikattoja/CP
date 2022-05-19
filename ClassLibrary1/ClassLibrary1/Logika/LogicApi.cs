using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using TPW.Dane;

namespace TPW.Logika
{ 


public abstract class LogicApi : IObserver<int>, IObservable<int>
    {
        public class OnPositionChangeEventArgs : EventArgs
        {
            public readonly int ballid;

            public OnPositionChangeEventArgs(int ballid)
            {
                this.ballid = ballid;
            }
        }

        public abstract IDisposable Subscribe(IObserver<int> observer);

	public event EventHandler<OnPositionChangeEventArgs> PositionChange;
	public abstract void AddBalls(int howMany);
	protected virtual void OnPositionChange(OnPositionChangeEventArgs args)
	{
		PositionChange?.Invoke(this, args);
	}

	public static LogicApi CreateBallsLogic(Vector2 boardSize, DaneAPI data = default(DaneAPI))
	{
		return new BallsLogic( boardSize, data == null ? DaneAPI.CreateDataBall() : data);
	}

        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(int value);

        internal class BallsLogic : LogicApi, IObservable<int>
        {
            private readonly DaneAPI daneAPI;
            public static readonly int BallRadius = 40;
            private IDisposable unsubscriber;
            static object _lock = new object();
            private IObservable<EventPattern<OnPositionChangeEventArgs>> eventObservable = null;
            public event EventHandler<OnPositionChangeEventArgs> BallChanged;

            public CancellationTokenSource CancelSimulationSource { get; private set; }

            public BallsLogic(Vector2 boardSize,DaneAPI daneAPI)
            {
                eventObservable = Observable.FromEventPattern<OnPositionChangeEventArgs>(this, "BallChanged");
                this.daneAPI = daneAPI;
                BoardSize = boardSize;
                CancelSimulationSource = new CancellationTokenSource();
            }

            public Vector2 BoardSize { get; }

            protected override void OnPositionChange(OnPositionChangeEventArgs args)
            { 
                base.OnPositionChange(args);
            }
            public List<Ball> GetBallsList()
            {
                return daneAPI.GetBallsList();
            }
     
            public override void AddBalls(int howMany)
            {
                daneAPI.CreateBalls(howMany);
            }

            public virtual void Subscribe(IObservable<int> provider)
            {
                if (provider != null)
                    unsubscriber = provider.Subscribe(this);
            }

            public override IDisposable Subscribe(IObserver<int> observer)
            {
                return eventObservable.Subscribe(x => observer.OnNext(x.EventArgs.ballid), ex => observer.OnError(ex), () => observer.OnCompleted());
            }

            public override void OnCompleted()
            {
                Unsubscribe();
            }

            public override void OnError(Exception error)
            {
                throw error;
            }

            public override void OnNext(int value)
            {
                {
                    var tmpBallList = daneAPI.GetBallsList();
                    

                    Monitor.Enter(tmpBallList);
                    try
                    {
                        Collisions collisions = new Collisions(tmpBallList[value].Position, tmpBallList[value].Velocity, 40);
                        
                        for(int i = 1; i < tmpBallList.Count+1; i++)
                        { 

                        }
                       BallChanged?.Invoke(this, new OnPositionChangeEventArgs(value));
                    }
                    catch (SynchronizationLockException exception)
                    {
                        throw new Exception("Checking collision synchronization lock not working", exception);
                    }
                    finally
                    {
                        Monitor.Exit(tmpBallList);
                    }
                }
            }
            public virtual void Unsubscribe()
            {
                unsubscriber.Dispose();
            }
        }
    }
}