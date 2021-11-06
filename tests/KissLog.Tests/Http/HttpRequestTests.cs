using KissLog.Http;
using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.Json;

namespace KissLog.Tests.Http
{
    [TestClass]
    public class HttpRequestTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullCreateOptionsThrowsException()
        {
            HttpRequest item = new HttpRequest(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullUrlThrowsException()
        {
            HttpRequest item = new HttpRequest(new HttpRequest.CreateOptions
            {
                Url = null,
                HttpMethod = "GET"
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("*&^%()!@#")]
        public void NullOrInvalidHttpMethodThrowsException(string httpMethod)
        {
            HttpRequest item = new HttpRequest(new HttpRequest.CreateOptions
            {
                Url = UrlParser.GenerateUri(null),
                HttpMethod = httpMethod
            });
        }

        [TestMethod]
        public void IdIsNotNull()
        {
            HttpRequest item = new HttpRequest(new HttpRequest.CreateOptions
            {
                Url = UrlParser.GenerateUri(null),
                HttpMethod = "GET"
            });

            Assert.IsTrue(item.Id != Guid.Empty);
        }

        [TestMethod]
        public void StartDateTimeIsInUtcFormat()
        {
            HttpRequest item = new HttpRequest(new HttpRequest.CreateOptions
            {
                Url = UrlParser.GenerateUri(null),
                HttpMethod = "GET"
            });

            Assert.AreEqual(DateTimeKind.Utc, item.StartDateTime.Kind);
        }

        [TestMethod]
        public void StartDateTimeIsInThePast()
        {
            HttpRequest item = new HttpRequest(new HttpRequest.CreateOptions
            {
                Url = UrlParser.GenerateUri(null),
                HttpMethod = "GET"
            });

            Assert.IsTrue(item.StartDateTime < DateTime.UtcNow);
        }

        [TestMethod]
        public void StartDateTimeHasValue()
        {
            HttpRequest item = new HttpRequest(new HttpRequest.CreateOptions
            {
                Url = UrlParser.GenerateUri(null),
                HttpMethod = "GET"
            });

            Assert.IsTrue(item.StartDateTime.Year > default(DateTime).Year);
        }

        [TestMethod]
        public void CreateOptionsConstructorUpdatesProperties()
        {
            var options = new HttpRequest.CreateOptions
            {
                StartDateTime = DateTime.UtcNow.AddSeconds(-120),
                Url = UrlParser.GenerateUri(Guid.NewGuid().ToString()),
                HttpMethod = "GET",
                UserAgent = $"UserAgent {Guid.NewGuid()}",
                RemoteAddress = $"RemoteAddress {Guid.NewGuid()}",
                HttpReferer = $"HttpReferer {Guid.NewGuid()}",
                SessionId = $"SessionId {Guid.NewGuid()}",
                IsNewSession = true,
                MachineName = $"MachineName {Guid.NewGuid()}",
                Properties = new RequestProperties(new RequestProperties.CreateOptions())
            };

            HttpRequest item = new HttpRequest(options);

            Assert.AreEqual(options.StartDateTime, item.StartDateTime);
            Assert.AreEqual(options.Url.ToString(), item.Url.ToString());
            Assert.AreEqual(options.HttpMethod, item.HttpMethod);
            Assert.AreEqual(options.UserAgent, item.UserAgent);
            Assert.AreEqual(options.RemoteAddress, item.RemoteAddress);
            Assert.AreEqual(options.HttpReferer, item.HttpReferer);
            Assert.AreEqual(options.SessionId, item.SessionId);
            Assert.AreEqual(options.IsNewSession, item.IsNewSession);
            Assert.AreEqual(options.MachineName, item.MachineName);
            Assert.AreSame(options.Properties, item.Properties);
        }

        [TestMethod]
        public void PropertiesIsNotNull()
        {
            HttpRequest item = new HttpRequest(new HttpRequest.CreateOptions
            {
                Url = UrlParser.GenerateUri(null),
                HttpMethod = "GET",
                Properties = null
            });

            Assert.IsNotNull(item.Properties);
        }

        [TestMethod]
        [DataRow("get")]
        [DataRow(" Get")]
        [DataRow(" GeT ")]
        public void HttpMethodIsTransformedToUppercase(string httpMethod)
        {
            HttpRequest item = new HttpRequest(new HttpRequest.CreateOptions
            {
                Url = UrlParser.GenerateUri(null),
                HttpMethod = httpMethod
            });

            Assert.AreEqual(httpMethod.Trim().ToUpperInvariant(), item.HttpMethod);
        }

        [TestMethod]
        public void CloneCopiesAllTheProperties()
        {
            HttpRequest item = CommonTestHelpers.Factory.CreateHttpRequest();

            HttpRequest clone = item.Clone();

            Assert.AreEqual(JsonSerializer.Serialize(item), JsonSerializer.Serialize(clone));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetPropertiesThrowsExceptionForNullArgument()
        {
            HttpRequest item = new HttpRequest(new HttpRequest.CreateOptions
            {
                Url = UrlParser.GenerateUri(null),
                HttpMethod = "GET"
            });

            item.SetProperties(null);
        }
    }
}
