using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog.Tests.LogArguments
{
    [TestClass]
    public class StringTests
    {
        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void NullStringIsIgnored(string message)
        {
            Logger logger = new Logger();
            logger.Log(LogLevel.Trace, message);

            int count = logger.DataContainer.LogMessages.Count();

            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void StringCreatesAMessage()
        {
            string message = $"Log message: {Guid.NewGuid()}";

            Logger logger = new Logger();
            logger.Log(LogLevel.Trace, message);

            IEnumerable<LogMessage> logMessages = logger.DataContainer.LogMessages;

            bool exists = logMessages.Any(p => p.Message == message);

            Assert.IsTrue(exists);
        }
    }
}
