using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace KissLog.Tests.ExtensionMethods
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
            Logger logger = new Logger();

            string content = "String file content";

            logger.LogAsFile(content);

            List<LoggedFile> loggedFiles = logger.DataContainer.FilesContainer.GetLoggedFiles();

            // clean-up the temporary files
            logger.Reset();

            Assert.AreEqual(1, loggedFiles.Count);
        }

        [TestMethod]
        public void LogByteArrayAsFileCreatesALoggedFile()
        {
            Logger logger = new Logger();

            byte[] content = Encoding.UTF8.GetBytes("Byte[] file content");

            logger.LogAsFile(content);

            List<LoggedFile> loggedFiles = logger.DataContainer.FilesContainer.GetLoggedFiles();

            logger.Reset();

            Assert.AreEqual(1, loggedFiles.Count);
        }

        [TestMethod]
        public void LogFileCreatesALoggedFile()
        {
            using (TemporaryFile sourceFile = new TemporaryFile())
            {
                Logger logger = new Logger();

                logger.LogFile(sourceFile.FileName);

                List<LoggedFile> loggedFiles = logger.DataContainer.FilesContainer.GetLoggedFiles();

                logger.Reset();

                Assert.AreEqual(1, loggedFiles.Count);
            }
        }
    }
}
