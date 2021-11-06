using KissLog.ReadStream;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Text;

namespace KissLog.Tests.ReadStream
{
    [TestClass]
    public class ReadStreamStrategyFactoryTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenStreamIsNull()
        {
            ReadStreamStrategyFactory.Create(null, Encoding.UTF8, "text/plain");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenEncodingIsNull()
        {
            var stream = new Mock<Stream>();

            ReadStreamStrategyFactory.Create(stream.Object, null, "text/plain");
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void DoesNotThrowExceptionWhenContentTypeIsNull(string contentType)
        {
            var stream = new Mock<Stream>();

            ReadStreamStrategyFactory.Create(stream.Object, Encoding.UTF8, contentType);
        }

        [TestMethod]
        public void ReturnsReadStreamAsStringWhenContentSizeIsLteThanMaximumContentLength()
        {
            var stream = new Mock<Stream>();
            stream.Setup(p => p.CanRead).Returns(true);
            stream.Setup(p => p.Length).Returns(Constants.ReadStreamAsStringMaxContentLengthInBytes);

            IReadStreamStrategy strategy = ReadStreamStrategyFactory.Create(stream.Object, Encoding.UTF8, "text/plain");

            Assert.IsInstanceOfType(strategy, typeof(ReadStreamAsString));
        }

        [TestMethod]
        public void ReturnsReadStreamToTemporaryFileWhenContentSizeIsGtThanMaximumContentLength()
        {
            var stream = new Mock<Stream>();
            stream.Setup(p => p.CanRead).Returns(true);
            stream.Setup(p => p.Length).Returns(Constants.ReadStreamAsStringMaxContentLengthInBytes + 1);

            IReadStreamStrategy strategy = ReadStreamStrategyFactory.Create(stream.Object, Encoding.UTF8, "text/plain");

            Assert.IsInstanceOfType(strategy, typeof(ReadStreamToTemporaryFile));
        }

        [TestMethod]
        [DataRow("application/json")]
        [DataRow("application/xml")]
        [DataRow("text/plain")]
        [DataRow("text/xml")]
        public void ReturnsReadStreamAsStringForValidContentTypes(string contentType)
        {
            var stream = new Mock<Stream>();
            stream.Setup(p => p.CanRead).Returns(true);
            stream.Setup(p => p.Length).Returns(Constants.ReadStreamAsStringMaxContentLengthInBytes);

            IReadStreamStrategy strategy = ReadStreamStrategyFactory.Create(stream.Object, Encoding.UTF8, contentType);

            Assert.IsInstanceOfType(strategy, typeof(ReadStreamAsString));
        }

        [TestMethod]
        public void ReturnsNullReadStreamWhenStreamIsDisposed()
        {
            var stream = new Mock<Stream>();
            stream.Setup(p => p.CanRead).Returns(false);

            IReadStreamStrategy strategy = ReadStreamStrategyFactory.Create(stream.Object, Encoding.UTF8, "text/plain");

            Assert.IsInstanceOfType(strategy, typeof(NullReadStream));
        }
    }
}
