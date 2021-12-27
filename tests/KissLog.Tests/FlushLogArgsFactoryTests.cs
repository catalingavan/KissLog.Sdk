using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog.Tests
{
    [TestClass]
    public class FlushLogArgsFactoryTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionForNullLoggers()
        {
            FlushLogArgsFactory.Create(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionForEmptyList()
        {
            FlushLogArgsFactory.Create(new Logger[] { });
        }

        [TestMethod]
        public void HasValueForEmptyLogger()
        {
            FlushLogArgs item = FlushLogArgsFactory.Create(new[] { new Logger() });

            Assert.IsNotNull(item);
        }

        [TestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void IsCreatedByHttpRequestMatchesIsManagedByHttpRequest(bool isManagedByHttpRequest)
        {
            Logger logger = new Logger();
            logger.DataContainer.LoggerProperties.IsManagedByHttpRequest = isManagedByHttpRequest;

            FlushLogArgs item = FlushLogArgsFactory.Create(new[] { logger });

            Assert.AreEqual(isManagedByHttpRequest, item.IsCreatedByHttpRequest);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("/app/method")]
        public void HttpPropertiesAreNotNull(string url)
        {
            FlushLogArgs item = FlushLogArgsFactory.Create(new[] { new Logger(url: url) });

            Assert.IsNotNull(item.HttpProperties);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("/app/method")]
        public void HttpPropertiesRequestIsNotNull(string url)
        {
            FlushLogArgs item = FlushLogArgsFactory.Create(new[] { new Logger(url: url) });

            Assert.IsNotNull(item.HttpProperties.Request);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("/app/method")]
        public void MachineNameIsCopied(string url)
        {
            FlushLogArgs item = FlushLogArgsFactory.Create(new[] { new Logger(url: url) });

            string machineName = InternalHelpers.GetMachineName();

            Assert.AreEqual(machineName, item.HttpProperties.Request.MachineName);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("/app/method")]
        public void HttpPropertiesResponseIsNotNull(string url)
        {
            FlushLogArgs item = FlushLogArgsFactory.Create(new[] { new Logger(url: url) });

            Assert.IsNotNull(item.HttpProperties.Response);
        }

        [TestMethod]
        public void HasMessagesGroupsFromAllTheLoggers()
        {
            List<Logger> loggers = new List<Logger>();

            for(int i = 0; i < 10; i++)
            {
                Logger logger = new Logger();
                logger.Trace($"Message from Logger {i}");

                loggers.Add(logger);
            }

            FlushLogArgs item = FlushLogArgsFactory.Create(loggers.ToArray());
            
            int messagesGroups = item.MessagesGroups.Count();
            int messagesCount = item.MessagesGroups.Sum(p => p.Messages.Count());

            Assert.AreEqual(10, messagesGroups);
            Assert.AreEqual(10, messagesCount);
        }

        [TestMethod]
        public void MessagesGroupsDoesNotContainEmptyLogsMessages()
        {
            Logger logger1 = new Logger();
            logger1.Trace("Message");

            Logger logger2 = new Logger();

            FlushLogArgs item = FlushLogArgsFactory.Create(new[] { logger1, logger2 });

            int messagesGroups = item.MessagesGroups.Count();
            int messagesCount = item.MessagesGroups.Sum(p => p.Messages.Count());

            Assert.AreEqual(1, messagesGroups);
            Assert.AreEqual(1, messagesCount);
        }

        [TestMethod]
        public void MessagesAreNotReferenced()
        {
            Logger logger = new Logger();
            logger.Trace("Message 1");

            FlushLogArgs item = FlushLogArgsFactory.Create(new[] { logger });

            logger.Trace("Message 2");
            int count = item.MessagesGroups.Sum(p => p.Messages.Count());

            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void HasExceptionsFromAllTheLoggers()
        {
            List<Logger> loggers = new List<Logger>();

            for (int i = 0; i < 10; i++)
            {
                Logger logger = new Logger();
                logger.Trace(new Exception($"Exception from Logger {i}"));

                loggers.Add(logger);
            }

            FlushLogArgs item = FlushLogArgsFactory.Create(loggers.ToArray());
            int count = item.Exceptions.Count();

            Assert.AreEqual(10, count);
        }

        [TestMethod]
        public void ExceptionsAreNotReferenced()
        {
            Logger logger = new Logger();
            logger.Trace(new Exception());

            FlushLogArgs item = FlushLogArgsFactory.Create(new[] { logger });

            logger.Trace(new Exception());
            int count = item.Exceptions.Count();

            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void HasFilesFromAllTheLoggers()
        {
            List<Logger> loggers = new List<Logger>();

            for (int i = 0; i < 10; i++)
            {
                Logger logger = new Logger();
                logger.LogAsFile($"File {i}");

                loggers.Add(logger);
            }

            FlushLogArgs item = FlushLogArgsFactory.Create(loggers.ToArray());
            int count = item.Files.Count();

            foreach(Logger logger in loggers)
            {
                logger.Reset();
            }

            Assert.AreEqual(10, count);
        }

        [TestMethod]
        public void FilesAreNotReferenced()
        {
            Logger logger = new Logger();
            logger.LogAsFile("File 1");

            FlushLogArgs item = FlushLogArgsFactory.Create(new[] { logger });

            logger.LogAsFile("File 2");
            int count = item.Files.Count();

            logger.Reset();

            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void DefaultLoggerCategoryHasPriorityWhenSelectingHttpProperties()
        {
            Logger logger1 = new Logger(categoryName: "SomeLogger", url: "/SomeLogger/Method1");
            Logger logger2 = new Logger(url: "/DefaultLogger/Method1");

            FlushLogArgs item = FlushLogArgsFactory.Create(new[] { logger1, logger2 });

            Assert.AreEqual("/DefaultLogger/Method1", item.HttpProperties.Request.Url.LocalPath);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetExplicitStatusCodeThrowsExceptionForNullList()
        {
            int? value = FlushLogArgsFactory.GetExplicitStatusCode(null);
        }

        [TestMethod]
        public void GetExplicitStatusCodeReturnsNullForEmptyList()
        {
            int? value = FlushLogArgsFactory.GetExplicitStatusCode(new List<Logger>());

            Assert.IsNull(value);
        }

        [TestMethod]
        public void GetExplicitStatusCodeReturnsNullIfNoValueIsFound()
        {
            Logger logger1 = new Logger();
            Logger logger2 = new Logger();

            int? value = FlushLogArgsFactory.GetExplicitStatusCode(new List<Logger> { logger1, logger2 });

            Assert.IsNull(value);
        }

        [TestMethod]
        public void GetExplicitStatusCodeUsesDefaultLoggerAsPrimarySource()
        {
            Logger logger1 = new Logger(categoryName: "Category 1");
            Logger logger2 = new Logger();

            logger1.SetStatusCode(200);
            logger2.SetStatusCode(400);

            int? value = FlushLogArgsFactory.GetExplicitStatusCode(new List<Logger> { logger1, logger2 });

            Assert.AreEqual(400, value.Value);
        }

        [TestMethod]
        public void GetExplicitStatusCodeReturnsTheFirstValueFound()
        {
            Logger logger1 = new Logger();
            Logger logger2 = new Logger();
            Logger logger3 = new Logger();

            logger2.SetStatusCode(100);
            logger3.SetStatusCode(200);

            int? value = FlushLogArgsFactory.GetExplicitStatusCode(new List<Logger> { logger1, logger2, logger3 });

            Assert.AreEqual(100, value.Value);
        }
    }
}
