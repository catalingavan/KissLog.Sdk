using KissLog.CloudListeners.RequestLogsListener;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace KissLog.CloudListeners.Tests.RequestLogsListener
{
    [TestClass]
    public class ObfuscationServiceTests
    {
        [TestMethod]
        public void DefaultPatternsAreUsedIfNoPatternsAreProvided()
        {
            var service = new ObfuscationService();

            Assert.AreEqual(JsonSerializer.Serialize(ObfuscationService.DefaultPatterns), JsonSerializer.Serialize(service.GetPatterns()));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionForNullArgument()
        {
            var service = new ObfuscationService(null);
        }

        [TestMethod]
        public void ConstructorUpdatesThePatterns()
        {
            List<string> patterns = new List<string> { "pattern1", "pattern2", "pattern3" };

            var service = new ObfuscationService(patterns);

            Assert.AreEqual(JsonSerializer.Serialize(patterns), JsonSerializer.Serialize(service.GetPatterns()));
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void NullPatternValueDoesNotThrowException(string pattern)
        {
            List<string> patterns = new List<string> { pattern };

            var service = new ObfuscationService(patterns);

            service.ShouldObfuscate("key", "value", "propertyName");
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void ShouldObfuscateReturnsFalseForNullKey(string key)
        {
            var service = new ObfuscationService();

            bool result = service.ShouldObfuscate(key, "value", "propertyName");

            Assert.IsFalse(result);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void ShouldObfuscateDoesNotThrowExceptionForNullValue(string value)
        {
            var service = new ObfuscationService();

            bool result = service.ShouldObfuscate("key", value, "propertyName");

            Assert.IsFalse(result);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void ShouldObfuscateDoesNotThrowExceptionForNullPropertyName(string propertyName)
        {
            var service = new ObfuscationService();

            bool result = service.ShouldObfuscate("key", "value", propertyName);

            Assert.IsFalse(result);
        }

        [TestMethod]
        [DataRow("(?si)pass", "password", true)]
        [DataRow("(?si)pass", "MYPASSWORD", true)]
        [DataRow("(?si)^pin$", "pin", true)]
        [DataRow("(?si)^pin$", "PIN", true)]
        [DataRow("(?si)^pin$", "mypin", false)]
        public void RegexPatternIsEvaluated(string pattern, string key, bool expectedResult)
        {
            var service = new ObfuscationService(new List<string> { pattern });

            bool result = service.ShouldObfuscate(key, null, "propertyName");

            Assert.AreEqual(expectedResult, result);
        }
    }
}
