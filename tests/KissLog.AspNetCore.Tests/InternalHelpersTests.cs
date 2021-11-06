using KissLog.AspNetCore.Tests.Collections;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace KissLog.AspNetCore.Tests
{
    [TestClass]
    public class InternalHelpersTests
    {
        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        public void RequestCookieCollectionToKeyValuePairIgnoresEmptyKeyNames(string keyName)
        {
            CustomRequestCookieCollection collection = new CustomRequestCookieCollection(new Dictionary<string, string>
            {
                { keyName, Guid.NewGuid().ToString() }
            });

            var result = InternalHelpers.ToKeyValuePair(collection);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        public void HeaderDictionaryToKeyValuePairIgnoresEmptyKeyNames(string keyName)
        {
            CustomHeaderCollection collection = new CustomHeaderCollection(new Dictionary<string, StringValues>
            {
                { keyName, Guid.NewGuid().ToString() }
            });

            var result = InternalHelpers.ToKeyValuePair(collection);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        public void FormCollectionToKeyValuePairIgnoresEmptyKeyNames(string keyName)
        {
            CustomFormCollection collection = new CustomFormCollection(new Dictionary<string, StringValues>
            {
                { keyName, Guid.NewGuid().ToString() }
            });

            var result = InternalHelpers.ToKeyValuePair(collection);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        public void QueryCollectionToKeyValuePairIgnoresEmptyKeyNames(string keyName)
        {
            CustomQueryCollection collection = new CustomQueryCollection(new Dictionary<string, StringValues>
            {
                { keyName, Guid.NewGuid().ToString() }
            });

            var result = InternalHelpers.ToKeyValuePair(collection);

            Assert.AreEqual(0, result.Count);
        }
    }
}
