using KissLog.RestClient.HttpClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KissLog.RestClient.Tests.HttpClient
{
    [TestClass]
    public class LogHttpClientDecoratorTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionForNullDecorator()
        {
            var client = new LogHttpClientDecorator(null);
        }
    }
}
