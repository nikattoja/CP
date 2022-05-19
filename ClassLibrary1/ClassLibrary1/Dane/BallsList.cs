using System.Collections.Generic;
using System.Numerics;

namespace TPW.Dane
{
    internal class BallsList 
    {
        private readonly List<Ball> ballsList;

        public BallsList()
        {
            this.ballsList = new List<Ball>();
        }

        public void AddBalls(int howMany)
        {
            for (int i = 0; i < howMany; i++)
            {
                ballsList.Add(new Ball(i+1));
            }
        }






    }
}
