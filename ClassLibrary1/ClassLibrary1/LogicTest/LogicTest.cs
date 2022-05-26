using NUnit.Framework;

using System.Collections.Generic;
using System.Numerics;
using TPW.Logika;

namespace TestProject1
{
    public class Tests
    {
        private LogicApi logicApi;
        private readonly Vector2 boardSize = new Vector2(150, 100);
        private Vector2 position = new Vector2(20, 20);
        private Vector2 velocity = new Vector2(1, 1);
        private Vector2 position2 = new Vector2(30, 30);
        [SetUp]
        public void Setup()
        {
            logicApi = LogicApi.CreateBallsLogic(boardSize);
        }

        [Test]
        public void CollisionTest()
        {
            Collisions collisions = new Collisions(position, velocity, 40);
            logicApi.AddBalls(2);
            Assert.IsTrue(collisions.IsCollision(position2, 30, true));
           
        }

	

	}
    
}


