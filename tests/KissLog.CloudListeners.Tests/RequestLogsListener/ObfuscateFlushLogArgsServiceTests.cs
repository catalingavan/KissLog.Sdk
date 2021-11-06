using KissLog.CloudListeners.RequestLogsListener;
using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog.CloudListeners.Tests.RequestLogsListener
{
    [TestClass]
    public class ObfuscateFlushLogArgsServiceTests
    {
        [TestMethod]
        public void ObfuscateDoesNotThrowExceptionForNullObfuscationService()
        {
            FlushLogArgs flushLogArgs = CommonTestHelpers.Factory.CreateFlushLogArgs();

            var service = new ObfuscateFlushLogArgsService(null);
            service.Obfuscate(flushLogArgs);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ObfuscateThrowsExceptionForNullFlushLogArgs()
        {
            IObfuscationService obfuscationService = new Mock<IObfuscationService>().Object;

            var service = new ObfuscateFlushLogArgsService(obfuscationService);

            service.Obfuscate(null);
        }

        [TestMethod]
        public void GetPropertyNameReturnsCorrectValue()
        {
            var service = new ObfuscateFlushLogArgsService(null);

            Assert.AreEqual("HttpProperties.Request.Properties.Cookies", service.GetPropertyName(p => p.HttpProperties.Request.Properties.Cookies));
            Assert.AreEqual("HttpProperties.Response.Properties.Headers", service.GetPropertyName(p => p.HttpProperties.Response.Properties.Headers));
            Assert.AreEqual("HttpProperties.Response.Properties", service.GetPropertyName(p => p.HttpProperties.Response.Properties));
        }

        [TestMethod]
        [DataRow("HttpProperties.Request.Properties.Headers")]
        [DataRow("HttpProperties.Request.Properties.Cookies")]
        [DataRow("HttpProperties.Request.Properties.FormData")]
        [DataRow("HttpProperties.Request.Properties.ServerVariables")]
        [DataRow("HttpProperties.Request.Properties.Claims")]
        [DataRow("HttpProperties.Response.Properties.Headers")]
        public void ShouldObfuscatePropertyNameIsSetCorrectly(string property)
        {
            int count = 2;
            
            FlushLogArgs flushLogArgs = CommonTestHelpers.Factory.CreateFlushLogArgs();
            flushLogArgs.HttpProperties.Request.SetProperties(new Http.RequestProperties(new Http.RequestProperties.CreateOptions
            {
                Headers = property == "HttpProperties.Request.Properties.Headers" ? CommonTestHelpers.GenerateList(count) : null,
                Cookies = property == "HttpProperties.Request.Properties.Cookies" ? CommonTestHelpers.GenerateList(count) : null,
                FormData = property == "HttpProperties.Request.Properties.FormData" ? CommonTestHelpers.GenerateList(count) : null,
                ServerVariables = property == "HttpProperties.Request.Properties.ServerVariables" ? CommonTestHelpers.GenerateList(count) : null,
                Claims = property == "HttpProperties.Request.Properties.Claims" ? CommonTestHelpers.GenerateList(count) : null
            }));
            flushLogArgs.HttpProperties.Response.SetProperties(new Http.ResponseProperties(new Http.ResponseProperties.CreateOptions
            {
                Headers = property == "HttpProperties.Response.Properties.Headers" ? CommonTestHelpers.GenerateList(count) : null
            }));

            List<string> propertyNames = new List<string>();

            var obfuscationService = new Mock<IObfuscationService>();
            obfuscationService
                .Setup(p => p.ShouldObfuscate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string key, string value, string propertyName) =>
                {
                    propertyNames.Add(propertyName);
                })
                .Returns(true);

            var service = new ObfuscateFlushLogArgsService(obfuscationService.Object);
            service.Obfuscate(flushLogArgs);

            Assert.AreEqual(count, propertyNames.Count);
            Assert.IsTrue(propertyNames.All(p => p == property));
        }

        [TestMethod]
        [DataRow("HttpProperties.Request.Properties.Headers")]
        [DataRow("HttpProperties.Request.Properties.Cookies")]
        [DataRow("HttpProperties.Request.Properties.FormData")]
        [DataRow("HttpProperties.Request.Properties.ServerVariables")]
        [DataRow("HttpProperties.Request.Properties.Claims")]
        [DataRow("HttpProperties.Response.Properties.Headers")]
        public void ObfuscatesTheProperties(string property)
        {
            int count = 2;
            
            FlushLogArgs flushLogArgs = CommonTestHelpers.Factory.CreateFlushLogArgs();
            flushLogArgs.HttpProperties.Request.SetProperties(new Http.RequestProperties(new Http.RequestProperties.CreateOptions
            {
                Headers = property == "HttpProperties.Request.Properties.Headers" ? CommonTestHelpers.GenerateList(count) : null,
                Cookies = property == "HttpProperties.Request.Properties.Cookies" ? CommonTestHelpers.GenerateList(count) : null,
                FormData = property == "HttpProperties.Request.Properties.FormData" ? CommonTestHelpers.GenerateList(count) : null,
                ServerVariables = property == "HttpProperties.Request.Properties.ServerVariables" ? CommonTestHelpers.GenerateList(count) : null,
                Claims = property == "HttpProperties.Request.Properties.Claims" ? CommonTestHelpers.GenerateList(count) : null
            }));
            flushLogArgs.HttpProperties.Response.SetProperties(new Http.ResponseProperties(new Http.ResponseProperties.CreateOptions
            {
                Headers = property == "HttpProperties.Response.Properties.Headers" ? CommonTestHelpers.GenerateList(count) : null
            }));

            var obfuscationService = new Mock<IObfuscationService>();
            obfuscationService
                .Setup(p => p.ShouldObfuscate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            var service = new ObfuscateFlushLogArgsService(obfuscationService.Object);
            service.Obfuscate(flushLogArgs);

            IEnumerable<KeyValuePair<string, string>> properties =
                property == "HttpProperties.Request.Properties.Headers" ? flushLogArgs.HttpProperties.Request.Properties.Headers :
                property == "HttpProperties.Request.Properties.Cookies" ? flushLogArgs.HttpProperties.Request.Properties.Cookies :
                property == "HttpProperties.Request.Properties.FormData" ? flushLogArgs.HttpProperties.Request.Properties.FormData :
                property == "HttpProperties.Request.Properties.ServerVariables" ? flushLogArgs.HttpProperties.Request.Properties.ServerVariables :
                property == "HttpProperties.Request.Properties.Claims" ? flushLogArgs.HttpProperties.Request.Properties.Claims :
                property == "HttpProperties.Response.Properties.Headers" ? flushLogArgs.HttpProperties.Response.Properties.Headers :
                new List<KeyValuePair<string, string>>();

            Assert.AreEqual(count, properties.Count());
            Assert.IsTrue(properties.All(p => p.Value == ObfuscateFlushLogArgsService.Placeholder));
        }
    }
}
