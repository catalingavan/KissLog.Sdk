using KissLog.Http;
using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace KissLog.Tests
{
    [TestClass]
    public class StatusCodeInterceptorTests
    {
        [TestMethod]
        public void MinimumLogMessageLevelHasDefaultValue()
        {
            var interceptor = new StatusCodeInterceptor();

            Assert.AreEqual(LogLevel.Trace, interceptor.MinimumLogMessageLevel);
        }

        [TestMethod]
        public void MinimumResponseHttpStatusCodeHasDefaultValue()
        {
            var interceptor = new StatusCodeInterceptor();

            Assert.AreEqual(0, interceptor.MinimumResponseHttpStatusCode);
        }

        [TestMethod]
        public void ShouldLogHttpRequestReturnsTrue()
        {
            var interceptor = new StatusCodeInterceptor();

            ILogListener listener = new Mock<ILogListener>().Object;
            bool result = interceptor.ShouldLog((HttpRequest)null, listener);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldLogLogMessageThrowsExceptionForNullLogMessage()
        {
            var interceptor = new StatusCodeInterceptor();

            ILogListener listener = new Mock<ILogListener>().Object;
            interceptor.ShouldLog((LogMessage)null, listener);
        }

        [TestMethod]
        [DataRow(LogLevel.Trace, false)]
        [DataRow(LogLevel.Debug, false)]
        [DataRow(LogLevel.Information, false)]
        [DataRow(LogLevel.Warning, true)]
        [DataRow(LogLevel.Error, true)]
        [DataRow(LogLevel.Critical, true)]
        public void ShouldLogLogMessageEvaluatesMinimumLogMessageLevel(LogLevel logLevel, bool expectedResult)
        {
            var interceptor = new StatusCodeInterceptor
            {
                MinimumLogMessageLevel = LogLevel.Warning
            };

            ILogListener listener = new Mock<ILogListener>().Object;
            LogMessage message = new LogMessage(new LogMessage.CreateOptions
            {
                CategoryName = Constants.DefaultLoggerCategoryName,
                LogLevel = logLevel,
                Message = "Message"
            });

            bool result = interceptor.ShouldLog(message, listener);

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [DataRow(100, false)]
        [DataRow(200, false)]
        [DataRow(300, false)]
        [DataRow(400, true)]
        [DataRow(401, true)]
        [DataRow(403, true)]
        [DataRow(500, true)]
        public void ShouldLogFlushLogArgsEvaluatesMinimumResponseHttpStatusCode(int statusCode, bool expectedResult)
        {
            var interceptor = new StatusCodeInterceptor
            {
                MinimumResponseHttpStatusCode = 400
            };

            ILogListener listener = new Mock<ILogListener>().Object;
            FlushLogArgs flushLogArgs = CommonTestHelpers.Factory.CreateFlushLogArgs();
            flushLogArgs.HttpProperties.Response.SetStatusCode(statusCode);

            bool result = interceptor.ShouldLog(flushLogArgs, listener);

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void LoggerNotManagedByHttpRequestAlwaysReturnsTrueForShouldLogFlushLogArgs(bool isManagedByHttpRequest)
        {
            var interceptor = new StatusCodeInterceptor
            {
                MinimumResponseHttpStatusCode = 600
            };

            ILogListener listener = new Mock<ILogListener>().Object;

            Logger logger = new Logger();
            logger.DataContainer.LoggerProperties.IsManagedByHttpRequest = isManagedByHttpRequest;

            FlushLogArgs flushLogArgs = FlushLogArgsFactory.Create(new[] { logger });

            bool result = interceptor.ShouldLog(flushLogArgs, listener);
            bool expectedResult = isManagedByHttpRequest == true ? false : true;

            Assert.AreEqual(expectedResult, result);
        }
    }
}
