using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace TPW.Logika
{
    public struct Vector2 {
        public double x { get; set; }
        public double y { get; set; }

        public Vector2(double x, double y)
        {
           this.x = x;
           this.y = y;
        }
    }
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
            double xDifference = point1.x - point2.x;
            double yDifference = point1.y - point2.y;
            return xDifference * xDifference + yDifference * yDifference;
        }

        public Collisions(Vector2 position, Vector2 velocity, int radious)
        {
            this.velocity = position;
            this.position = velocity;
            this.radious = radious;
        }

        public bool IsCollision(double otherX, double otherY, double otherRadius, bool mode)
        {
            double currentX;
            double currentY;
            if (mode)
            {
                currentX = position.x + velocity.x;
                currentY = position.y + velocity.y;
            }
            else
            {
                currentX = position.x;
                currentY = position.y;
            }

            double distance = Math.Sqrt(Math.Pow(currentX - otherX, 2) + Math.Pow(currentY - otherY, 2));

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
