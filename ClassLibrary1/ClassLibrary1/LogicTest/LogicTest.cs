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
        [SetUp]
        public void Setup()
        {
            logicApi = LogicApi.CreateBallsLogic(boardSize);
        }

        [Test]
        public void Test1()
        {


            logicApi.AddBalls(1);
            Assert.AreEqual(1, logicApi.GetBallCount());

        }

    }
}


