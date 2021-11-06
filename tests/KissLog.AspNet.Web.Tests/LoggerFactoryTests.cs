using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KissLog.AspNet.Web.Tests
{
    [TestClass]
    public class LoggerFactoryTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetInstanceThrowsExceptionWhenHttpContextIsNull()
        {
            var factory = new LoggerFactory();

            Logger logger = factory.GetInstance(null);
        }

        [TestMethod]
        public void GetInstanceCreatesLoggersDictionary()
        {
            var factory = new LoggerFactory();

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(p => p.Items).Returns(new Dictionary<string, object>());

            Logger logger = factory.GetInstance(httpContext.Object);

            IDictionary<string, Logger> dictionary = httpContext.Object.Items[LoggerFactory.DictionaryKey] as IDictionary<string, Logger>;

            Assert.IsNotNull(dictionary);
        }

        [TestMethod]
        public void GetInstanceAddsTheLoggerToDictionary()
        {
            var factory = new LoggerFactory();

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(p => p.Items).Returns(new Dictionary<string, object>());

            Logger logger = factory.GetInstance(httpContext.Object);

            var dictionary = httpContext.Object.Items[LoggerFactory.DictionaryKey] as IDictionary<string, Logger>;

            Assert.AreSame(logger, dictionary[logger.CategoryName]);
        }

        [TestMethod]
        public void GetInstanceReturnsTheSameLoggerInstance()
        {
            var factory = new LoggerFactory();

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(p => p.Items).Returns(new Dictionary<string, object>());

            List<Logger> loggers = new List<Logger>();

            Logger logger = factory.GetInstance(httpContext.Object);
            loggers.Add(logger);

            logger = factory.GetInstance(httpContext.Object);
            loggers.Add(logger);

            Assert.AreSame(loggers[0], loggers[1]);
        }

        [TestMethod]
        public void GetInstanceReturnsTheSameLoggerInstanceForTheProvidedCategoryName()
        {
            var factory = new LoggerFactory();

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(p => p.Items).Returns(new Dictionary<string, object>());

            Dictionary<string, Logger> loggers = new Dictionary<string, Logger>();
            foreach(string categoryName in new[] { Constants.DefaultLoggerCategoryName, "Category 1", "Category 2", "Category 3" })
            {
                Logger logger = factory.GetInstance(httpContext.Object, categoryName: categoryName);
                loggers.Add(categoryName, logger);
            }

            foreach(var item in loggers)
            {
                string categoryName = item.Key;
                Logger logger = factory.GetInstance(httpContext.Object, categoryName: categoryName);

                Assert.AreSame(item.Value, logger);
            }
        }

        [TestMethod]
        public void LoggerHasIsManagedByHttpRequestSetToTrue()
        {
            var factory = new LoggerFactory();

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(p => p.Items).Returns(new Dictionary<string, object>());

            foreach (string categoryName in new[] { Constants.DefaultLoggerCategoryName, "Category 1", "Category 2", "Category 3" })
            {
                Logger logger = factory.GetInstance(httpContext.Object, categoryName: categoryName);
                Assert.IsTrue(logger.DataContainer.LoggerProperties.IsManagedByHttpRequest);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetAllThrowsExceptionWhenHttpContextIsNull()
        {
            var factory = new LoggerFactory();

            IEnumerable<Logger> loggers = factory.GetAll(null);
        }

        [TestMethod]
        public void GetAllReturnsEmptyListIfNoLoggersHaveBeenCreated()
        {
            var factory = new LoggerFactory();

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(p => p.Items).Returns(new Dictionary<string, object>());

            IEnumerable<Logger> loggers = factory.GetAll(httpContext.Object);

            Assert.AreEqual(0, loggers.Count());
        }

        [TestMethod]
        public void GetAllReturnsAllTheLoggersCreated()
        {
            var factory = new LoggerFactory();

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(p => p.Items).Returns(new Dictionary<string, object>());

            foreach (string categoryName in new[] { Constants.DefaultLoggerCategoryName, "Category 1", "Category 2", "Category 3" })
            {
                Logger logger = factory.GetInstance(httpContext.Object, categoryName: categoryName);
            }

            IEnumerable<Logger> loggers = factory.GetAll(httpContext.Object);

            Assert.AreEqual(4, loggers.Count());
        }
    }
}
