using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Text.Json;
using System.Web;

namespace KissLog.AspNet.Web.Tests
{
    [TestClass]
    public class HttpResponseFactoryTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateThrowsExceptionForNullHttpResponse()
        {
            HttpResponseFactory.Create(null, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NegativeContentLengthThrowsException()
        {
            var httpResponse = new Mock<HttpResponseBase>();

            var result = HttpResponseFactory.Create(httpResponse.Object, -1);
        }

        [TestMethod]
        public void StatusCodeIsCopied()
        {
            var httpResponse = new Mock<HttpResponseBase>();
            httpResponse.Setup(p => p.StatusCode).Returns(404);

            var result = HttpResponseFactory.Create(httpResponse.Object, 0);

            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public void HeadersAreCopied()
        {
            var value = KissLog.Tests.Common.CommonTestHelpers.GenerateList(5);

            var httpResponse = new Mock<HttpResponseBase>();
            httpResponse.Setup(p => p.Headers).Returns(Helpers.GenerateNameValueCollection(value));

            var result = HttpResponseFactory.Create(httpResponse.Object, 0);

            Assert.AreEqual(JsonSerializer.Serialize(value), JsonSerializer.Serialize(result.Properties.Headers));
        }
    }
}
