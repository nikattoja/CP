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

        public Collisions(double poitionX, double poitionY, double speedX, double speedY, int radious, int mass)
        {
            this.velocity = new Vector2(speedX, speedY);
            this.position = new Vector2(poitionX, poitionY);
            this.radious = radious;
            this.mass = mass;
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

        public Vector2[] ImpulseSpeed(double otherX, double otherY, double speedX, double speedY, double otherMass)
        {
            Vector2 velocityOther = new Vector2(speedX, speedY);
            Vector2 positionOther = new Vector2(otherX, otherY);

            double fDistance = Math.Sqrt((position.x - positionOther.x) * (position.x - positionOther.x) + (position.y - positionOther.y) * (position.y - positionOther.y));

            double nx = (positionOther.x - position.x) / fDistance;
            double ny = (positionOther.y - position.y) / fDistance;

            double tx = -ny;
            double ty = nx;

            // Dot Product Tangent
            double dpTan1 = velocity.x * tx + velocity.y * ty;
            double dpTan2 = velocityOther.x * tx + velocityOther.y * ty;

            // Dot Product Normal
            double dpNorm1 = velocity.x * nx + velocity.y * ny;
            double dpNorm2 = velocityOther.x * nx + velocityOther.y * ny;

            // Conservation of momentum in 1D
            double m1 = (dpNorm1 * (mass - otherMass) + 2.0f * otherMass * dpNorm2) / (mass + otherMass);
            double m2 = (dpNorm2 * (otherMass - mass) + 2.0f * mass * dpNorm1) / (mass + otherMass);

            return new Vector2[2] { new Vector2(tx * dpTan1 + nx * m1, ty * dpTan1 + ny * m1), new Vector2(tx * dpTan2 + nx * m2, ty * dpTan2 + ny * m2) };

        }
    }
}
