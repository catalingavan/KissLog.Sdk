using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Web;

namespace KissLog.AspNet.Web.Tests.KissLogHttpModuleTests
{
    [TestClass]
    public class OnPreRequestHandlerExecuteTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionForNullHttpContext()
        {
            KissLogHttpModule module = new KissLogHttpModule();

            module.OnPreRequestHandlerExecute(null);
        }

        [TestMethod]
        public void DoesNotThrowExceptionForNullResponse()
        {
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(p => p.Response).Returns((HttpResponseBase)null);

            KissLogHttpModule module = new KissLogHttpModule();

            module.OnPreRequestHandlerExecute(httpContext.Object);
        }

        [TestMethod]
        public void DoesNotThrowExceptionForNullResponseFilter()
        {
            var httpResponse = new Mock<HttpResponseBase>();
            httpResponse.Setup(p => p.Filter).Returns((Stream)null);

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(p => p.Response).Returns(httpResponse.Object);

            KissLogHttpModule module = new KissLogHttpModule();

            module.OnPreRequestHandlerExecute(httpContext.Object);
        }

        [TestMethod]
        public void UpdatesTheResponseFilterToMirrorStreamDecorator()
        {
            Stream ms = new MemoryStream();
            var httpResponse = new Mock<HttpResponseBase>();
            httpResponse.SetupProperty(p => p.Filter, ms);

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(p => p.Response).Returns(httpResponse.Object);

            KissLogHttpModule module = new KissLogHttpModule();

            module.OnPreRequestHandlerExecute(httpContext.Object);

            Assert.IsInstanceOfType(httpContext.Object.Response.Filter, typeof(MirrorStreamDecorator));
        }
    }
}
