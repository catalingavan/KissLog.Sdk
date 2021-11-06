using KissLog.LoggerFactories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace KissLog.Tests.LoggerFactories
{
    [TestClass]
    public class InstanceLoggerFactoryTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullLoggerThrowsException()
        {
            var factory = new InstanceLoggerFactory(null);
        }

        [TestMethod]
        [DataRow(null, null)]
        [DataRow("Category", "my/url")]
        public void GetReturnsLoggerInstance(string categoryName, string url)
        {
            Logger logger = new Logger();
            var factory = new InstanceLoggerFactory(logger);

            Assert.AreSame(logger, factory.Get(categoryName, url));
        }

        [TestMethod]
        public void GetAllReturnsLoggerInstance()
        {
            Logger logger = new Logger();
            var factory = new LoggerFactory(logger);

            var loggers = factory.GetAll();

            Assert.AreEqual(1, loggers.Count());
            Assert.AreSame(logger, loggers.First());
        }
    }
}
