using System;
using System.ComponentModel;
using System.Numerics;
using System.Reactive;
using System.Reactive.Linq;
using TPW.Logika;
using TPW.Dane;
using System.Collections.Generic;

namespace TPW.Presentation.Model
{
    public interface IBall : INotifyPropertyChanged
    {
        double Top { get; }
        double Left { get; }
        int Diameter { get; }
    }
    public class BallChaneEventArgs : EventArgs
    {
        public IBall Ball { get; set; }
    }

    public abstract class ModelAPI : IObservable<IBall>
    {
        public abstract void StartSimulation();
       // public abstract void StopSimulation();
        public abstract void SetBallNumber(int amount);
        public abstract int GetBallsCount();

        public abstract IDisposable Subscribe(IObserver<IBall> observer);
        public static ModelAPI CreateApi()
        {
            return new MainModel();
        }

        internal class MainModel : ModelAPI
        {

            private readonly Vector2 boardSize;
            private int ballsAmount;
            private LogicApi ballsLogic;
            public event EventHandler<BallChaneEventArgs> BallChanged;
            private IObservable<EventPattern<BallChaneEventArgs>> eventObservable = null;
            private List<BallModel> Balls = new List<BallModel>();

            public MainModel()
            {
                
                boardSize = new Vector2(650, 400);
                ballsLogic = ballsLogic ?? LogicApi.CreateBallsLogic(boardSize);
                ballsAmount = 5;
                IDisposable observer = ballsLogic.Subscribe<int>(x => Balls[x - 1].Move(ballsLogic.getBallPosition(x).X, ballsLogic.getBallPosition(x).Y));
                eventObservable = Observable.FromEventPattern<BallChaneEventArgs>(this, "BallChanged");

            }
            public override void StartSimulation()
            {
                ballsLogic.AddBalls(ballsAmount);
                for (int i = 1; i <= ballsAmount; i++)
                {
                    BallModel newBall = new BallModel(ballsLogic.getBallPosition(i).X, ballsLogic.getBallPosition(i).Y, 40);
                    Balls.Add(newBall);
                }

                foreach (BallModel ball in Balls)
                {
                    BallChanged?.Invoke(this, new BallChaneEventArgs() { Ball = ball });
                }
            }

         /*   public override void StopSimulation()
            {
                ballsLogic.StopSimulation();
                ballsLogic = LogicApi.CreateBallsLogic(boardSize);
                ballsLogic.PositionChange += (sender, args) =>
                {
                    BallPositionChange?.Invoke(this, new OnPositionChangeUiAdapterEventArgs(args.Ball.Position, args.Ball.Id));
                };
            }*/

            public override void SetBallNumber(int amount)
            {
                ballsAmount = amount;
            }

            public override int GetBallsCount()
            {
                return ballsAmount;
            }

            public override IDisposable Subscribe(IObserver<IBall> observer)
            {
                return eventObservable.Subscribe(x => observer.OnNext(x.EventArgs.Ball), ex => observer.OnError(ex), () => observer.OnCompleted());
            }
        }
    }
}
