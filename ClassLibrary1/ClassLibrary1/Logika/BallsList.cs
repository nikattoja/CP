using System.Collections.Generic;
using System.Numerics;

namespace TPW.Logika
{
    internal class BallsList : LogicApi
    {
        private readonly List<IBall> ballsList;

        public BallsList()
        {
            this.ballsList = new List<IBall>();
        }

        public override void Add(IBall ball)
        {
            ballsList.Add(ball);
        }
        public override void AddBalls(int howMany)
        {
            throw new System.NotImplementedException();
        }

        public override IBall Get(int index)
        {
            return ballsList[index];
        }

        public override int GetBallCount()
        {
            return ballsList.Count;
        }

        public override void StartSimulation()
        {
            throw new System.NotImplementedException();
        }

        public override void StopSimulation()
        {
            throw new System.NotImplementedException();
        }
    }
}
