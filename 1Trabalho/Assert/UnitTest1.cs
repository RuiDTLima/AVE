using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Assert
{
    [TestClass]
    public class Assert
    {
        [TestMethod]
        public static void Equals(String first, String second)
        {
            Assert.Equals(first, second);
        }
    }
}
