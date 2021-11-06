using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KissLog.Tests
{
    [TestClass]
    public class TemporaryFileTests
    {
        [TestMethod]
        public void CreatesEmptyFile()
        {
            TemporaryFile file = new TemporaryFile();

            FileInfo fi = new FileInfo(file.FileName);
            bool exists = fi.Exists;
            long length = fi.Length;

            file.Dispose();

            Assert.IsTrue(exists);
            Assert.AreEqual(0, length);
        }

        [TestMethod]
        public void DisposeDeletesTheFile()
        {
            TemporaryFile file = new TemporaryFile();

            bool existsBeforeDispose = File.Exists(file.FileName);

            file.Dispose();

            bool existsAfterDispose = File.Exists(file.FileName);

            Assert.IsTrue(existsBeforeDispose);
            Assert.IsFalse(existsAfterDispose);
        }

        [TestMethod]
        public void DisposeUpdatesTheDisposedField()
        {
            TemporaryFile file = new TemporaryFile();

            bool disposedBefore = file._disposed;

            file.Dispose();

            bool disposedAfter = file._disposed;

            Assert.IsFalse(disposedBefore);
            Assert.IsTrue(disposedAfter);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(".")]
        [DataRow("png")]
        [DataRow("txt")]
        [DataRow("pdf")]
        [DataRow("exe")]
        public void FileNameAlwaysHasExtension(string extension)
        {
            TemporaryFile file = new TemporaryFile(extension);

            string fileExtension = Path.GetExtension(file.FileName).Substring(1);

            file.Dispose();

            Assert.IsTrue(string.IsNullOrWhiteSpace(fileExtension) == false);
            Assert.IsTrue(fileExtension.Length > 1);
        }

        [TestMethod]
        public void AllowedExtensionReflectsInFileName()
        {
            List<string> extensions = TemporaryFile.AllowedExtensions.Select(p => p).ToList();

            foreach(string extension in extensions)
            {
                using(TemporaryFile file = new TemporaryFile(extension))
                {
                    string fileExtension = Path.GetExtension(file.FileName).Substring(1);
                    Assert.AreEqual(extension, fileExtension);
                }
            }
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(".")]
        [DataRow("exe")]
        [DataRow(".exe")]
        public void NotAllowedExtensionGeneratesDefaultExtension(string extension)
        {
            TemporaryFile file = new TemporaryFile(extension);

            string fileExtension = Path.GetExtension(file.FileName).Substring(1);

            file.Dispose();

            Assert.AreEqual(TemporaryFile.DefaultExtension, fileExtension);
        }

        [TestMethod]
        [DataRow(".txt", "txt")]
        [DataRow("..txt", "txt")]
        public void ExtensionContainingDotIsHandled(string extension, string expectedExtension)
        {
            TemporaryFile file = new TemporaryFile(extension);

            string fileExtension = Path.GetExtension(file.FileName).Substring(1);

            file.Dispose();

            Assert.AreEqual(expectedExtension, fileExtension);
        }

        [TestMethod]
        [DataRow("TXT", "txt")]
        [DataRow(".TXT", "txt")]
        [DataRow("EXE", TemporaryFile.DefaultExtension)]
        [DataRow(".EXE", TemporaryFile.DefaultExtension)]
        public void ExtensionIsInLowerCase(string extension, string expectedExtension)
        {
            TemporaryFile file = new TemporaryFile(extension);

            string fileExtension = Path.GetExtension(file.FileName).Substring(1);

            file.Dispose();

            Assert.AreEqual(expectedExtension, fileExtension);
        }
    }
}
