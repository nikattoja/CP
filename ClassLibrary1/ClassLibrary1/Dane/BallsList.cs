using System.Collections.Generic;
using System.Numerics;

namespace TPW.Dane
{
    internal class BallsList 
    {
        private readonly List<Ball> ballsList;
        private Logger logger;
        public List<Ball> GetBallsList()
        {
            return ballsList;
        }

        public BallsList()
        {
            this.ballsList = new List<Ball>();
        }

        public void AddBalls(int howMany)
        {
            logger = new Logger();
            for (int i = 0; i < howMany; i++)
            {
                Ball ball = new Ball(i+1);
                ball.logger = logger;
                ballsList.Add(ball);
               
            }
        }
        public Ball GetBall(int id)
        {
            return ballsList[id-1];
        }
        public void SetBallSpeed(int id, Vector2 velocity)
        {
            ballsList[id].Velocity = velocity;
        }






    }
}
