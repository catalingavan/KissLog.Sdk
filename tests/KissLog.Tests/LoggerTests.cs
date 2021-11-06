using KissLog.Http;
using KissLog.LoggerData;
using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog.Tests
{
    [TestClass]
    public class LoggerTests
    {
        [TestMethod]
        public void IdIsNotNull()
        {
            Logger logger = new Logger();

            Assert.IsNotNull(logger.Id);
            Assert.IsTrue(logger.Id != Guid.Empty);
        }

        [TestMethod]
        public void NewIdIsGeneratedForEachInstance()
        {
            Logger logger1 = new Logger();
            Logger logger2 = new Logger();

            Assert.AreNotEqual(logger1.Id, logger2.Id);
        }

        [TestMethod]
        public void LoggerDataContainerIsNotNull()
        {
            Logger logger = new Logger();

            Assert.IsNotNull(logger.DataContainer);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void DefaultConstuctorGeneratesDefaultCategory(string categoryName)
        {
            Logger logger = new Logger(categoryName: categoryName);

            Assert.AreEqual(Constants.DefaultLoggerCategoryName, logger.CategoryName);
        }

        [TestMethod]
        [DataRow("Category 1", "Category 1")]
        [DataRow("Database", "Database")]
        [DataRow("", Constants.DefaultLoggerCategoryName)]
        public void CategoryNameConstuctor(string categoryName, string expectedValue)
        {
            Logger logger = new Logger(categoryName: categoryName);

            Assert.AreEqual(expectedValue, logger.CategoryName);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void NullUrlGeneratesNullHttpProperties(string url)
        {
            Logger logger = new Logger(url: url);

            Assert.IsNull(logger.DataContainer.HttpProperties);
        }

        [TestMethod]
        [DataRow("/")]
        [DataRow("my/path")]
        [DataRow("http://my-application/path")]
        [DataRow("!*&")]
        public void UrlGeneratesHttpProperties(string url)
        {
            Logger logger = new Logger(url: url);

            Assert.IsNotNull(logger.DataContainer.HttpProperties);
        }

        #region Notify Message

        [TestMethod]
        public void NotifyMessageIsInvoked()
        {
            CommonTestHelpers.ResetContext();

            List<LogMessage> messageArgs = new List<LogMessage>();

            KissLogConfiguration.Listeners.Add(new CustomLogListener(onMessage: (LogMessage arg) => { messageArgs.Add(arg); }));

            Logger logger = new Logger();
            logger.Log(LogLevel.Trace, "Message");

            Assert.AreEqual(1, messageArgs.Count);
        }

        [TestMethod]
        public void NotifyMessageIsInvokedForEachMessage()
        {
            CommonTestHelpers.ResetContext();

            List<LogMessage> messageArgs = new List<LogMessage>();

            KissLogConfiguration.Listeners.Add(new CustomLogListener(onMessage: (LogMessage arg) => { messageArgs.Add(arg); }));

            Logger logger = new Logger();
            logger.Log(LogLevel.Trace, "Message");
            logger.Log(LogLevel.Debug, Car.Dacia);
            logger.Log(LogLevel.Information, new Args("Message", 100, new Exception()));
            logger.Log(LogLevel.Warning, new Exception());

            Assert.AreEqual(4, messageArgs.Count);
        }

        [TestMethod]
        public void NotifyMessageIsInvokedForEachLogger()
        {
            CommonTestHelpers.ResetContext();

            List<LogMessage> messageArgs = new List<LogMessage>();

            KissLogConfiguration.Listeners.Add(new CustomLogListener(onMessage: (LogMessage arg) => { messageArgs.Add(arg); }));

            Logger logger1 = new Logger();
            logger1.Log(LogLevel.Trace, "Message");

            Logger logger2 = new Logger();
            logger2.Log(LogLevel.Trace, "Message");

            Logger logger3 = new Logger();
            logger3.Log(LogLevel.Trace, "Message");

            Assert.AreEqual(3, messageArgs.Count);
        }

        [TestMethod]
        public void NotifyMessageExceptionIsSilentlySwallowed()
        {
            CommonTestHelpers.ResetContext();

            KissLogConfiguration.Listeners.Add(new CustomLogListener(onMessage: (LogMessage arg) => { throw new Exception(); }));

            Logger logger = new Logger();
            logger.Log(LogLevel.Trace, "Message");
        }

        #endregion

        #region Notify OnBeginRequest

        [TestMethod]
        public void NotifyOnBeginRequestContainsTheLoggerUrl()
        {
            CommonTestHelpers.ResetContext();

            HttpRequest httpRequestArgs = null;

            KissLogConfiguration.Listeners.Add(new CustomLogListener(onBeginRequest: (HttpRequest arg) => { httpRequestArgs = arg; }));

            Logger logger = new Logger(url: "/App/Method1");

            Assert.AreEqual(httpRequestArgs.Url.LocalPath, "/App/Method1");
        }

        [TestMethod]
        public void NotifyOnBeginRequestIsInvokedForAllLoggersWithUrl()
        {
            CommonTestHelpers.ResetContext();

            List<HttpRequest> onBeginRequestArgs = new List<HttpRequest>();

            KissLogConfiguration.Listeners.Add(new CustomLogListener(onBeginRequest: (HttpRequest arg) => { onBeginRequestArgs.Add(arg); }));

            Logger logger1 = new Logger(url: "App/Method1");
            Logger logger2 = new Logger(url: "App/Method2");

            Assert.AreEqual(2, onBeginRequestArgs.Count);
        }

        [TestMethod]
        public void NotifyOnBeginRequestIsNotInvokedForLoggersWithoutUrl()
        {
            CommonTestHelpers.ResetContext();

            List<HttpRequest> onBeginRequestArgs = new List<HttpRequest>();

            KissLogConfiguration.Listeners.Add(new CustomLogListener(onBeginRequest: (HttpRequest arg) => { onBeginRequestArgs.Add(arg); }));

            Logger logger1 = new Logger();
            Logger logger2 = new Logger();

            Assert.AreEqual(0, onBeginRequestArgs.Count);
        }

        [TestMethod]
        public void NotifyOnBeginRequestExceptionIsSilentlySwallowed()
        {
            CommonTestHelpers.ResetContext();

            KissLogConfiguration.Listeners.Add(new CustomLogListener(onBeginRequest: (HttpRequest arg) => { throw new Exception(); }));

            Logger logger = new Logger(url: "App/Method1");
        }

        #endregion

        [TestMethod]
        public void NotifyListenersIgnoresNullLogger()
        {
            Logger logger = null;
            Logger.NotifyListeners(logger);
        }

        [TestMethod]
        public void NotifyListenersIgnoresNullLoggersArray()
        {
            Logger[] loggers = null;
            Logger.NotifyListeners(loggers);
        }

        [TestMethod]
        public void NotifyListenersIgnoresEmptyLoggersArray()
        {
            Logger[] loggers = new Logger[] { };
            Logger.NotifyListeners(loggers);
        }

        [TestMethod]
        public void NotifyListenersExceptionIsSilentlySwallowed()
        {
            CommonTestHelpers.ResetContext();

            KissLogConfiguration.Listeners.Add(new CustomLogListener(onFlush: (FlushLogArgs arg) => { throw new Exception(); }));

            Logger logger = new Logger();

            Logger.NotifyListeners(logger);
        }

        [TestMethod]
        public void ResetDisposesTheOldDataContainerAndCreatesANewOne()
        {
            Logger logger = new Logger();
            logger.Trace("Message");
            logger.Error(new Exception());
            logger.LogAsFile("My file");

            LoggerDataContainer oldDataContainer = logger.DataContainer;

            logger.Reset();

            LoggerDataContainer newDataContainer = logger.DataContainer;

            Assert.IsTrue(oldDataContainer._disposed);
            Assert.AreNotSame(oldDataContainer, newDataContainer);
        }

        [TestMethod]
        public void ResetClearsTheLoggerDataContainer()
        {
            Logger logger = new Logger();
            logger.Trace("Message");
            logger.Error(new Exception());
            logger.LogAsFile("My file");

            logger.Reset();

            LoggerDataContainer dataContainer = logger.DataContainer;

            Assert.AreEqual(0, dataContainer.LogMessages.Count());
            Assert.AreEqual(0, dataContainer.Exceptions.Count());
            Assert.AreEqual(0, dataContainer.FilesContainer.GetLoggedFiles().Count);
        }

        [TestMethod]
        public void ResetCreatesANewLoggerDataContainer()
        {
            Logger logger = new Logger();
            logger.Trace("Message");
            logger.Error(new Exception());
            logger.LogAsFile("My file");

            LoggerDataContainer dataContainer1 = logger.DataContainer;

            logger.Reset();

            LoggerDataContainer dataContainer2 = logger.DataContainer;

            Assert.AreNotSame(dataContainer1, dataContainer2);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("/App/Method1")]
        public void ResetKeepsTheHttpPropertiesReference(string url)
        {
            Logger logger = new Logger(url: url);

            HttpProperties httpProperties = logger.DataContainer.HttpProperties;

            logger.Reset();

            Assert.AreSame(httpProperties, logger.DataContainer.HttpProperties);
        }

        [TestMethod]
        public void FactoryIsNotNull()
        {
            CommonTestHelpers.ResetContext();

            ILoggerFactory factory = Logger.Factory;

            Assert.IsNotNull(factory);
        }

        [TestMethod]
        public void FactoryIsTypeOfLoggerFactory()
        {
            ILoggerFactory factory = Logger.Factory;

            Assert.IsInstanceOfType(factory, typeof(LoggerFactory));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetFactoryThrowExceptionForNullArgument()
        {
            Logger.SetFactory(null);
        }

        [TestMethod]
        public void SetFactoryUpdatesTheFactory()
        {
            ILoggerFactory factory = new Mock<ILoggerFactory>().Object;

            Logger.SetFactory(factory);

            Assert.AreSame(factory, Logger.Factory);
        }
    }
}
