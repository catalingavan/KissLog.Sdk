using KissLog.LogResponseBody;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KissLog.Tests.LogResponseBody
{
    [TestClass]
    public class LogResponseBodyStrategyFactoryTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenStreamIsNull()
        {
            LogResponseBodyStrategyFactory.Create(null, Encoding.UTF8, new Logger());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenEncodingIsNull()
        {
            var stream = new Mock<Stream>();

            LogResponseBodyStrategyFactory.Create(stream.Object, null, new Logger());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenLoggerIsNull()
        {
            var stream = new Mock<Stream>();

            LogResponseBodyStrategyFactory.Create(stream.Object, Encoding.UTF8, null);
        }

        [TestMethod]
        public void ReturnsNullStrategyWhenStreamIsDisposed()
        {
            var stream = new Mock<Stream>();
            stream.Setup(p => p.CanRead).Returns(false);

            ILogResponseBodyStrategy strategy = LogResponseBodyStrategyFactory.Create(stream.Object, Encoding.UTF8, new Logger());

            Assert.IsInstanceOfType(strategy, typeof(NullLogResponseBody));
        }

        [TestMethod]
        public void ReturnLogResponseBodySizeTooLargeExceptionWhenContentSizeIsGreaterThanMaximumAllowedFileSize()
        {
            var stream = new Mock<Stream>();
            stream.Setup(p => p.CanRead).Returns(true);
            stream.Setup(p => p.Length).Returns(Constants.MaximumAllowedFileSizeInBytes + 1);

            ILogResponseBodyStrategy strategy = LogResponseBodyStrategyFactory.Create(stream.Object, Encoding.UTF8, new Logger());

            Assert.IsInstanceOfType(strategy, typeof(LogResponseBodySizeTooLargeException));
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(Constants.MaximumAllowedFileSizeInBytes)]
        public void DoesNotReturnLogResponseBodySizeTooLargeExceptionWhenContentSizeIsLteThanMaximumAllowedFileSize(long contentLength)
        {
            var stream = new Mock<Stream>();
            stream.Setup(p => p.CanRead).Returns(true);
            stream.Setup(p => p.Length).Returns(contentLength);

            Logger logger = new Logger(url: "/");
            logger.DataContainer.HttpProperties.SetResponse(new KissLog.Http.HttpResponse(new KissLog.Http.HttpResponse.CreateOptions
            {
                Properties = new KissLog.Http.ResponseProperties(new KissLog.Http.ResponseProperties.CreateOptions
                {
                    Headers = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("Content-Type", "application/json")
                    }
                })
            }));

            ILogResponseBodyStrategy strategy = LogResponseBodyStrategyFactory.Create(stream.Object, Encoding.UTF8, logger);

            Assert.IsNotInstanceOfType(strategy, typeof(LogResponseBodySizeTooLargeException));
        }
    }
}
