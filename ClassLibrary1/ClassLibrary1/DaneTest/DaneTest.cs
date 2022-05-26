using NUnit.Framework;
using TPW.Dane;

namespace DaneTest
{
    public class Tests
    {
        DaneAPI api;
        [SetUp]
        public void Setup()
        {
            api = DaneAPI.CreateDataBall();
        }

        [Test]
        public void AddBallsAndStart()
        {
            api.CreateBalls(1);
            Assert.AreEqual(1, api.GetBallsCount());
            var startPosition = api.GetPositionBall(1);
            
            Assert.AreNotEqual(startPosition, api.GetPositionBall(1));
        }
    }

}