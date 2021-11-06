using KissLog.Http;
using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.Json;

namespace KissLog.Tests.Http
{
    [TestClass]
    public class HttpPropertiesTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullCreateOptionsThrowsException()
        {
            HttpProperties item = new HttpProperties(null);
        }

        [TestMethod]
        public void HttpRequestConstructorUpdatesProperties()
        {
            HttpRequest httpRequest = new HttpRequest(new HttpRequest.CreateOptions
            {
                HttpMethod = "GET",
                Url = UrlParser.GenerateUri(null)
            });

            HttpProperties item = new HttpProperties(httpRequest);

            Assert.AreSame(httpRequest, item.Request);
        }

        [TestMethod]
        public void RequestIsNotNull()
        {
            HttpProperties item = new HttpProperties(new HttpRequest(new HttpRequest.CreateOptions
            {
                HttpMethod = "GET",
                Url = UrlParser.GenerateUri(null)
            }));

            Assert.IsNotNull(item.Request);
        }

        [TestMethod]
        public void ResponseIsNull()
        {
            HttpProperties item = new HttpProperties(new HttpRequest(new HttpRequest.CreateOptions
            {
                HttpMethod = "GET",
                Url = UrlParser.GenerateUri(null)
            }));

            Assert.IsNull(item.Response);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetResponseWithNullArgumentsThrowsException()
        {
            HttpProperties item = new HttpProperties(new HttpRequest(new HttpRequest.CreateOptions
            {
                HttpMethod = "GET",
                Url = UrlParser.GenerateUri(null)
            }));

            item.SetResponse(null);
        }

        [TestMethod]
        public void SetResponseUpdatesProperty()
        {
            HttpResponse httpResponse = new HttpResponse(new HttpResponse.CreateOptions());

            HttpProperties item = new HttpProperties(new HttpRequest(new HttpRequest.CreateOptions
            {
                HttpMethod = "GET",
                Url = UrlParser.GenerateUri(null)
            }));

            item.SetResponse(httpResponse);

            Assert.AreSame(httpResponse, item.Response);
        }

        [TestMethod]
        public void CloneCopiesAllTheProperties()
        {
            HttpProperties item = CommonTestHelpers.Factory.CreateHttpProperties();

            HttpProperties clone = item.Clone();

            Assert.AreEqual(JsonSerializer.Serialize(item), JsonSerializer.Serialize(clone));
        }
    }
}
