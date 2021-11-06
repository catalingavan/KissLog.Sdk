using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace KissLog.AspNet.Web.Tests
{
    internal static class Helpers
    {
        public static List<Claim> GenerateClaims(List<KeyValuePair<string, string>> items)
        {
            return items.Select(p => new Claim(p.Key, p.Value)).ToList();
        }

        public static NameValueCollection GenerateNameValueCollection(List<KeyValuePair<string, string>> items)
        {
            NameValueCollection result = new NameValueCollection();
            foreach (var item in items)
                result.Add(item.Key, item.Value);

            return result;
        }

        public static HttpCookieCollection GenerateHttpCookieCollection(List<KeyValuePair<string, string>> items)
        {
            HttpCookieCollection result = new HttpCookieCollection();
            foreach (var item in items)
                result.Add(new HttpCookie(item.Key, item.Value));

            return result;
        }

        public static Mock<HttpContextBase> MockHttpContext(string responseBody = null, string inputStream = null)
        {
            if (responseBody == null)
                responseBody = $"ResponseBody {Guid.NewGuid()}";

            if (inputStream == null)
                inputStream = $"InputStream {Guid.NewGuid()}";

            var ms = new MirrorStreamDecorator(new MemoryStream());
            var sw = new StreamWriter(ms);
            sw.Write(responseBody);
            sw.Flush();

            var httpRequest = new Mock<HttpRequestBase>();
            httpRequest.Setup(p => p.Url).Returns(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.HttpMethod).Returns("POST");
            httpRequest.Setup(p => p.InputStream).Returns(new MemoryStream(Encoding.UTF8.GetBytes(inputStream)));
            httpRequest.Setup(p => p.Headers).Returns(GenerateNameValueCollection(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Content-Type", "application/json")
            }));

            var httpResponse = new Mock<HttpResponseBase>();
            httpResponse.Setup(p => p.StatusCode).Returns(204);
            httpResponse.SetupProperty(p => p.Filter, ms);
            httpResponse.Setup(p => p.Headers).Returns(GenerateNameValueCollection(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Content-Type", "application/json")
            }));

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(p => p.Request).Returns(httpRequest.Object);
            httpContext.Setup(p => p.Response).Returns(httpResponse.Object);
            httpContext.Setup(p => p.Items).Returns(new Dictionary<string, object>());

            return httpContext;
        }
    }
}
