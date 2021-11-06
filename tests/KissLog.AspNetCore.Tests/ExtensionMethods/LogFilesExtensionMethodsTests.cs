using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace KissLog.AspNetCore.Tests
{
    [TestClass]
    public class LogFilesExtensionMethodsTests
    {
        [TestMethod]
        public void NullLoggerDoesNotThrowException()
        {
            Type t = typeof(LogFilesExtensionMethods);
            List<MethodInfo> methods = t.GetMethods(BindingFlags.Public | BindingFlags.Static).ToList();

            foreach (MethodInfo method in methods)
            {
                object[] parameters = method.GetParameters().Select(p => p.ParameterType.IsValueType ? Activator.CreateInstance(p.ParameterType) : null).ToArray();
                method.Invoke(null, parameters);
            }
        }

        [TestMethod]
        public void LogStringAsFileCreatesALoggedFile()
        {
            Logger kisslogger = new Logger();

            var options = new LoggerOptions
            {
                Factory = new KissLog.LoggerFactory(kisslogger)
            };

            ILogger logger = new LoggerAdapter(options);

            logger.LogAsFile("String file content");

            List<LoggedFile> loggedFiles = kisslogger.DataContainer.FilesContainer.GetLoggedFiles();

            // clean-up the temporary files
            kisslogger.Reset();

            Assert.AreEqual(1, loggedFiles.Count);
        }


        [TestMethod]
        public void LogByteArrayAsFileCreatesALoggedFile()
        {
            Logger kisslogger = new Logger();

            var options = new LoggerOptions
            {
                Factory = new KissLog.LoggerFactory(kisslogger)
            };

            ILogger logger = new LoggerAdapter(options);

            byte[] content = Encoding.UTF8.GetBytes("Byte[] file content");

            logger.LogAsFile(content);

            List<LoggedFile> loggedFiles = kisslogger.DataContainer.FilesContainer.GetLoggedFiles();

            kisslogger.Reset();

            Assert.AreEqual(1, loggedFiles.Count);
        }

        [TestMethod]
        public void LogFileCreatesALoggedFile()
        {
            using (TemporaryFile sourceFile = new TemporaryFile())
            {
                Logger kisslogger = new Logger();

                var options = new LoggerOptions
                {
                    Factory = new KissLog.LoggerFactory(kisslogger)
                };

                ILogger logger = new LoggerAdapter(options);

                logger.LogFile(sourceFile.FileName);

                List<LoggedFile> loggedFiles = kisslogger.DataContainer.FilesContainer.GetLoggedFiles();

                kisslogger.Reset();

                Assert.AreEqual(1, loggedFiles.Count);
            }
        }
    }
}
