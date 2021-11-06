using KissLog.Exceptions;
using KissLog.LoggerData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KissLog.Tests.LoggerData
{
    [TestClass]
    public class FilesContainerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullLoggerThrowsException()
        {
            FilesContainer filesContainer = new FilesContainer(null);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        public void EmptyStringIsIgnored(string content)
        {
            using (FilesContainer filesContainer = new FilesContainer(new Logger()))
            {
                LoggedFile loggedFile = filesContainer.LogAsFile(content, "File.txt");

                Assert.IsNull(loggedFile);
            }
        }

        [TestMethod]
        public void EmptyByteArrayIsIgnored()
        {
            using (FilesContainer filesContainer = new FilesContainer(new Logger()))
            {
                LoggedFile loggedFile = filesContainer.LogAsFile(new byte[] { }, "File.txt");

                Assert.IsNull(loggedFile);
            }
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("File")]
        [DataRow("File.txt")]
        [DataRow("\\File.txt")]
        [DataRow("File.txt\\")]
        [DataRow("/File/Name")]
        public void FileNameAlwaysHasValue(string fileName)
        {
            using (FilesContainer filesContainer = new FilesContainer(new Logger()))
            {
                LoggedFile loggedFile = filesContainer.LogAsFile("Content", fileName);

                Assert.IsNotNull(loggedFile.FileName);
            }
        }

        [TestMethod]
        public void DisposeAlsoDisposesTheTemporaryFiles()
        {
            FilesContainer filesContainer = new FilesContainer(new Logger());

            filesContainer.LogAsFile("Content", null);
            filesContainer.LogAsFile("Content", null);

            List<TemporaryFile> temporaryFiles = filesContainer._temporaryFiles;

            filesContainer.Dispose();

            foreach(TemporaryFile item in temporaryFiles)
            {
                Assert.IsTrue(item._disposed);
            }
        }

        [TestMethod]
        public void DisposeUpdatesTheDisposedField()
        {
            FilesContainer filesContainer = new FilesContainer(new Logger());

            bool disposedBefore = filesContainer._disposed;

            filesContainer.Dispose();

            bool disposedAfter = filesContainer._disposed;

            Assert.IsFalse(disposedBefore);
            Assert.IsTrue(disposedAfter);
        }

        [TestMethod]
        public void LogStringAsFileCreatesAPhysicalFile()
        {
            using (FilesContainer filesContainer = new FilesContainer(new Logger()))
            {
                string contents = "String file content";

                LoggedFile file = filesContainer.LogAsFile(contents, null);

                FileInfo fi = new FileInfo(file.FilePath);

                Assert.IsTrue(fi.Exists);
                Assert.IsNotNull(file.FileName);
                Assert.AreEqual(fi.Length, file.FileSize);
            }
        }

        [TestMethod]
        public void LogByteArrayAsFileCreatesAPhysicalFile()
        {
            using (FilesContainer filesContainer = new FilesContainer(new Logger()))
            {
                byte[] contents = Encoding.UTF8.GetBytes("Byte[] file content");

                LoggedFile file = filesContainer.LogAsFile(contents, null);

                FileInfo fi = new FileInfo(file.FilePath);

                Assert.IsTrue(fi.Exists);
                Assert.IsNotNull(file.FileName);
                Assert.AreEqual(fi.Length, file.FileSize);
            }
        }

        [TestMethod]
        public void LogFileCreatesAPhysicalCopyOfTheSourceFile()
        {
            using (TemporaryFile sourceFile = new TemporaryFile())
            {
                File.WriteAllText(sourceFile.FileName, "Source file content");

                string copiedFilePath = null;
                bool exists = false;
                long sourceFileLength = sourceFile.GetSize();

                using (FilesContainer filesContainer = new FilesContainer(new Logger()))
                {
                    LoggedFile file = filesContainer.LogFile(sourceFile.FileName, null);
                    copiedFilePath = file.FilePath;

                    exists = File.Exists(copiedFilePath);
                    Assert.IsTrue(exists);
                    Assert.AreEqual(sourceFileLength, file.FileSize);
                }

                exists = File.Exists(copiedFilePath);
                Assert.IsFalse(exists);

                exists = File.Exists(sourceFile.FileName);
                Assert.IsTrue(exists);
            }
        }

        [TestMethod]
        public void LogFileCopiesTheContentsOfTheSourceFile()
        {
            using (TemporaryFile sourceFile = new TemporaryFile())
            {
                string sourceContent = "Source file content";

                File.WriteAllText(sourceFile.FileName, sourceContent);

                using (FilesContainer filesContainer = new FilesContainer(new Logger()))
                {
                    LoggedFile file = filesContainer.LogFile(sourceFile.FileName, null);

                    string content = File.ReadAllText(file.FilePath);

                    Assert.AreEqual(sourceContent, content);
                }
            }
        }

        [TestMethod]
        public void GetLoggedFilesReturnsEmptyList()
        {
            FilesContainer filesContainer = new FilesContainer(new Logger());

            List<LoggedFile> loggedFiles = filesContainer.GetLoggedFiles();

            Assert.IsNotNull(loggedFiles);
            Assert.AreEqual(0, loggedFiles.Count);
        }

        [TestMethod]
        public void GetLoggedFilesIsNotReferenced()
        {
            using (FilesContainer filesContainer = new FilesContainer(new Logger()))
            {
                List<LoggedFile> loggedFiles = filesContainer.GetLoggedFiles();

                filesContainer.LogAsFile("Content", null);

                Assert.AreEqual(0, loggedFiles.Count);
            }
        }

        [TestMethod]
        [DataRow(null, "File 1")]
        [DataRow("", "File 1")]
        [DataRow(" ", "File 1")]
        [DataRow("!@#$%^&*()", "File 1")]
        [DataRow("File", "File")]
        [DataRow("File.txt", "File.txt")]
        [DataRow("\\File.txt", "File.txt")]
        [DataRow("File.txt\\", "File.txt")]
        [DataRow("/File/Name", "FileName")]
        public void GenerateFileNameHandlesVariousInputs(string input, string expectedValue)
        {
            FilesContainer filesContainer = new FilesContainer(new Logger());

            string value = filesContainer.NormalizeFileName(input);

            Assert.AreEqual(expectedValue, value);
        }

        [TestMethod]
        public void LogStringAsFileLogsErrorMessageWhenFileSizeIsTooLarge()
        {
            int maxFileSize = Convert.ToInt32(Constants.MaximumAllowedFileSizeInBytes);
            string contents = string.Join(string.Empty, Enumerable.Range(0, maxFileSize + 1).Select(p => "0"));
            var ex = new FileSizeTooLargeException(contents.Length, Constants.MaximumAllowedFileSizeInBytes);

            Logger logger = new Logger();

            using (FilesContainer filesContainer = new FilesContainer(logger))
            {
                LoggedFile file = filesContainer.LogAsFile(contents, null);
            }

            LogMessage message = logger.DataContainer.LogMessages.FirstOrDefault();

            Assert.IsNotNull(message);
            Assert.AreEqual(LogLevel.Error, message.LogLevel);
            Assert.IsTrue(message.Message.Contains(ex.Message));
        }

        [TestMethod]
        public void LogByteArrayAsFileLogsErrorMessageWhenFileSizeIsTooLarge()
        {
            int maxFileSize = Convert.ToInt32(Constants.MaximumAllowedFileSizeInBytes);
            byte[] contents = Enumerable.Range(0, maxFileSize + 1).Select(p => byte.MaxValue).ToArray();
            var ex = new FileSizeTooLargeException(contents.Length, Constants.MaximumAllowedFileSizeInBytes);

            Logger logger = new Logger();

            using (FilesContainer filesContainer = new FilesContainer(logger))
            {
                LoggedFile file = filesContainer.LogAsFile(contents, null);
            }

            LogMessage message = logger.DataContainer.LogMessages.FirstOrDefault();

            Assert.IsNotNull(message);
            Assert.AreEqual(LogLevel.Error, message.LogLevel);
            Assert.IsTrue(message.Message.Contains(ex.Message));
        }

        [TestMethod]
        public void LogFileLogsErrorMessageWhenFileSizeIsTooLarge()
        {
            int maxFileSize = Convert.ToInt32(Constants.MaximumAllowedFileSizeInBytes);
            byte[] contents = Enumerable.Range(0, maxFileSize + 1).Select(p => byte.MaxValue).ToArray();
            var ex = new FileSizeTooLargeException(contents.Length, Constants.MaximumAllowedFileSizeInBytes);

            Logger logger = new Logger();

            using (TemporaryFile sourceFile = new TemporaryFile())
            {
                File.WriteAllBytes(sourceFile.FileName, contents);

                using (FilesContainer filesContainer = new FilesContainer(logger))
                {
                    LoggedFile file = filesContainer.LogFile(sourceFile.FileName, null);
                }
            }

            LogMessage message = logger.DataContainer.LogMessages.FirstOrDefault();

            Assert.IsNotNull(message);
            Assert.AreEqual(LogLevel.Error, message.LogLevel);
            Assert.IsTrue(message.Message.Contains(ex.Message));
        }

        [TestMethod]
        public void LogFileLogsErrorMessageWhenFileWasNotFound()
        {
            string fileName = null;
            using(TemporaryFile tempFile = new TemporaryFile())
            {
                fileName = tempFile.FileName;
            }

            var ex = new LogFileException(fileName, new FileNotFoundException(null, fileName));

            Logger logger = new Logger();
            using (FilesContainer filesContainer = new FilesContainer(logger))
            {
                LoggedFile file = filesContainer.LogFile(fileName);
            }

            LogMessage message = logger.DataContainer.LogMessages.FirstOrDefault();

            Assert.IsNotNull(message);
            Assert.AreEqual(LogLevel.Error, message.LogLevel);
            Assert.IsTrue(message.Message.Contains(ex.Message));
        }
    }
}
