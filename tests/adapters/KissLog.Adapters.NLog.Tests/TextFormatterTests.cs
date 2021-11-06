using KissLog.Http;
using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KissLog.Adapters.NLog.Tests
{
    [TestClass]
    public class TextFormatterTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FormatBeginRequestThrowsExceptionForNullHttpRequest()
        {
            var formatter = new TextFormatter();
            formatter.FormatBeginRequest(null);
        }

        [TestMethod]
        public void FormatBeginRequestReturnsNonEmptyString()
        {
            HttpRequest httpRequest = CommonTestHelpers.Factory.CreateHttpRequest();

            var formatter = new TextFormatter();
            string result = formatter.FormatBeginRequest(httpRequest);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FormatEndRequestThrowsExceptionForNullHttpRequest()
        {
            HttpResponse httpResponse = CommonTestHelpers.Factory.CreateHttpResponse();

            var formatter = new TextFormatter();
            formatter.FormatEndRequest(null, httpResponse);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FormatEndRequestThrowsExceptionForNullHttpResponse()
        {
            HttpRequest httpRequest = CommonTestHelpers.Factory.CreateHttpRequest();

            var formatter = new TextFormatter();
            formatter.FormatEndRequest(httpRequest, null);
        }

        [TestMethod]
        public void FormatEndRequestReturnsNonEmptyString()
        {
            HttpRequest httpRequest = CommonTestHelpers.Factory.CreateHttpRequest();
            HttpResponse httpResponse = CommonTestHelpers.Factory.CreateHttpResponse();

            var formatter = new TextFormatter();
            string result = formatter.FormatEndRequest(httpRequest, httpResponse);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FormatLogMessageThrowsExceptionForNullLogMessage()
        {
            var formatter = new TextFormatter();
            formatter.FormatLogMessage(null);
        }

        [TestMethod]
        public void FormatLogMessageReturnsNonEmptyString()
        {
            LogMessage logMessage = CommonTestHelpers.Factory.CreateLogMessage();

            var formatter = new TextFormatter();
            string result = formatter.FormatLogMessage(logMessage);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        [TestMethod]
        public void FormatLogMessageContainsLogMessage()
        {
            LogMessage logMessage = CommonTestHelpers.Factory.CreateLogMessage();

            var formatter = new TextFormatter();
            string result = formatter.FormatLogMessage(logMessage);

            Assert.IsTrue(result.Contains(logMessage.Message));
        }
    }
}
