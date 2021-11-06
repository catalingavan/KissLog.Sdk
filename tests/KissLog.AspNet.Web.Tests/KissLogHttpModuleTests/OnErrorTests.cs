using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KissLog.AspNet.Web.Tests.KissLogHttpModuleTests
{
    [TestClass]
    public class OnErrorTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionForNullHttpContext()
        {
            KissLogHttpModule module = new KissLogHttpModule();

            module.OnError(null);
        }

        [TestMethod]
        public void DoesNotThrowExceptionWhenServerIsNull()
        {
            var httpContext = Helpers.MockHttpContext();
            httpContext.Setup(p => p.Server).Returns((HttpServerUtilityBase)null);

            KissLogHttpModule module = new KissLogHttpModule();
            module.OnError(httpContext.Object);
        }

        [TestMethod]
        public void GetsOrAddsTheLoggerToHttpContext()
        {
            var server = new Mock<HttpServerUtilityBase>();
            server.Setup(p => p.GetLastError()).Returns(new Exception());

            var httpContext = Helpers.MockHttpContext();
            httpContext.Setup(p => p.Server).Returns(server.Object);

            KissLogHttpModule module = new KissLogHttpModule();
            module.OnError(httpContext.Object);

            var dictionary = httpContext.Object.Items[LoggerFactory.DictionaryKey] as IDictionary<string, Logger>;

            Assert.IsNotNull(dictionary);
            Assert.AreEqual(1, dictionary.Count);
        }

        [TestMethod]
        public void LogsTheServerGetLastError()
        {
            var ex = new Exception($"Exception {Guid.NewGuid()}");

            var server = new Mock<HttpServerUtilityBase>();
            server.Setup(p => p.GetLastError()).Returns(ex);

            var httpContext = Helpers.MockHttpContext();
            httpContext.Setup(p => p.Server).Returns(server.Object);

            KissLogHttpModule module = new KissLogHttpModule();
            module.OnError(httpContext.Object);

            var dictionary = httpContext.Object.Items[LoggerFactory.DictionaryKey] as IDictionary<string, Logger>;
            Logger logger = dictionary[Constants.DefaultLoggerCategoryName];

            Exception capturedException = logger.DataContainer.Exceptions.First();
            LogMessage message = logger.DataContainer.LogMessages.First();

            Assert.AreEqual(ex.Message, capturedException.Message);
            Assert.IsTrue(message.Message.Contains(ex.Message));
        }
    }
}
