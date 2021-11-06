using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KissLog.AspNet.Mvc.Tests
{
    [TestClass]
    public class KissLogWebMvcExceptionFilterAttributeTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OnExceptionThrowsExceptionForNullException()
        {
            var httpContext = new Mock<HttpContextBase>();

            var filter = new KissLogWebMvcExceptionFilterAttribute();
            filter.OnException(null, httpContext.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OnExceptionThrowsExceptionForNullHttpContext()
        {
            var filter = new KissLogWebMvcExceptionFilterAttribute();
            filter.OnException(new Exception(), null);
        }

        [TestMethod]
        public void OnExceptionGetsOrAddsTheLoggerToHttpContext()
        {
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(p => p.Items).Returns(new Dictionary<string, object>());

            var filter = new KissLogWebMvcExceptionFilterAttribute();
            filter.OnException(new Exception(), httpContext.Object);

            var dictionary = httpContext.Object.Items[KissLog.AspNet.Web.LoggerFactory.DictionaryKey] as IDictionary<string, Logger>;

            Assert.IsNotNull(dictionary);
            Assert.AreEqual(1, dictionary.Count);
        }

        [TestMethod]
        public void OnExceptionLogsTheError()
        {
            var ex = new Exception($"Exception {Guid.NewGuid()}");

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(p => p.Items).Returns(new Dictionary<string, object>());

            var filter = new KissLogWebMvcExceptionFilterAttribute();
            filter.OnException(ex, httpContext.Object);

            var dictionary = httpContext.Object.Items[KissLog.AspNet.Web.LoggerFactory.DictionaryKey] as IDictionary<string, Logger>;
            Logger logger = dictionary[Constants.DefaultLoggerCategoryName];

            Exception capturedException = logger.DataContainer.Exceptions.First();
            LogMessage message = logger.DataContainer.LogMessages.First();

            Assert.AreEqual(ex.Message, capturedException.Message);
            Assert.IsTrue(message.Message.Contains(ex.Message));
        }

        [TestMethod]
        [DataRow(200)]
        [DataRow(404)]
        [DataRow(500)]
        public void OnExceptionUpdatesTheHttpExceptionStatusCode(int statusCode)
        {
            var ex = new HttpException(statusCode, $"Exception {Guid.NewGuid()}");

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(p => p.Items).Returns(new Dictionary<string, object>());

            var filter = new KissLogWebMvcExceptionFilterAttribute();
            filter.OnException(ex, httpContext.Object);

            var dictionary = httpContext.Object.Items[KissLog.AspNet.Web.LoggerFactory.DictionaryKey] as IDictionary<string, Logger>;
            Logger logger = dictionary[Constants.DefaultLoggerCategoryName];

            Exception capturedException = logger.DataContainer.Exceptions.First();
            LogMessage message = logger.DataContainer.LogMessages.First();

            Assert.AreEqual(ex.Message, capturedException.Message);
            Assert.IsTrue(message.Message.Contains(ex.Message));
            Assert.AreEqual(statusCode, logger.DataContainer.LoggerProperties.ExplicitStatusCode.Value);
        }
    }
}
