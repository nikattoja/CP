using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using TPW.Dane;
using static TPW.Logika.LogicApi;

namespace TPW.Logika
{ 
public interface ILogicBall
{
	Vector2 Position { get; set; }
	int Id { get; }
}

internal class LogicBallDecorator : ILogicBall
{
	private readonly IBall ball;
	private readonly BallsLogic owner;
	public event EventHandler<OnPositionChangeEventArgs> PositionChange;
	public int Id { get; private set; }

	public LogicBallDecorator(IBall ball, int id, BallsLogic owner)
	{
		this.ball = ball;
		this.owner = owner;
		this.Id = id;
	
	}
	public Vector2 Position
	{
		get => ball.Position;
		set => ball.Position = value;
	}
	 public Vector2 Velocity
    {
        get=> ball.Velocity;
        set => ball.Velocity = value;
    }
    public void Simulate()
	{
		while (!owner.CancelSimulationSource.Token.IsCancellationRequested)
        {
            Position = GetRandomPointInsideBoard();
            PositionChange?.Invoke(this, new OnPositionChangeEventArgs(this));
            new ManualResetEvent(false).WaitOne(2);
            }
	}

        private Vector2 GetRandomPointInsideBoard()
        {

            Vector2 newPosition = Position + Velocity;
            Vector2 newPosition2 = Velocity;

            if (newPosition.X < 0 || newPosition.X > owner.BoardSize.X - BallsLogic.BallRadius)
            {
                newPosition2.X = -newPosition2.X;
            }

            if (newPosition.Y < 0 || newPosition.Y > owner.BoardSize.Y - BallsLogic.BallRadius)
            {
                newPosition2.Y = -newPosition2.Y;
            }
            Velocity = newPosition2;

            return Position + newPosition2;
        }

    }
}