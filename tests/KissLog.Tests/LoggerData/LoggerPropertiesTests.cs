using KissLog.LoggerData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KissLog.Tests.LoggerData
{
    [TestClass]
    public class LoggerPropertiesTests
    {
        [TestMethod]
        public void ExplicitStatusCodeIsNull()
        {
            LoggerProperties item = new LoggerProperties();

            Assert.IsNull(item.ExplicitStatusCode);
        }

        [TestMethod]
        public void ExplicitLogResponseBodyIsNull()
        {
            LoggerProperties item = new LoggerProperties();

            Assert.IsNull(item.ExplicitLogResponseBody);
        }

        [TestMethod]
        public void IsManagedByHttpRequestIsFalse()
        {
            LoggerProperties item = new LoggerProperties();

            Assert.IsFalse(item.IsManagedByHttpRequest);
        }

        [TestMethod]
        public void CustomPropertiesIsNotNull()
        {
            LoggerProperties item = new LoggerProperties();

            Assert.IsNotNull(item.CustomProperties);
        }
    }
}
