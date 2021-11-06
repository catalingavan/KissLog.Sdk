using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Web;

namespace KissLog.AspNet.Web.Tests.KissLogHttpModuleTests
{
    [TestClass]
    public class OnBeginRequestTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionForNullHttpContext()
        {
            KissLogHttpModule module = new KissLogHttpModule();

            module.OnBeginRequest(null);
        }

        [TestMethod]
        public void DoesNotThrowExceptionForNullRequest()
        {
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(p => p.Request).Returns((HttpRequestBase)null);

            KissLogHttpModule module = new KissLogHttpModule();

            module.OnBeginRequest(httpContext.Object);
        }

        [TestMethod]
        public void GetsOrAddsTheLoggerToHttpContext()
        {
            var httpContext = Helpers.MockHttpContext();

            KissLogHttpModule module = new KissLogHttpModule();
            module.OnBeginRequest(httpContext.Object);

            var dictionary = httpContext.Object.Items[LoggerFactory.DictionaryKey] as IDictionary<string, Logger>;

            Assert.IsNotNull(dictionary);
            Assert.AreEqual(1, dictionary.Count);
        }

        [TestMethod]
        public void UpdatesTheLoggerHttpProperties()
        {
            var httpContext = Helpers.MockHttpContext();

            KissLogHttpModule module = new KissLogHttpModule();
            module.OnBeginRequest(httpContext.Object);

            var dictionary = httpContext.Object.Items[LoggerFactory.DictionaryKey] as IDictionary<string, Logger>;

            Logger logger = dictionary[Constants.DefaultLoggerCategoryName];

            Assert.IsNotNull(logger.DataContainer.HttpProperties);
        }

        [TestMethod]
        public void NotifiesListeners()
        {
            KissLog.Tests.Common.CommonTestHelpers.ResetContext();

            List<KissLog.Http.HttpRequest> onBeginRequestArgs = new List<KissLog.Http.HttpRequest>();

            KissLogConfiguration.Listeners.Add(new KissLog.Tests.Common.CustomLogListener(onBeginRequest: (KissLog.Http.HttpRequest arg) => { onBeginRequestArgs.Add(arg); }));

            var httpContext = Helpers.MockHttpContext();

            KissLogHttpModule module = new KissLogHttpModule();
            module.OnBeginRequest(httpContext.Object);

            Assert.AreEqual(1, onBeginRequestArgs.Count);
        }
    }
}
