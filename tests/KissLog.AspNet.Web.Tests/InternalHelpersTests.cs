using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Security.Claims;
using System.Web;

namespace KissLog.AspNet.Web.Tests
{
    [TestClass]
    public class InternalHelpersTests
    {
        [TestMethod]
        public void GetMachineNameDoesNotThrowExceptionForNullHttpRequest()
        {
            string result = InternalHelpers.GetMachineName(null);
        }

        [TestMethod]
        public void GetMachineNameReturnsEnvironmentMachineName()
        {
            string result = InternalHelpers.GetMachineName(null);

            Assert.AreEqual(Environment.MachineName, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetServerNameFromHttpRequestThrowsExceptionForNullHttpRequest()
        {
            InternalHelpers.GetServerNameFromHttpRequest(null);
        }

        [TestMethod]
        public void GetServerNameFromHttpRequestDoesNotThrowExceptionForNullServerVariables()
        {
            var httpRequest = new Mock<HttpRequestBase>();
            httpRequest.Setup(p => p.ServerVariables).Returns((NameValueCollection)null);

            InternalHelpers.GetServerNameFromHttpRequest(httpRequest.Object);
        }

        [TestMethod]
        [DataRow("server_name")]
        [DataRow("Server_Name")]
        [DataRow("SERVER_NAME")]
        public void GetServerNameFromHttpRequestReadsServerVariables(string keyName)
        {
            string serverName = Guid.NewGuid().ToString();

            NameValueCollection serverVariables = new NameValueCollection();
            serverVariables.Add(keyName, serverName);

            var httpRequest = new Mock<HttpRequestBase>();
            httpRequest.Setup(p => p.ServerVariables).Returns(serverVariables);

            string result = InternalHelpers.GetServerNameFromHttpRequest(httpRequest.Object);

            Assert.AreEqual(serverName, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadInputStreamThrowsExceptionForNullHttpRequest()
        {
            InternalHelpers.ReadInputStream(null);
        }

        [TestMethod]
        [DataRow("swxamxgstz")]
        [DataRow("gñlhjmdbon")]
        [DataRow("rÜqzißttda")]
        [DataRow("み（え）ふらこへかねりふ")]
        [DataRow("ДЕЦХМПЁРЩЖ")]
        [DataRow("ԿՍևՈԶՓոԾԱՂ")]
        [DataRow("ΧΚΘΛΧΦΡΛΑΒ")]
        [DataRow("ףבּךכּףשׂפכּפח")]
        [DataRow("ÜtihdtctlÇ")]
        [DataRow("ظمكيثصثستم")]
        public void ReadInputStream(string value)
        {
            string body = $"Input stream {Guid.NewGuid()} - {value}";

            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            sw.Write(body);
            sw.Flush();
            ms.Position = 0;

            var httpRequest = new Mock<HttpRequestBase>();
            httpRequest.Setup(p => p.Url).Returns(UrlParser.GenerateUri("/Home/Index"));
            httpRequest.Setup(p => p.HttpMethod).Returns("POST");
            httpRequest.Setup(p => p.InputStream).Returns(ms);

            var result = InternalHelpers.ReadInputStream(httpRequest.Object);

            Assert.AreEqual(body, result);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        public void NameValueCollectionToKeyValuePairIgnoresEmptyKeyNames(string keyName)
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add(keyName, Guid.NewGuid().ToString());

            var result = InternalHelpers.ToKeyValuePair(collection);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        public void HttpCookieCollectionToKeyValuePairIgnoresEmptyKeyNames(string keyName)
        {
            HttpCookieCollection collection = new HttpCookieCollection();
            collection.Add(new HttpCookie(keyName, Guid.NewGuid().ToString()));

            var result = InternalHelpers.ToKeyValuePair(collection);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        public void ClaimsIdentityToKeyValuePairIgnoresEmptyKeyNames(string keyName)
        {
            ClaimsIdentity collection = new ClaimsIdentity();
            collection.AddClaim(new Claim(keyName, Guid.NewGuid().ToString()));

            var result = InternalHelpers.ToKeyValuePair(collection);

            Assert.AreEqual(0, result.Count);
        }
    }
}
