using System;
using System.Numerics;

namespace TPW.Logika
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
        public override string ToString()
		{
			return $"({Position.X}, {Position.Y})";
		}
	}
}
