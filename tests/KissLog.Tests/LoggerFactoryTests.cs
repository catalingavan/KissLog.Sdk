using KissLog.LoggerFactories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KissLog.Tests
{
    [TestClass]
    public class LoggerFactoryTests
    {
        [TestMethod]
        public void DefaultConstructorGeneratesDefaultLoggerFactory()
        {
            var factory = new LoggerFactory();

            Assert.IsInstanceOfType(factory._factory, typeof(DefaultLoggerFactory));
        }

        [TestMethod]
        public void LoggerInstanceGeneratesInstanceLoggerFactory()
        {
            var factory = new LoggerFactory(new Logger());

            Assert.IsInstanceOfType(factory._factory, typeof(InstanceLoggerFactory));
        }

        [TestMethod]
        public void LoggerFnGeneratesDelegateLoggerFactory()
        {
            var factory = new LoggerFactory((category, url) => new Logger());

            Assert.IsInstanceOfType(factory._factory, typeof(DelegateLoggerFactory));
        }
    }
}
