using NUnit.Framework;
using System.Numerics;
using System.Threading;
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
            Vector2 vector2 = new Vector2(1, 1);
            api.SetBallSpeed(1, vector2);
            Assert.AreEqual(vector2, api.GetVelocityBall(1));
        }
    }

}