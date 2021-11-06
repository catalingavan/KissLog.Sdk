using KissLog.Http;
using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Text.Json;

namespace KissLog.Tests.Http
{
    [TestClass]
    public class ResponsePropertiesTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullCreateOptionsThrowsException()
        {
            ResponseProperties item = new ResponseProperties(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NegativeContentLengthThrowsException()
        {
            var options = new ResponseProperties.CreateOptions
            {
                Headers = CommonTestHelpers.GenerateList(2),
                ContentLength = -1
            };

            ResponseProperties item = new ResponseProperties(options);
        }

        [TestMethod]
        public void CreateOptionsConstructorUpdatesProperties()
        {
            var options = new ResponseProperties.CreateOptions
            {
                Headers = CommonTestHelpers.GenerateList(2),
                ContentLength = 10000
            };

            ResponseProperties item = new ResponseProperties(options);

            Assert.AreEqual(JsonSerializer.Serialize(options.Headers), JsonSerializer.Serialize(item.Headers));
            Assert.AreEqual(options.ContentLength, item.ContentLength);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        public void NullKeyValuesAreNotCopied(string keyName)
        {
            var options = new ResponseProperties.CreateOptions
            {
                Headers = CommonTestHelpers.GenerateList(keyName, 2),
                ContentLength = 10000
            };

            ResponseProperties item = new ResponseProperties(options);

            Assert.AreEqual(0, item.Headers.Count());
        }

        [TestMethod]
        public void HeadersIsNotNull()
        {
            ResponseProperties item = new ResponseProperties(new ResponseProperties.CreateOptions
            {
                Headers = null
            });

            Assert.IsNotNull(item.Headers);
        }

        [TestMethod]
        public void CloneCopiesAllTheProperties()
        {
            ResponseProperties item = CommonTestHelpers.Factory.CreateResponseProperties();

            ResponseProperties clone = item.Clone();

            Assert.AreEqual(JsonSerializer.Serialize(item), JsonSerializer.Serialize(clone));
        }
    }
}
