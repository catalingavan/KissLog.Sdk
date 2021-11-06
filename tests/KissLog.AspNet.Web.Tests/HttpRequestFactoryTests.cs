using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Web;

namespace KissLog.AspNet.Web.Tests
{
    [TestClass]
    public class HttpRequestFactoryTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateThrowsExceptionForNullHttpRequest()
        {
            HttpRequestFactory.Create(null);
        }

        [TestMethod]
        public void UrlIsCopied()
        {
            Uri url = UrlParser.GenerateUri($"/Home/Index/{Guid.NewGuid()}");

            var httpRequest = new Mock<HttpRequestBase>();
            httpRequest.Setup(p => p.Url).Returns(url);
            httpRequest.Setup(p => p.HttpMethod).Returns("GET");

            var result = HttpRequestFactory.Create(httpRequest.Object);

            Assert.AreEqual(url.ToString(), result.Url.ToString());
        }

        [TestMethod]
        [DataRow("GET")]
        [DataRow("POST")]
        [DataRow("PUT")]
        public void HttpMethodIsCopied(string httpMethod)
        {
            var httpRequest = new Mock<HttpRequestBase>();
            httpRequest.Setup(p => p.Url).Returns(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.HttpMethod).Returns(httpMethod);

            var result = HttpRequestFactory.Create(httpRequest.Object);

            Assert.AreEqual(httpMethod, result.HttpMethod);
        }

        [TestMethod]
        public void UserAgentIsCopied()
        {
            string userAgent = $"UserAgent {Guid.NewGuid()}";
            
            var httpRequest = new Mock<HttpRequestBase>();
            httpRequest.Setup(p => p.Url).Returns(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.HttpMethod).Returns("GET");
            httpRequest.Setup(p => p.UserAgent).Returns(userAgent);

            var result = HttpRequestFactory.Create(httpRequest.Object);

            Assert.AreEqual(userAgent, result.UserAgent);
        }

        [TestMethod]
        public void UrlReferrerIsCopied()
        {
            Uri urlReferrer = new Uri("https://google.com");

            var httpRequest = new Mock<HttpRequestBase>();
            httpRequest.Setup(p => p.Url).Returns(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.HttpMethod).Returns("GET");
            httpRequest.Setup(p => p.UrlReferrer).Returns(urlReferrer);

            var result = HttpRequestFactory.Create(httpRequest.Object);

            Assert.AreEqual(urlReferrer.ToString(), result.HttpReferer);
        }

        [TestMethod]
        public void UserHostAddressIsCopied()
        {
            string userHostAddress = $"UserHostAddress {Guid.NewGuid()}";

            var httpRequest = new Mock<HttpRequestBase>();
            httpRequest.Setup(p => p.Url).Returns(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.HttpMethod).Returns("GET");
            httpRequest.Setup(p => p.UserHostAddress).Returns(userHostAddress);

            var result = HttpRequestFactory.Create(httpRequest.Object);

            Assert.AreEqual(userHostAddress, result.RemoteAddress);
        }

        [TestMethod]
        public void MachineNameIsCopied()
        {
            NameValueCollection serverVariables = new NameValueCollection();
            serverVariables.Add("SERVER_NAME", Guid.NewGuid().ToString());

            var httpRequest = new Mock<HttpRequestBase>();
            httpRequest.Setup(p => p.Url).Returns(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.HttpMethod).Returns("GET");
            httpRequest.Setup(p => p.ServerVariables).Returns(serverVariables);

            var result = HttpRequestFactory.Create(httpRequest.Object);

            string machineName = InternalHelpers.GetMachineName(httpRequest.Object);

            Assert.AreEqual(machineName, result.MachineName);
        }

        [TestMethod]
        public void HeadersAreCopiedFromUnvalidatedRequestValues()
        {
            var value = KissLog.Tests.Common.CommonTestHelpers.GenerateList(5);
            var unvalidatedValue = KissLog.Tests.Common.CommonTestHelpers.GenerateList(5);

            var httpRequest = new Mock<HttpRequestBase>();
            httpRequest.Setup(p => p.Url).Returns(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.HttpMethod).Returns("GET");
            httpRequest.Setup(p => p.Headers).Returns(Helpers.GenerateNameValueCollection(value));
            httpRequest.Setup(p => p.Unvalidated.Headers).Returns(Helpers.GenerateNameValueCollection(unvalidatedValue));

            var result = HttpRequestFactory.Create(httpRequest.Object);

            Assert.AreEqual(JsonSerializer.Serialize(unvalidatedValue), JsonSerializer.Serialize(result.Properties.Headers));
        }

        [TestMethod]
        public void CookiesAreCopiedFromUnvalidatedRequestValues()
        {
            var value = KissLog.Tests.Common.CommonTestHelpers.GenerateList(5);
            var unvalidatedValue = KissLog.Tests.Common.CommonTestHelpers.GenerateList(5);

            var httpRequest = new Mock<HttpRequestBase>();
            httpRequest.Setup(p => p.Url).Returns(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.HttpMethod).Returns("GET");
            httpRequest.Setup(p => p.Cookies).Returns(Helpers.GenerateHttpCookieCollection(value));
            httpRequest.Setup(p => p.Unvalidated.Cookies).Returns(Helpers.GenerateHttpCookieCollection(unvalidatedValue));

            var result = HttpRequestFactory.Create(httpRequest.Object);

            Assert.AreEqual(JsonSerializer.Serialize(unvalidatedValue), JsonSerializer.Serialize(result.Properties.Cookies));
        }

