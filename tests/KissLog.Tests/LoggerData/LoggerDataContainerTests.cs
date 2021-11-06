using KissLog.LoggerData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KissLog.Tests.LoggerData
{
    [TestClass]
    public class LoggerDataContainerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullLoggerThrowsException()
        {
            LoggerDataContainer loggerDataContainer = new LoggerDataContainer(null);
        }

        [TestMethod]
        public void LogMessagesIsNotNull()
        {
            LoggerDataContainer loggerDataContainer = new LoggerDataContainer(new Logger());

            Assert.IsNotNull(loggerDataContainer.LogMessages);
        }

        [TestMethod]
        public void ExceptionsIsNotNull()
        {
            LoggerDataContainer loggerDataContainer = new LoggerDataContainer(new Logger());

            Assert.IsNotNull(loggerDataContainer.Exceptions);
        }

        [TestMethod]
        public void FilesContainerIsNotNull()
        {
            LoggerDataContainer loggerDataContainer = new LoggerDataContainer(new Logger());

            Assert.IsNotNull(loggerDataContainer.FilesContainer);
        }

        [TestMethod]
        public void LoggerPropertiesIsNotNull()
        {
            LoggerDataContainer loggerDataContainer = new LoggerDataContainer(new Logger());

            Assert.IsNotNull(loggerDataContainer.LoggerProperties);
        }

        [TestMethod]
        public void StartDateTimeIsInUtcFormat()
        {
            LoggerDataContainer loggerDataContainer = new LoggerDataContainer(new Logger());

            Assert.AreEqual(DateTimeKind.Utc, loggerDataContainer.DateTimeCreated.Kind);
        }

        [TestMethod]
        public void StartDateTimeIsInThePast()
        {
            LoggerDataContainer loggerDataContainer = new LoggerDataContainer(new Logger());

            Assert.IsTrue(loggerDataContainer.DateTimeCreated < DateTime.UtcNow);
        }

        [TestMethod]
        public void StartDateTimeHasValue()
        {
            LoggerDataContainer loggerDataContainer = new LoggerDataContainer(new Logger());

            Assert.IsTrue(loggerDataContainer.DateTimeCreated.Year > default(DateTime).Year);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetHttpPropertiesThrowsExceptionForNullArgument()
        {
            LoggerDataContainer loggerDataContainer = new LoggerDataContainer(new Logger());

            loggerDataContainer.SetHttpProperties(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddMessageThrowsExceptionForNullArgument()
        {
            LoggerDataContainer loggerDataContainer = new LoggerDataContainer(new Logger());
            
            loggerDataContainer.Add((LogMessage)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddExceptionThrowsExceptionForNullArgument()
        {
            LoggerDataContainer loggerDataContainer = new LoggerDataContainer(new Logger());

            loggerDataContainer.Add((Exception)null);
        }

        [TestMethod]
        public void DisposeUpdatesTheDisposedField()
        {
            LoggerDataContainer loggerDataContainer = new LoggerDataContainer(new Logger());

            bool disposedBefore = loggerDataContainer._disposed;

            loggerDataContainer.Dispose();

            bool disposedAfter = loggerDataContainer._disposed;

            Assert.IsFalse(disposedBefore);
            Assert.IsTrue(disposedAfter);
        }

        [TestMethod]
        public void DisposeAlsoDisposesTheFilesContainer()
        {
            LoggerDataContainer loggerDataContainer = new LoggerDataContainer(new Logger());

            bool disposedBefore = loggerDataContainer.FilesContainer._disposed;

            loggerDataContainer.Dispose();

            bool disposedAfter = loggerDataContainer.FilesContainer._disposed;

            Assert.IsFalse(disposedBefore);
            Assert.IsTrue(disposedAfter);
        }
    }
}
