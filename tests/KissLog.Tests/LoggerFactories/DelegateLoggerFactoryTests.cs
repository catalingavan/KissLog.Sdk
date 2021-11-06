using KissLog.LoggerFactories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace KissLog.Tests.LoggerFactories
{
    [TestClass]
    public class DelegateLoggerFactoryTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullHandlerThrowsException()
        {
            Func<string, string, Logger> fn = null;
            var factory = new DelegateLoggerFactory(fn);
        }

        [TestMethod]
        [DataRow(null, null)]
        [DataRow("Category", "my/url")]
        public void GetReturnsTheFnResult(string categoryName, string url)
        {
            Logger logger = new Logger();
            var factory = new DelegateLoggerFactory((string categoryName, string url) => logger);

            Assert.AreSame(logger, factory.Get(categoryName, url));
        }

        [TestMethod]
        public void GetAllReturnsTheSameLogger()
        {
            Logger logger = new Logger();
            var factory = new DelegateLoggerFactory((string categoryName, string url) => logger);

            foreach(string categoryName in new[] { "Category1", "Category2", "Category3" })
            {
                factory.Get(categoryName: categoryName);
            }

            Assert.AreEqual(1, factory.GetAll().Count());
            Assert.AreSame(logger, factory.GetAll().ElementAt(0));
        }

        [TestMethod]
        public void GetAllReturnsAllTheGeneratedLoggers()
        {
            var factory = new DelegateLoggerFactory((string categoryName, string url) => new Logger());

            foreach (string categoryName in new[] { "Category1", "Category2", "Category3" })
            {
                factory.Get(categoryName: categoryName);
            }

            Assert.AreEqual(3, factory.GetAll().Count());
        }
    }
}
