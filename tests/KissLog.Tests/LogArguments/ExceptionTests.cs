using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog.Tests.LogArguments
{
    [TestClass]
    public class ExceptionTests
    {
        [TestMethod]
        public void NullExceptionIsIgnored()
        {
            Exception ex = null;

            Logger logger = new Logger();
            logger.Log(LogLevel.Trace, ex);

            IEnumerable<LogMessage> logMessages = logger.DataContainer.LogMessages;

            int count = logMessages.Count();

            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void ExceptionCreatesALogMessages()
        {
            var ex = new Exception();

            Logger logger = new Logger();
            logger.Log(LogLevel.Trace, ex);

            int count = logger.DataContainer.LogMessages.Count();

            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void OnlyRootExceptionsAreStoredInLoggerDataContainer()
        {
            var ex1 = new Exception("Exception #1",
                new Exception("Inner exception #1")
            );

            var ex2 = new Exception("Exception #2",
                new Exception("Inner exception #2",
                    new Exception("Inner exception #3")
                )
            );

            Logger logger = new Logger();
            logger.Log(LogLevel.Trace, ex1);
            logger.Log(LogLevel.Trace, ex2);

            int count = logger.DataContainer.Exceptions.Count();

            Assert.AreEqual(2, count);
        }
    }
}
