using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KissLog.Tests
{
    [TestClass]
    public class LogListenerDecoratorTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullLogListenerThrowsException()
        {
            LogListenerDecorator decorator = new LogListenerDecorator(null);
        }
    }
}
