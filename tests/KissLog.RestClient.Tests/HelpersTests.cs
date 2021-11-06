using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KissLog.RestClient.Tests
{
    [TestClass]
    public class HelpersTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void BuildUrlThrowsExceptionForNullBaseUrl(string baseUrl)
        {
            Uri url = Helpers.BuildUri(baseUrl, "path");
        }

        [TestMethod]
        [DataRow("http://app", null)]
        [DataRow("http://app", "")]
        [DataRow("http://app", " ")]
        [DataRow("http://app", "path1")]
        [DataRow("http://app", "/path1/path2")]
        public void BuildUrlReturnsAbsoluteUri(string baseUrl, string resource)
        {
            Uri url = Helpers.BuildUri(baseUrl, resource);

            Assert.AreEqual(true, url.IsAbsoluteUri);
        }
    }
}
