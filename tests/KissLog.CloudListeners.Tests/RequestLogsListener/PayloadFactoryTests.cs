using KissLog.CloudListeners.RequestLogsListener;
using KissLog.RestClient.Requests.CreateRequestLog;
using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace KissLog.CloudListeners.Tests.RequestLogsListener
{
    [TestClass]
    public class PayloadFactoryTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateThrowsExceptionForNullArgument()
        {
            FlushLogArgs args = null;
            var result = PayloadFactory.Create(args);
        }

        [TestMethod]
        public void CreateCopiesAllProperties()
        {
            FlushLogArgs args = CommonTestHelpers.Factory.CreateFlushLogArgs();
            var logMessages = args.MessagesGroups.SelectMany(p => p.Messages).OrderBy(p => p.DateTime).ToList();

            var result = PayloadFactory.Create(args);

            Assert.IsNotNull(result.SdkName);
            Assert.IsNotNull(result.SdkVersion);
            Assert.AreEqual(args.HttpProperties.Request.StartDateTime, result.StartDateTime);
            Assert.IsTrue(result.DurationInMilliseconds >= 0);
            Assert.AreEqual(args.HttpProperties.Request.MachineName, result.MachineName);
            Assert.AreEqual(args.HttpProperties.Request.IsNewSession, result.IsNewSession);
            Assert.AreEqual(args.HttpProperties.Request.SessionId, result.SessionId);
            Assert.AreEqual(args.HttpProperties.Request.IsAuthenticated, result.IsAuthenticated);
            Assert.IsNull(result.User);
            Assert.AreEqual(logMessages.Count, result.LogMessages.Count());
            Assert.AreEqual(args.Exceptions.Count(), result.Exceptions.Count());
            Assert.AreEqual(args.CustomProperties.Count(), result.CustomProperties.Count);

            for(int i = 0; i < logMessages.Count; i++)
            {
                TestLogMessage(logMessages[i], result.LogMessages.ElementAt(i));
            }

            for (int i = 0; i < args.Exceptions.Count(); i++)
            {
                TestCapturedException(args.Exceptions.ElementAt(i), result.Exceptions.ElementAt(i));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateHttpPropertiesThrowsExceptionForNullArgument()
        {
            KissLog.Http.HttpProperties httpProperties = null;
            var result = PayloadFactory.Create(httpProperties);
        }

        [TestMethod]
        public void CreateHttpPropertiesCopiesAllProperties()
        {
            KissLog.Http.HttpProperties httpProperties = CommonTestHelpers.Factory.CreateHttpProperties();
            var result = PayloadFactory.Create(httpProperties);

            TestUrl(httpProperties.Request.Url, result.Url);
            Assert.AreEqual(httpProperties.Request.UserAgent, result.UserAgent);
            Assert.AreEqual(httpProperties.Request.HttpMethod, result.HttpMethod);
            Assert.AreEqual(httpProperties.Request.HttpReferer, result.HttpReferer);
            Assert.AreEqual(httpProperties.Request.RemoteAddress, result.RemoteAddress);
            TestRequestProperties(httpProperties.Request.Properties, result.Request);
            TestResponseProperties(httpProperties.Response, result.Response);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateRequestPropertiesThrowsExceptionForNullArgument()
        {
            KissLog.Http.RequestProperties requestProperties = null;
            var result = PayloadFactory.Create(requestProperties);
        }

        [TestMethod]
        public void CreateRequestPropertiesCopiesAllProperties()
        {
            KissLog.Http.RequestProperties requestProperties = CommonTestHelpers.Factory.CreateRequestProperties();
            var result = PayloadFactory.Create(requestProperties);

            TestRequestProperties(requestProperties, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateResponsePropertiesThrowsExceptionForNullArgument()
        {
            KissLog.Http.HttpResponse httpResponse = null;
            var result = PayloadFactory.Create(httpResponse);
        }

        [TestMethod]
        public void CreateResponsePropertiesCopiesAllProperties()
        {
            KissLog.Http.HttpResponse httpResponse = CommonTestHelpers.Factory.CreateHttpResponse();
            var result = PayloadFactory.Create(httpResponse);

            TestResponseProperties(httpResponse, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateLogMessageThrowsExceptionForNullArgument()
        {
            LogMessage message = null;
            var result = PayloadFactory.Create(message, DateTime.UtcNow);
        }

        [TestMethod]
        public void CreateLogMessageCopiesAllProperties()
        {
            DateTime startRequestDateTime = DateTime.UtcNow.AddSeconds(-5);

            LogMessage message = CommonTestHelpers.Factory.CreateLogMessage();
            var result = PayloadFactory.Create(message, startRequestDateTime);

            TestLogMessage(message, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateExceptionThrowsExceptionForNullArgument()
        {
            CapturedException exception = null;
            var result = PayloadFactory.Create(exception);
        }

        [TestMethod]
        public void CreateExceptionCopiesAllProperties()
        {
            CapturedException exception = CommonTestHelpers.Factory.CreateCapturedException();
            var result = PayloadFactory.Create(exception);

            TestCapturedException(exception, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateUriThrowsExceptionForNullArgument()
        {
            Uri uri = null;
            var result = PayloadFactory.Create(uri);
        }

        [TestMethod]
        public void CreateUriCopiesAllTheProperties()
        {
            Uri uri = new Uri("http://application/path1/path2?param1=value&param2=anotherValue");
            Url result = PayloadFactory.Create(uri);

            TestUrl(uri, result);
        }

        private void TestRequestProperties(KissLog.Http.RequestProperties requestProperties, KissLog.RestClient.Requests.CreateRequestLog.Http.RequestProperties result)
        {
            Assert.AreEqual(JsonSerializer.Serialize(requestProperties.Cookies), JsonSerializer.Serialize(result.Cookies));
            Assert.AreEqual(JsonSerializer.Serialize(requestProperties.Headers), JsonSerializer.Serialize(result.Headers));
            Assert.AreEqual(JsonSerializer.Serialize(requestProperties.Claims), JsonSerializer.Serialize(result.Claims));
            Assert.AreEqual(JsonSerializer.Serialize(requestProperties.QueryString), JsonSerializer.Serialize(result.QueryString));
            Assert.AreEqual(JsonSerializer.Serialize(requestProperties.FormData), JsonSerializer.Serialize(result.FormData));
            Assert.AreEqual(JsonSerializer.Serialize(requestProperties.ServerVariables), JsonSerializer.Serialize(result.ServerVariables));
            Assert.AreEqual(requestProperties.InputStream, result.InputStream);
        }

        private void TestResponseProperties(KissLog.Http.HttpResponse httpResponse, KissLog.RestClient.Requests.CreateRequestLog.Http.ResponseProperties result)
        {
            Assert.AreEqual(httpResponse.StatusCode, result.HttpStatusCode);
            Assert.AreEqual(((HttpStatusCode)httpResponse.StatusCode).ToString(), result.HttpStatusCodeText);
            Assert.AreEqual(JsonSerializer.Serialize(httpResponse.Properties.Headers), JsonSerializer.Serialize(result.Headers));
            Assert.AreEqual(httpResponse.Properties.ContentLength, result.ContentLength);
        }

        private void TestLogMessage(LogMessage message, KissLog.RestClient.Requests.CreateRequestLog.LogMessage result)
        {
            Assert.AreEqual(message.CategoryName, result.CategoryName);
            Assert.AreEqual(message.LogLevel.ToString(), result.LogLevel);
            Assert.AreEqual(message.Message, result.Message);
            Assert.AreEqual(message.MemberType, result.MemberType);
            Assert.AreEqual(message.MemberName, result.MemberName);
            Assert.AreEqual(message.LineNumber, result.LineNumber);
            Assert.IsTrue(result.MillisecondsSinceStartRequest >= 0);
        }

        private void TestCapturedException(CapturedException exception, KissLog.RestClient.Requests.CreateRequestLog.CapturedException result)
        {
            Assert.AreEqual(exception.Type, result.ExceptionType);
            Assert.AreEqual(exception.Message, result.ExceptionMessage);
        }

        private void TestUrl(Uri uri, Url result)
        {
            Assert.AreEqual(uri.AbsoluteUri, result.AbsoluteUri);
            Assert.AreEqual(uri.PathAndQuery, result.PathAndQuery);
            Assert.AreEqual(uri.LocalPath, result.Path);
        }
    }
}
