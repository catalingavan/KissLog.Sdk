using KissLog.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KissLog.Tests.Http
{
    [TestClass]
    public class HttpPropertiesFactoryTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void NullUrlThrowsException(string url)
        {
            HttpProperties result = HttpPropertiesFactory.Create(url);
        }

        [TestMethod]
        public void RequestHttpMethodIsGet()
        {
            HttpProperties result = HttpPropertiesFactory.Create("/");

            Assert.AreEqual("GET", result.Request.HttpMethod);
        }
    }
}
