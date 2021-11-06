using KissLog.Exceptions;
using KissLog.LogResponseBody;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace KissLog.Tests.LogResponseBody
{
    [TestClass]
    public class LogResponseBodySizeTooLargeExceptionTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenLoggerIsNull()
        {
            var service = new LogResponseBodySizeTooLargeException(null, 0 ,0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowsExceptionWhenContentLengthIsNegative()
        {
            var service = new LogResponseBodySizeTooLargeException(new Logger(), -1, 0);
        }

        [TestMethod]
        [DataRow(0, 0)]
        [DataRow(0, 10)]
        [DataRow(5, 10)]
        [DataRow(10, 10)]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowsExceptionWhenContentLengthIsLteThanMaximumAllowedFileSize(long contentLength, long maximumAllowedFileSizeInBytes)
        {
            var service = new LogResponseBodySizeTooLargeException(new Logger(), contentLength, maximumAllowedFileSizeInBytes);
        }

        [TestMethod]
        public void CreatesALogWithTheErrorMessage()
        {
            Logger logger = new Logger();

            long contentLength = Constants.MaximumAllowedFileSizeInBytes + 1;
            long maximumAllowedFileSizeInBytes = Constants.MaximumAllowedFileSizeInBytes;

            var ex = new ResponseBodySizeTooLargeException(contentLength, maximumAllowedFileSizeInBytes);

            var service = new LogResponseBodySizeTooLargeException(logger, contentLength, maximumAllowedFileSizeInBytes);
            service.Execute();

            LogMessage message = logger.DataContainer.LogMessages.FirstOrDefault();

            Assert.IsNotNull(message);
            Assert.IsTrue(message.Message.Contains(ex.Message));
        }
    }
}
