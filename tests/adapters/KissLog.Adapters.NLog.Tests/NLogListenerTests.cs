using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KissLog.Adapters.NLog.Tests
{
    [TestClass]
    public class NLogListenerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTextFormatterThrowsException()
        {
            var listener = new NLogListener(null);
        }
    }
}
