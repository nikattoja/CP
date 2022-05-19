using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace TPW.Logika
{

    public class Collisions
    {
        private int mass;
        private int radious;
        private Vector2 position;
        private Vector2 velocity;

        public static double Distance(Vector2 point1, Vector2 point2)
        {
            return Math.Sqrt(DistanceSquared(point1, point2));
        }

        public static double DistanceSquared(Vector2 point1, Vector2 point2)
        {
            double xDifference = point1.X - point2.X;
            double yDifference = point1.Y - point2.Y;
            return xDifference * xDifference + yDifference * yDifference;
        }

        public Collisions(Vector2 position, Vector2 velocity, int radious)
        {
            this.velocity = position;
            this.position = velocity;
            this.radious = radious;
        }

        public bool IsCollision(Vector2 other, double otherRadius, bool mode)
        {
            double currentX;
            double currentY;
            if (mode)
            {
                currentX = position.X + velocity.X;
                currentY = position.Y + velocity.Y;
            }
            else
            {
                currentX = position.X;
                currentY = position.X;
            }

            double distance = Math.Sqrt(Math.Pow(currentX - other.X, 2) + Math.Pow(currentY - other.Y, 2));

            if (Math.Abs(distance) <= radious + otherRadius)
            {
                return true;
            }

            return false;
        }

        public Vector2[] ImpulseSpeed(Vector2 first, Vector2 second )
        {
            var firstAfterChange = second;
            var secondAfterChange = first;
            Vector2[] vector2tab= new Vector2[2];
            vector2tab[0] = firstAfterChange;
            vector2tab[1] = secondAfterChange;
            return vector2tab;
        }
    }
}
