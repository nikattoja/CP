using System;
using System.ComponentModel;
using System.Numerics;
using System.Reactive;
using System.Reactive.Linq;
using TPW.Logika;
using TPW.Dane;

namespace TPW.Presentation.Model
{
    public class OnPositionChange : EventArgs
    {
        public readonly Vector2 Position;
        public readonly int Id;
        public OnPositionChange(Vector2 position, int id)
        {
            this.Position = position;
            Id = id;
        }
    }
    public abstract class ModelAPI
    {
        public abstract void StartSimulation();
       // public abstract void StopSimulation();
        public abstract void SetBallNumber(int amount);
        public abstract int GetBallsCount();
        public abstract void OnBallPositionChange(OnPositionChange args);
        public abstract event EventHandler<OnPositionChange> BallPositionChange;

        public static ModelAPI CreateApi()
        {
            return new MainModel();
        }

        internal class MainModel : ModelAPI
        {

            private readonly Vector2 boardSize;
            private int ballsAmount;
            private LogicApi ballsLogic;
            private IObservable<EventPattern<OnPositionChange>> eventObservable = null;
            public override event EventHandler<OnPositionChange> BallPositionChange;

            public MainModel()
            {
                boardSize = new Vector2(650, 400);
                ballsAmount = 0;
                ballsLogic = LogicApi.CreateBallsLogic(boardSize);
                ballsLogic.PositionChange += (sender, args) =>
                {
                    BallPositionChange?.Invoke(this, new OnPositionChange(args.Ball.Position, args.Ball.Id));
                };
                eventObservable = Observable.FromEventPattern<OnPositionChange>(this, "BallChanged");
            }
            public override void StartSimulation()
            {
                ballsLogic.AddBalls(ballsAmount);
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

            public override void OnBallPositionChange(OnPositionChange args)
            {
                BallPositionChange?.Invoke(this, args);
            }
        }
    }
}
