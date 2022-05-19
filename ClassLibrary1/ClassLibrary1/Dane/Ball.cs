using System;
using System.Numerics;
using System.Threading.Tasks;

namespace TPW.Dane
{
    public interface IBall
    {
        Vector2 Position { get; set; }
        Vector2 Velocity { get; set; }
    }

    internal class Ball : IBall
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public int id;
        private Task BallTask;

        public Ball(int id)
        {
            this.id = id;
            this.Position = GetRandomPointInsideBoard();
            var rng = new Random();
            var x = (float)(rng.NextDouble() - 0.5) * 1;
            var y = (float)(rng.NextDouble() - 0.5) * 1;
            var result = new Vector2(x, y);

            this.Velocity = result;
        }

        private Vector2 GetRandomPointInsideBoard()
        {
            var rng = new Random();
            var x = rng.Next(40, (int)(650 - 40));
            var y = rng.Next(40, (int)(400 - 40));

            return new Vector2(x, y);
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

        public override string ToString()
        {
            return $"({Position.X}, {Position.Y})";
        }
    }
}
