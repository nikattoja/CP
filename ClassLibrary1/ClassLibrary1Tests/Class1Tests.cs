using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Tests
{
    [TestClass()]
    public class Class1Tests
    {
        Class1 class1 = new(1.2, 3.6);

        [TestMethod()]
        public void SumTest()
        {
            Assert.Equals(4.8, class1.Sum());
        }

        [TestMethod()]
        public void MultiplyTest()
        {
            Assert.Equals(4.32, class1.Multiply());
        }
    }
}