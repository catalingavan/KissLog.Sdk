using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text.Json;

namespace KissLog.Tests
{
    [TestClass]
    public class CapturedExceptionTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullExceptionThrowsException()
        {
            CapturedException item = new CapturedException(null);
        }

        [TestMethod]
        public void ExceptionType()
        {
            CapturedException item = new CapturedException(new FileNotFoundException());

            Assert.AreEqual(typeof(FileNotFoundException).FullName, item.Type);
        }

        [TestMethod]
        public void ExceptionMessage()
        {
            string exceptionMessage = $"Exception {Guid.NewGuid()}";
            CapturedException item = new CapturedException(new Exception(exceptionMessage));

            Assert.AreEqual(exceptionMessage, item.Message);
        }

        [TestMethod]
        public void ExceptionString()
        {
            var ex = new NotImplementedException($"This method is not implemented {Guid.NewGuid()}");

            CapturedException item = new CapturedException(ex);

            Assert.AreEqual(ex.ToString(), item.ExceptionString);
        }
    }
}
