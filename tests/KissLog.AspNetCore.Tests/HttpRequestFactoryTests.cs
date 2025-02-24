using KissLog.AspNetCore.ReadInputStream;
using KissLog.AspNetCore.Tests.Collections;
using KissLog.Tests.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace KissLog.AspNetCore.Tests
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
        public void EmptyHttpRequestDoesNotThrowException()
        {
            Uri url = UrlParser.GenerateUri($"/Home/Index/{Guid.NewGuid()}");

            var httpRequest = new Mock<HttpRequest>();
            httpRequest.SetUrl(url);
            httpRequest.Setup(p => p.Method).Returns("GET");

            var result = HttpRequestFactory.Create(httpRequest.Object);
        }

        [TestMethod]
        public void UrlIsCopied()
        {
            Uri url = UrlParser.GenerateUri($"/Home/Index/{Guid.NewGuid()}?param1={Guid.NewGuid()}");

            var httpRequest = new Mock<HttpRequest>();
            httpRequest.SetUrl(url);
            httpRequest.Setup(p => p.Method).Returns("GET");

            var result = HttpRequestFactory.Create(httpRequest.Object);

            Assert.AreEqual(url.ToString(), result.Url.ToString());
        }

        [TestMethod]
        [DataRow("GET")]
        [DataRow("POST")]
        [DataRow("PUT")]
        public void HttpMethodIsCopied(string httpMethod)
        {
            var httpRequest = new Mock<HttpRequest>();
            httpRequest.SetUrl(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.Method).Returns(httpMethod);

            var result = HttpRequestFactory.Create(httpRequest.Object);

            Assert.AreEqual(httpMethod, result.HttpMethod);
        }

        [TestMethod]
        public void UserAgentIsCopied()
        {
            string userAgent = $"UserAgent {Guid.NewGuid()}";

            var httpRequest = new Mock<HttpRequest>();
            httpRequest.SetUrl(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.Method).Returns("GET");
            httpRequest.Setup(p => p.Headers).Returns(new CustomHeaderCollection(new Dictionary<string, StringValues>
            {
                { HeaderNames.UserAgent, userAgent }
            }));

            var result = HttpRequestFactory.Create(httpRequest.Object);

            Assert.AreEqual(userAgent, result.UserAgent);
        }

        [TestMethod]
        public void ReferrerIsCopied()
        {
            string referrer = $"Referer {Guid.NewGuid()}";

            var httpRequest = new Mock<HttpRequest>();
            httpRequest.SetUrl(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.Method).Returns("GET");
            httpRequest.Setup(p => p.Headers).Returns(new CustomHeaderCollection(new Dictionary<string, StringValues>
            {
                { HeaderNames.Referer, referrer }
            }));

            var result = HttpRequestFactory.Create(httpRequest.Object);

            Assert.AreEqual(referrer, result.HttpReferer);
        }

        [TestMethod]
        public void RemoteIpAddressIsCopied()
        {
            string ipAddress = "82.116.36.117";

            var connectionInfo = new Mock<ConnectionInfo>();
            connectionInfo.Setup(p => p.RemoteIpAddress).Returns(IPAddress.Parse(ipAddress));

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(p => p.Connection).Returns(connectionInfo.Object);

            var httpRequest = new Mock<HttpRequest>();
            httpRequest.SetUrl(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.Method).Returns("GET");
            httpRequest.Setup(p => p.HttpContext).Returns(httpContext.Object);

            var result = HttpRequestFactory.Create(httpRequest.Object);

            Assert.AreEqual(ipAddress, result.RemoteAddress);
        }

        [TestMethod]
        [DataRow("X-Forwarded-For", "203.2.64.59", "203.2.64.59")]
        [DataRow("x-forwarded-for", "187.122.27.32, 203.2.64.59", "187.122.27.32")]
        public void XForwardedForHasIsCopied(string headerName, string headerValue, string expectedValue)
        {
            string ipAddress = "82.116.36.117";

            var connectionInfo = new Mock<ConnectionInfo>();
            connectionInfo.Setup(p => p.RemoteIpAddress).Returns(IPAddress.Parse(ipAddress));

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(p => p.Connection).Returns(connectionInfo.Object);

            var httpRequest = new Mock<HttpRequest>();
            httpRequest.SetUrl(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.Method).Returns("GET");
            httpRequest.Setup(p => p.HttpContext).Returns(httpContext.Object);
            httpRequest.Setup(p => p.Headers).Returns(new CustomHeaderCollection(new Dictionary<string, StringValues>
            {
                { headerName, headerValue }
            }));

            var result = HttpRequestFactory.Create(httpRequest.Object);

            Assert.AreEqual(result.RemoteAddress, expectedValue);
        }

        [TestMethod]
        public void MachineNameIsCopied()
        {
            var httpRequest = new Mock<HttpRequest>();
            httpRequest.SetUrl(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.Method).Returns("GET");

            var result = HttpRequestFactory.Create(httpRequest.Object);

            string machineName = InternalHelpers.GetMachineName();

            Assert.AreEqual(machineName, result.MachineName);
        }

        [TestMethod]
        public void HeadersAreCopied()
        {
            var value = CommonTestHelpers.GenerateList(5);

            var httpRequest = new Mock<HttpRequest>();
            httpRequest.SetUrl(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.Method).Returns("GET");
            httpRequest.Setup(p => p.Headers).Returns(new CustomHeaderCollection(value.ToStringValuesDictionary()));

            var result = HttpRequestFactory.Create(httpRequest.Object);

            Assert.AreEqual(JsonSerializer.Serialize(value), JsonSerializer.Serialize(result.Properties.Headers));
        }

        [TestMethod]
        public void CookiesAreCopied()
        {
            var value = CommonTestHelpers.GenerateList(5);
            var dictionary = CommonTestHelpers.GenerateDictionary(value);

            var httpRequest = new Mock<HttpRequest>();
            httpRequest.SetUrl(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.Method).Returns("GET");
            httpRequest.Setup(p => p.Cookies).Returns(new CustomRequestCookieCollection(dictionary));

            var result = HttpRequestFactory.Create(httpRequest.Object);

            Assert.AreEqual(JsonSerializer.Serialize(value), JsonSerializer.Serialize(result.Properties.Cookies));
        }

        [TestMethod]
        public void QueryStringIsCopied()
        {
            var value = CommonTestHelpers.GenerateList(5);
            var qs = new FormUrlEncodedContent(value).ReadAsStringAsync().Result;

            var dictionary = CommonTestHelpers.GenerateDictionary(value);

            var httpRequest = new Mock<HttpRequest>();
            httpRequest.SetUrl(UrlParser.GenerateUri("/Home/Index?" + qs));
            httpRequest.Setup(p => p.Method).Returns("GET");

            var result = HttpRequestFactory.Create(httpRequest.Object);

            Assert.AreEqual(JsonSerializer.Serialize(value), JsonSerializer.Serialize(result.Properties.QueryString));
        }

        [TestMethod]
        public void FormDataIsCopied()
        {
            KissLog.Tests.Common.CommonTestHelpers.ResetContext();

            var value = CommonTestHelpers.GenerateList(5);

            var dictionary = CommonTestHelpers.GenerateDictionary(value);

            var httpRequest = new Mock<HttpRequest>();
            httpRequest.SetUrl(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.Method).Returns("GET");
            httpRequest.Setup(p => p.HasFormContentType).Returns(true);
            httpRequest.Setup(p => p.Form).Returns(new CustomFormCollection(value.ToStringValuesDictionary()));

            var result = HttpRequestFactory.Create(httpRequest.Object);

            Assert.AreEqual(JsonSerializer.Serialize(value), JsonSerializer.Serialize(result.Properties.FormData));
        }

        [TestMethod]
        public void InputStreamIsCopied()
        {
            KissLog.Tests.Common.CommonTestHelpers.ResetContext();

            string body = $"Input stream {Guid.NewGuid()}";
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(body));

            var httpRequest = new Mock<HttpRequest>();
            httpRequest.SetUrl(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.Method).Returns("GET");
            httpRequest.Setup(p => p.Body).Returns(ms);
            httpRequest.Setup(p => p.Headers).Returns(new CustomHeaderCollection(new Dictionary<string, StringValues>
            {
                { HeaderNames.ContentType, "text/plain" }
            }));

            ModuleInitializer.ReadInputStreamProvider = new EnableBufferingReadInputStreamProvider();

            var result = HttpRequestFactory.Create(httpRequest.Object);

            Assert.AreEqual(body, result.Properties.InputStream);
        }

        [TestMethod]
        public void EvaluatesOptionsShouldLogFormData()
        {
            KissLog.Tests.Common.CommonTestHelpers.ResetContext();

            KissLogConfiguration.Options.ShouldLogFormData((KissLog.Http.HttpRequest args) => false);

            var value = CommonTestHelpers.GenerateList(5);

            var dictionary = CommonTestHelpers.GenerateDictionary(value);

            var httpRequest = new Mock<HttpRequest>();
            httpRequest.SetUrl(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.Method).Returns("GET");
            httpRequest.Setup(p => p.HasFormContentType).Returns(true);
            httpRequest.Setup(p => p.Form).Returns(new CustomFormCollection(value.ToStringValuesDictionary()));

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

            var httpRequest = new Mock<HttpRequest>();
            httpRequest.SetUrl(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.Method).Returns("GET");
            httpRequest.Setup(p => p.Body).Returns(ms);
            httpRequest.Setup(p => p.Headers).Returns(new CustomHeaderCollection(new Dictionary<string, StringValues>
            {
                { HeaderNames.ContentType, "text/plain" }
            }));

            ModuleInitializer.ReadInputStreamProvider = new EnableBufferingReadInputStreamProvider();

            var result = HttpRequestFactory.Create(httpRequest.Object);

            Assert.IsNull(result.Properties.InputStream);
        }
    }
}
