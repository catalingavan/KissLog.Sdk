using KissLog.Http;
using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Text.Json;

namespace KissLog.Tests.Http
{
    [TestClass]
    public class RequestPropertiesTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullCreateOptionsThrowsException()
        {
            RequestProperties item = new RequestProperties(null);
        }

        [TestMethod]
        public void CreateOptionsConstructorUpdatesProperties()
        {
            var options = new RequestProperties.CreateOptions
            {
                Headers = CommonTestHelpers.GenerateList(2),
                Cookies = CommonTestHelpers.GenerateList(2),
                QueryString = CommonTestHelpers.GenerateList(2),
                FormData = CommonTestHelpers.GenerateList(2),
                ServerVariables = CommonTestHelpers.GenerateList(2),
                Claims = CommonTestHelpers.GenerateList(2),
                InputStream = $"Input stream {Guid.NewGuid()}"
            };

            RequestProperties item = new RequestProperties(options);

            Assert.AreEqual(JsonSerializer.Serialize(options.Headers), JsonSerializer.Serialize(item.Headers));
            Assert.AreEqual(JsonSerializer.Serialize(options.Cookies), JsonSerializer.Serialize(item.Cookies));
            Assert.AreEqual(JsonSerializer.Serialize(options.QueryString), JsonSerializer.Serialize(item.QueryString));
            Assert.AreEqual(JsonSerializer.Serialize(options.FormData), JsonSerializer.Serialize(item.FormData));
            Assert.AreEqual(JsonSerializer.Serialize(options.ServerVariables), JsonSerializer.Serialize(item.ServerVariables));
            Assert.AreEqual(JsonSerializer.Serialize(options.Claims), JsonSerializer.Serialize(item.Claims));
            Assert.AreEqual(options.InputStream, item.InputStream);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        public void NullKeyValuesAreNotCopied(string keyName)
        {
            var options = new RequestProperties.CreateOptions
            {
                Headers = CommonTestHelpers.GenerateList(keyName, 2),
                Cookies = CommonTestHelpers.GenerateList(keyName, 2),
                QueryString = CommonTestHelpers.GenerateList(keyName, 2),
                FormData = CommonTestHelpers.GenerateList(keyName, 2),
                ServerVariables = CommonTestHelpers.GenerateList(keyName, 2),
                Claims = CommonTestHelpers.GenerateList(keyName, 2)
            };

            RequestProperties item = new RequestProperties(options);

            Assert.AreEqual(0, item.Headers.Count());
            Assert.AreEqual(0, item.Cookies.Count());
            Assert.AreEqual(0, item.QueryString.Count());
            Assert.AreEqual(0, item.FormData.Count());
            Assert.AreEqual(0, item.ServerVariables.Count());
            Assert.AreEqual(0, item.Claims.Count());
        }

        [TestMethod]
        public void NullHeaderKeysAreNotCopied()
        {
            RequestProperties item = new RequestProperties(new RequestProperties.CreateOptions
            {
                Headers = null
            });

            Assert.IsNotNull(item.Headers);
        }

        [TestMethod]
        public void CookiesIsNotNull()
        {
            RequestProperties item = new RequestProperties(new RequestProperties.CreateOptions
            {
                Cookies = null
            });

            Assert.IsNotNull(item.Cookies);
        }

        [TestMethod]
        public void QueryStringIsNotNull()
        {
            RequestProperties item = new RequestProperties(new RequestProperties.CreateOptions
            {
                QueryString = null
            });

            Assert.IsNotNull(item.QueryString);
        }

        [TestMethod]
        public void FormDataIsNotNull()
        {
            RequestProperties item = new RequestProperties(new RequestProperties.CreateOptions
            {
                FormData = null
            });

            Assert.IsNotNull(item.FormData);
        }

        [TestMethod]
        public void ServerVariablesIsNotNull()
        {
            RequestProperties item = new RequestProperties(new RequestProperties.CreateOptions
            {
                ServerVariables = null
            });

            Assert.IsNotNull(item.ServerVariables);
        }

        [TestMethod]
        public void ClaimsIsNotNull()
        {
            RequestProperties item = new RequestProperties(new RequestProperties.CreateOptions
            {
                Claims = null
            });

            Assert.IsNotNull(item.Claims);
        }

        [TestMethod]
        public void CloneCopiesAllTheProperties()
        {
            RequestProperties item = CommonTestHelpers.Factory.CreateRequestProperties();

            RequestProperties clone = item.Clone();

            Assert.AreEqual(JsonSerializer.Serialize(item), JsonSerializer.Serialize(clone));
        }
    }
}
