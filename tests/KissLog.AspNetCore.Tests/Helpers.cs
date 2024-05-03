using KissLog.AspNetCore.Tests.Collections;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace KissLog.AspNetCore.Tests
{
    internal static class Helpers
    {
        public static Mock<HttpContext> MockHttpContext(string inputStream = null, string responseContentType = "text/plain")
        {
            if (inputStream == null)
                inputStream = $"InputStream {Guid.NewGuid()}";

            var httpRequest = new Mock<HttpRequest>();
            httpRequest.SetUrl(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.Method).Returns(HttpMethods.Post);
            httpRequest.Setup(p => p.Body).Returns(new MemoryStream(Encoding.UTF8.GetBytes(inputStream)));
            httpRequest.Setup(p => p.Headers).Returns(new CustomHeaderCollection(new Dictionary<string, StringValues>
            {
                { HeaderNames.ContentType, "text/plain" }
            }));

            var httpResponse = new Mock<HttpResponse>();
            httpResponse.Setup(p => p.StatusCode).Returns(204);
            httpResponse.Setup(p => p.Headers).Returns(new CustomHeaderCollection(new Dictionary<string, StringValues>
            {
                { HeaderNames.ContentType, responseContentType }
            }));
            httpResponse.SetupProperty(p => p.Body, new MemoryStream());

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(p => p.Request).Returns(httpRequest.Object);
            httpContext.Setup(p => p.Response).Returns(httpResponse.Object);
            httpContext.Setup(p => p.Items).Returns(new Dictionary<object, object>());

            return httpContext;
        }

        public static KissLogMiddleware MockMiddleware(string responseBody = null)
        {
            if (responseBody == null)
                responseBody = $"ResponseBody {Guid.NewGuid()}";

            var middleware = new KissLogMiddleware((innerHttpContext) =>
            {
                var content = System.Text.Encoding.UTF8.GetBytes(responseBody);
                innerHttpContext.Response.Body.Write(content, 0, content.Length);

                return Task.CompletedTask;
            });

            return middleware;
        }
    }
}
