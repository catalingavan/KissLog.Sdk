using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KissLog.Tests
{
    [TestClass]
    public class KissLogConfigurationTests
    {
        [TestMethod]
        public void JsonFormatterIsNotNull()
        {
            var value = KissLogConfiguration.JsonSerializer;

            Assert.IsNotNull(value);
        }

        [TestMethod]
        public void KissLogPackagesIsNotNull()
        {
            var value = KissLogConfiguration.KissLogPackages;

            Assert.IsNotNull(value);
        }

        [TestMethod]
        public void LogListenersContainerIsNotNull()
        {
            var value = KissLogConfiguration.Listeners;

            Assert.IsNotNull(value);
        }

        [TestMethod]
        public void OptionsIsNotNull()
        {
            var value = KissLogConfiguration.Listeners;

            Assert.IsNotNull(value);
        }
    }
}
