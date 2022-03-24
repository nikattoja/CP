using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SumTest()
        {
            ClassLibrary1.Class1 class1 = new(1.5, 3);
            Assert.AreEqual(class1.Sum(), 4.5);
        }

        [TestMethod]
        public void MultiplyTest()
        {
            ClassLibrary1.Class1 class1 = new(1.5, 3);
            Assert.AreEqual(class1.Multiply(), 4.5);
        }
    }
}