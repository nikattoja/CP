using NUnit.Framework;
using TPW.Dane;
using System.Numerics;

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
            var startPosition = api.GetVelocityBall(1);
            Vector2 vector2 = new Vector2(3, 3);
            api.SetBallSpeed(1, vector2);
            Assert.AreNotEqual(startPosition, api.GetVelocityBall);
        }
    }
}