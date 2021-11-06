using KissLog.ReadStream;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Text;

namespace KissLog.Tests.ReadStream
{
    [TestClass]
    public class ReadStreamAsStringTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenStreamIsNull()
        {
            var service = new ReadStreamAsString(null, Encoding.UTF8);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenEncodingIsNull()
        {
            var stream = new Mock<Stream>();

            var service = new ReadStreamAsString(stream.Object, null);
        }

        [TestMethod]
        public void DoesNotThrowExceptionWhenStreamIsDisposed()
        {
            var stream = new MemoryStream();
            using (var sw = new StreamWriter(stream))
            {
                sw.Write(Guid.NewGuid().ToString());
                sw.Flush();
            }

            var service = new ReadStreamAsString(stream, Encoding.UTF8);

            stream.Dispose();

            var result = service.Read();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ReadsTheStream()
        {
            string value = Guid.NewGuid().ToString();
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));

            Logger logger = new Logger();

            var service = new ReadStreamAsString(stream, Encoding.UTF8);
            var result = service.Read();

            Assert.AreEqual(result.Content, value);
        }

        [TestMethod]
        public void ReadDisposesTheStream()
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));

            var service = new ReadStreamAsString(stream, Encoding.UTF8);
            service.Read();

            Assert.IsTrue(stream.CanRead == false);
        }
    }
}
