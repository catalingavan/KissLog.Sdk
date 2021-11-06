using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KissLog.Tests
{
    [TestClass]
    public class InternalLoggerTests
    {
        [TestMethod]
        public void LogExceptionDoesNotThrowExceptionWithNullArgument()
        {
            InternalLogger.LogException(null);
        }

        [TestMethod]
        public void LogExceptionDoesNotThrowExceptionWithuNullInternalLogDelegate()
        {
            KissLogConfiguration.InternalLog = null;

            InternalLogger.LogException(new Exception());
        }

        [TestMethod]
        public void LogException()
        {
            string messageArg = null;
            KissLogConfiguration.InternalLog = (message) => messageArg = message;

            string exceptionMessage = $"Exception {Guid.NewGuid()}";

            InternalLogger.LogException(new Exception(exceptionMessage));

            Assert.IsNotNull(messageArg);
            Assert.IsTrue(messageArg.Contains(exceptionMessage));
        }

        [TestMethod]
        public void ExceptionIsSilentlySwallowed()
        {
            CommonTestHelpers.ResetContext();

            KissLogConfiguration.InternalLog = (message) =>
            {
                throw new Exception();
            };

            InternalLogger.LogException(new Exception());
        }
    }
}
