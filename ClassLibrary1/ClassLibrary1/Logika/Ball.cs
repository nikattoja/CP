using System;
using System.Collections.Generic;
using System.Text;

namespace Logika
{
    public class Ball
    {
        double width;
        double height;
        double ballXPosition;
        double ballYPosition;

        public Ball(double width, double height, double ballXPosition, double ballYPosition)
        {
            this.width = width;
            this.height = height;
            this.ballXPosition = ballXPosition;
            this.ballYPosition = ballYPosition;
        }
    }
}