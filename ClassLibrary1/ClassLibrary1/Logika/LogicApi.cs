using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using TPW.Dane;

namespace TPW.Logika
{ 
public class OnPositionChangeEventArgs : EventArgs
{
	public readonly ILogicBall Ball;

	public OnPositionChangeEventArgs(ILogicBall ball)
	{
		this.Ball = ball;
	}
}

public abstract class LogicApi
{
	public abstract void Add(IBall ball);
	public abstract IBall Get(int index);
	public abstract int GetBallCount();
	public static LogicApi CreateBallsList()
	{
		return new BallsList();
	}
  
	public static IBall CreateBall(Vector2 position, Vector2 velocity)
    {
        return new Ball(position,velocity);
    }

	public event EventHandler<OnPositionChangeEventArgs> PositionChange;
	public abstract void AddBalls(int howMany);
	public abstract void StartSimulation();
	public abstract void StopSimulation();
	protected virtual void OnPositionChange(OnPositionChangeEventArgs args)
	{
		PositionChange?.Invoke(this, args);
	}

	public static LogicApi CreateBallsLogic(Vector2 boardSize, LogicApi logicApi = default(LogicApi), DaneAPI data = default(DaneAPI))
	{
		if (logicApi == null)
		{
				logicApi = CreateBallsList();
		}
		return new BallsLogic(logicApi, boardSize, data == null ? DaneAPI.CreateDataBall() : data);
	}

        internal class BallsLogic : LogicApi
        {
            private readonly DaneAPI daneAPI;
            public static readonly int BallRadius = 40;
            private readonly LogicApi dataBalls;
            public CancellationTokenSource CancelSimulationSource { get; private set; }

            public BallsLogic(LogicApi dataBalls, Vector2 boardSize,DaneAPI daneAPI)
            {
                this.daneAPI = daneAPI;
                this.dataBalls = dataBalls;
                BoardSize = boardSize;
                CancelSimulationSource = new CancellationTokenSource();
            }

            public Vector2 BoardSize { get; }

            protected override void OnPositionChange(OnPositionChangeEventArgs args)
            {
                base.OnPositionChange(args);
            }
            private Vector2 GetRandomNormalizedVector()
            {
                var rng = new Random();
                var x = (float)(rng.NextDouble() - 0.5) * 2;
                var y = (float)(rng.NextDouble() - 0.5) * 2;
                var result = new Vector2(x, y);
                return Vector2.Normalize(result);
            }
            public override void AddBalls(int howMany)
            {
                for (var i = 0; i < howMany; i++)
                {
                    var randomPoint = GetRandomPointInsideBoard();
                    var randomVelocity = GetRandomNormalizedVector();
                    dataBalls.Add(LogicApi.CreateBall(randomPoint, randomVelocity));
                }
            }

            private Vector2 GetRandomPointInsideBoard()
            {
                var rng = new Random();
                var x = rng.Next(BallRadius, (int)(BoardSize.X - BallRadius));
                var y = rng.Next(BallRadius, (int)(BoardSize.Y - BallRadius));

                return new Vector2(x, y);
            }
            public override void StartSimulation()
            {
                if (CancelSimulationSource.IsCancellationRequested) return;

                CancelSimulationSource = new CancellationTokenSource();
                
                for (var i = 0; i < dataBalls.GetBallCount(); i++)
                {
                    var ball = new BallPosition(dataBalls.Get(i), i, this);
                    ball.PositionChange += (_, args) => OnPositionChange(args);
                    Task.Factory.StartNew(ball.Simulate, CancelSimulationSource.Token);
                }
            }

            public override void StopSimulation()
            {
                CancelSimulationSource.Cancel();
            }
            public override void Add(IBall ball)
            {
                throw new NotImplementedException();
            }
            public override int GetBallCount()
            {
                return dataBalls.GetBallCount();
            }
            public override IBall Get(int index)
            {
                return dataBalls.Get(index);
            }
        }
    }
}