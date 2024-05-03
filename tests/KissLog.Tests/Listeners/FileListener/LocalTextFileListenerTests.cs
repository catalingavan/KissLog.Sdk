using KissLog.Listeners.FileListener;
using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace KissLog.Tests.Listeners.FileListener
{
    [TestClass]
    public class LocalTextFileListenerTests
    {
        [TestMethod]
        public void GetFileNameIsNotNull()
        {
            var listener = new LocalTextFileListener("logs");
            string fileName = listener.GetFileName();

            Assert.IsFalse(string.IsNullOrWhiteSpace(fileName));
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("logs")]
        [DataRow("\\logs")]
        [DataRow("logs\\")]
        [DataRow("C:\\my-app\\logs")]
        public void LogsDirectoryPathDoesNotThrowException(string logsDirectoryPath)
        {
            var listener = new LocalTextFileListener(logsDirectoryPath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [DataRow(FlushTrigger.OnFlush)]
        [DataRow(FlushTrigger.OnMessage)]
        public void NullTextFormatterThrowsException(FlushTrigger flushTrigger)
        {
            var listener = new LocalTextFileListener("logs", flushTrigger, null);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("logs")]
        [DataRow("\\logs")]
        [DataRow("logs\\")]
        [DataRow("C:\\logs")]
        [DataRow("C:/logs")]
        public void NormalizeLogsDirectoryPathReturnsFullyQualifiedPath(string logsDirectoryPath)
        {
            var listener = new LocalTextFileListener(logsDirectoryPath);
            string path = listener.NormalizeLogsDirectoryPath(logsDirectoryPath);

            Assert.IsTrue(Path.IsPathRooted(path));
        }

        [TestMethod]
        [DataRow(FlushTrigger.OnFlush)]
        [DataRow(FlushTrigger.OnMessage)]
        [Ignore] // On GitHub actions, creating directories doesn't work
        public void GetFileNameReflectsTheFileName(FlushTrigger flushTrigger)
        {
            string filename = $"{Guid.NewGuid()}.log";
            string logsDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tests", $"logs-{Guid.NewGuid()}");
            var listener = new LocalTextFileListener(logsDirectoryPath, flushTrigger)
            {
                GetFileName = () => filename
            };

            FlushLogArgs flushLogArgs = CommonTestHelpers.Factory.CreateFlushLogArgs();
            listener.OnFlush(flushLogArgs);

            try
            {
                Assert.IsTrue(File.Exists(Path.Combine(logsDirectoryPath, filename)));
            }
            finally
            {
                if (Directory.Exists(logsDirectoryPath))
                    Directory.Delete(logsDirectoryPath, true);
            }
        }


        [TestMethod]
        [Ignore] // this test fails on GitHub actions
        public void GetFilePathCreatesTheDirectoryIfDoesntExist()
        {
            string logsDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tests", $"logs-{Guid.NewGuid()}");

            var listener = new LocalTextFileListener(logsDirectoryPath);
            listener.GetFilePath();

            try
            {
                Assert.IsTrue(Directory.Exists(logsDirectoryPath));
            }
            finally
            {
                if (Directory.Exists(logsDirectoryPath))
                    Directory.Delete(logsDirectoryPath, true);
            }
        }
    }
}
