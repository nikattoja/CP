using System;
using System.Numerics;
using TPW.Logika;

namespace TPW.Presentation.Model
{
    public class OnPositionChangeUiAdapterEventArgs : EventArgs
    {
        public readonly Vector2 Position;
        public readonly int Id;

        public OnPositionChangeUiAdapterEventArgs(Vector2 position, int id)
        {
            this.Position = position;
            Id = id;
        }
    }
    public abstract class ModelAPI
    {
        public abstract void StartSimulation();
        public abstract void StopSimulation();
        public abstract void SetBallNumber(int amount);
        public abstract int GetBallsCount();
        public abstract void OnBallPositionChange(OnPositionChangeUiAdapterEventArgs args);
        public abstract event EventHandler<OnPositionChangeUiAdapterEventArgs> BallPositionChange;

        public static ModelAPI CreateApi()
        {
            return new MainModel();
        }
        internal class MainModel : ModelAPI
        {

            private readonly Vector2 boardSize;
            private int ballsAmount;
            private LogicApi ballsLogic;

            public override event EventHandler<OnPositionChangeUiAdapterEventArgs> BallPositionChange;

            public MainModel()
            {
                boardSize = new Vector2(650, 400);
                ballsAmount = 0;
                ballsLogic = LogicApi.CreateBallsLogic(boardSize);
                ballsLogic.PositionChange += (sender, args) =>
                {
                    BallPositionChange?.Invoke(this, new OnPositionChangeUiAdapterEventArgs(args.Ball.Position, args.Ball.Id));
                };
            }
            public override void StartSimulation()
            {
                ballsLogic.AddBalls(ballsAmount);
                ballsLogic.StartSimulation();
            }

            public override void StopSimulation()
            {
                ballsLogic.StopSimulation();
                ballsLogic = LogicApi.CreateBallsLogic(boardSize);
                ballsLogic.PositionChange += (sender, args) =>
                {
                    BallPositionChange?.Invoke(this, new OnPositionChangeUiAdapterEventArgs(args.Ball.Position, args.Ball.Id));
                };
            }

            public override void SetBallNumber(int amount)
            {
                ballsAmount = amount;
            }

            public override int GetBallsCount()
            {
                return ballsAmount;
            }

            public override void OnBallPositionChange(OnPositionChangeUiAdapterEventArgs args)
            {
                BallPositionChange?.Invoke(this, args);
            }
        }
    }
}
