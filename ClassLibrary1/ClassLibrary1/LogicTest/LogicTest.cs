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
        public void AddBallTest()
        {
            logicApi.AddBalls(1);
            //logicApi.GetBallsList().;
        }

	

	}
    
}


