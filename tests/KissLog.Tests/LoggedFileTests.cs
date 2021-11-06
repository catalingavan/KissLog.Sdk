using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KissLog.Tests
{
    [TestClass]
    public class LoggedFileTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void NullFileNameThrowsException(string fileName)
        {
            LoggedFile item = new LoggedFile(fileName, @"D:\my\file.txt", 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void NullFilePathThrowsException(string filePath)
        {
            LoggedFile item = new LoggedFile("File", filePath, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        [DataRow(-1)]
        [DataRow(-100)]
        [DataRow(-1000)]
        public void NegativeLengthThrowsException(long length)
        {
            LoggedFile item = new LoggedFile("File", @"D:\my\file.txt", length);
        }
    }
}
