using KissLog.RestClient.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KissLog.RestClient.Tests.Api
{
    [TestClass]
    public class PublicRestApiTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void ThrowsExceptionForNullBaseUrl(string baseUrl)
        {
            var service = new PublicRestApi(baseUrl);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        [DataRow("my")]
        [DataRow("my/path")]
        public void ThrowsExceptionForNotAbsoluteBaseUrl(string baseUrl)
        {
            var service = new PublicRestApi(baseUrl);
        }
    }
}
