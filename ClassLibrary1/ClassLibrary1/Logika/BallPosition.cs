﻿using System;
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

internal class BallPosition : ILogicBall
{
	private readonly IBall ball;
	private readonly BallsLogic owner;
	public event EventHandler<OnPositionChangeEventArgs> PositionChange;
	public int Id { get; private set; }

	public BallPosition(IBall ball, int id, BallsLogic owner)
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
            Position = GetNextPosition();
            PositionChange?.Invoke(this, new OnPositionChangeEventArgs(this));
			Thread.Sleep(2);
            }
	}
        private Vector2 GetNextPosition()
        {

            Vector2 newPosition = Position + Velocity;
            Vector2 newPosition2 = Velocity;

            if (newPosition.X < 0 || newPosition.X > 650 - 40)
            {
                newPosition2.X = -newPosition2.X;
            }

            if (newPosition.Y < 0 || newPosition.Y > 400 - 40)
            {
                newPosition2.Y = -newPosition2.Y;
            }
            Velocity = newPosition2;

            return Position + newPosition2;
        }
    }
}