        [TestMethod]
        public void QueryStringIsCopiedFromUnvalidatedRequestValues()
        {
            var value = KissLog.Tests.Common.CommonTestHelpers.GenerateList(5);
            var unvalidatedValue = KissLog.Tests.Common.CommonTestHelpers.GenerateList(5);

            var httpRequest = new Mock<HttpRequestBase>();
            httpRequest.Setup(p => p.Url).Returns(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.HttpMethod).Returns("GET");
            httpRequest.Setup(p => p.QueryString).Returns(Helpers.GenerateNameValueCollection(value));
            httpRequest.Setup(p => p.Unvalidated.QueryString).Returns(Helpers.GenerateNameValueCollection(unvalidatedValue));

            var result = HttpRequestFactory.Create(httpRequest.Object);

            Assert.AreEqual(JsonSerializer.Serialize(unvalidatedValue), JsonSerializer.Serialize(result.Properties.QueryString));
        }

        [TestMethod]
        public void FormDataIsCopiedFromUnvalidatedRequestValues()
        {
            var value = KissLog.Tests.Common.CommonTestHelpers.GenerateList(5);
            var unvalidatedValue = KissLog.Tests.Common.CommonTestHelpers.GenerateList(5);

            var httpRequest = new Mock<HttpRequestBase>();
            httpRequest.Setup(p => p.Url).Returns(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.HttpMethod).Returns("GET");
            httpRequest.Setup(p => p.Form).Returns(Helpers.GenerateNameValueCollection(value));
            httpRequest.Setup(p => p.Unvalidated.Form).Returns(Helpers.GenerateNameValueCollection(unvalidatedValue));

            var result = HttpRequestFactory.Create(httpRequest.Object);

            Assert.AreEqual(JsonSerializer.Serialize(unvalidatedValue), JsonSerializer.Serialize(result.Properties.FormData));
        }

        [TestMethod]
        public void ServerVariablesAreCopied()
        {
            var value = KissLog.Tests.Common.CommonTestHelpers.GenerateList(5);

            var httpRequest = new Mock<HttpRequestBase>();
            httpRequest.Setup(p => p.Url).Returns(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.HttpMethod).Returns("GET");
            httpRequest.Setup(p => p.ServerVariables).Returns(Helpers.GenerateNameValueCollection(value));

            var result = HttpRequestFactory.Create(httpRequest.Object);

            Assert.AreEqual(JsonSerializer.Serialize(value), JsonSerializer.Serialize(result.Properties.ServerVariables));
        }

        [TestMethod]
        public void InputStreamIsCopied()
        {
            string body = $"Input stream {Guid.NewGuid()}";

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(body));

            var httpRequest = new Mock<HttpRequestBase>();
            httpRequest.Setup(p => p.Url).Returns(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.HttpMethod).Returns("POST");
            httpRequest.Setup(p => p.InputStream).Returns(ms);
            httpRequest.Setup(p => p.Unvalidated.Headers).Returns(Helpers.GenerateNameValueCollection(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Content-Type", "text/plain")
            }));

            var result = HttpRequestFactory.Create(httpRequest.Object);

            Assert.AreEqual(body, result.Properties.InputStream);
        }

        [TestMethod]
        public void EvaluatesOptionsShouldLogFormData()
        {
            KissLog.Tests.Common.CommonTestHelpers.ResetContext();

            KissLogConfiguration.Options.ShouldLogFormData((KissLog.Http.HttpRequest args) => false);

            var value = KissLog.Tests.Common.CommonTestHelpers.GenerateList(5);

            var httpRequest = new Mock<HttpRequestBase>();
            httpRequest.Setup(p => p.Url).Returns(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.HttpMethod).Returns("GET");
            httpRequest.Setup(p => p.Unvalidated.Form).Returns(Helpers.GenerateNameValueCollection(value));

            var result = HttpRequestFactory.Create(httpRequest.Object);

            Assert.AreEqual(0, result.Properties.FormData.Count());
        }

        [TestMethod]
        public void EvaluatesOptionsShouldLogInputStream()
        {
            KissLog.Tests.Common.CommonTestHelpers.ResetContext();

            KissLogConfiguration.Options.ShouldLogInputStream((KissLog.Http.HttpRequest args) => false);

            string body = $"Input stream {Guid.NewGuid()}";

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(body));

            var httpRequest = new Mock<HttpRequestBase>();
            httpRequest.Setup(p => p.Url).Returns(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.HttpMethod).Returns("POST");
            httpRequest.Setup(p => p.InputStream).Returns(ms);
            httpRequest.Setup(p => p.Unvalidated.Headers).Returns(Helpers.GenerateNameValueCollection(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Content-Type", "text/plain")
            }));

            var result = HttpRequestFactory.Create(httpRequest.Object);

            Assert.IsNull(result.Properties.InputStream);
        }
    }
}
