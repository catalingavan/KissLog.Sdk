using KissLog.ReadStream;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

namespace KissLog.Tests.ReadStream
{
    [TestClass]
    public class ReadStreamToTemporaryFileTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenStreamIsNull()
        {
            var service = new ReadStreamToTemporaryFile(null);
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

            var service = new ReadStreamToTemporaryFile(stream);

            stream.Dispose();

            var result = service.Read();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ReadsTheStream()
        {
            string value = Guid.NewGuid().ToString();
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));

            var service = new ReadStreamToTemporaryFile(stream);
            var result = service.Read();

            string content = File.ReadAllText(result.TemporaryFile.FileName);

            result.TemporaryFile.Dispose();

            Assert.AreEqual(value, content);
        }

        [TestMethod]
        public void DoesNotDisposeTheStream()
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));

            var service = new ReadStreamToTemporaryFile(stream);
            var result = service.Read();

            result.TemporaryFile.Dispose();

            Assert.IsTrue(stream.CanRead == true);
        }
    }
}
