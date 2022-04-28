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

        public Ball(Vector2 position, Vector2 Velocity)
        {
            this.Position = position;
            this.Velocity = Velocity;
        }
        public override string ToString()
		{
			return $"({Position.X}, {Position.Y})";
		}
	}
}
