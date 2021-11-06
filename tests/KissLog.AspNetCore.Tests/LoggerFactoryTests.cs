using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog.AspNetCore.Tests
{
    [TestClass]
    public class LoggerFactoryTests
    {
        [TestMethod]
        public void ConstructorUsesHttpContextAccessorWhenNoInterfaceIsProvided()
        {
            var factory1 = new LoggerFactory();
            var factory2 = new LoggerFactory(null);

            Assert.IsInstanceOfType(factory1._httpContextAccessor, typeof(HttpContextAccessor));
            Assert.IsInstanceOfType(factory2._httpContextAccessor, typeof(HttpContextAccessor));
        }

        [TestMethod]
        public void GetDoesNotThrowExceptionWhenHttpContextIsNull()
        {
            var factory = new LoggerFactory(new CustomHttpContextAccessor());

            Logger logger = factory.Get();

            Assert.IsNotNull(logger);
        }

        [TestMethod]
        public void GetAllDoesNotThrowExceptionWhenHttpContextIsNull()
        {
            var factory = new LoggerFactory(new CustomHttpContextAccessor());

            IEnumerable<Logger> loggers = factory.GetAll();

            Assert.IsNotNull(loggers);
        }

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

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(p => p.Items).Returns(new Dictionary<object, object>());

            Logger logger = factory.GetInstance(httpContext.Object);

            IDictionary<string, Logger> dictionary = httpContext.Object.Items[LoggerFactory.DictionaryKey] as IDictionary<string, Logger>;

            Assert.IsNotNull(dictionary);
        }

        [TestMethod]
        public void GetInstanceAddsTheLoggerToDictionary()
        {
            var factory = new LoggerFactory();

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(p => p.Items).Returns(new Dictionary<object, object>());

            Logger logger = factory.GetInstance(httpContext.Object);

            var dictionary = httpContext.Object.Items[LoggerFactory.DictionaryKey] as IDictionary<string, Logger>;

            Assert.AreSame(logger, dictionary[logger.CategoryName]);
        }

        [TestMethod]
        public void GetInstanceReturnsTheSameLoggerInstance()
        {
            var factory = new LoggerFactory();

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(p => p.Items).Returns(new Dictionary<object, object>());

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

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(p => p.Items).Returns(new Dictionary<object, object>());

            Dictionary<string, Logger> loggers = new Dictionary<string, Logger>();
            foreach (string categoryName in new[] { Constants.DefaultLoggerCategoryName, "Category 1", "Category 2", "Category 3" })
            {
                Logger logger = factory.GetInstance(httpContext.Object, categoryName: categoryName);
                loggers.Add(categoryName, logger);
            }

            foreach (var item in loggers)
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

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(p => p.Items).Returns(new Dictionary<object, object>());

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

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(p => p.Items).Returns(new Dictionary<object, object>());

            IEnumerable<Logger> loggers = factory.GetAll(httpContext.Object);

            Assert.AreEqual(0, loggers.Count());
        }

        [TestMethod]
        public void GetAllReturnsAllTheLoggersCreated()
        {
            var factory = new LoggerFactory();

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(p => p.Items).Returns(new Dictionary<object, object>());

            foreach (string categoryName in new[] { Constants.DefaultLoggerCategoryName, "Category 1", "Category 2", "Category 3" })
            {
                Logger logger = factory.GetInstance(httpContext.Object, categoryName: categoryName);
            }

            IEnumerable<Logger> loggers = factory.GetAll(httpContext.Object);

            Assert.AreEqual(4, loggers.Count());
        }
    }
}
