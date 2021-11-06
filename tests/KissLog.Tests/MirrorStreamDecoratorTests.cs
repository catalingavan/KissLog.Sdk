using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

namespace KissLog.Tests
{
    [TestClass]
    public class MirrorStreamDecoratorTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenStreamIsNull()
        {
            var item = new MirrorStreamDecorator(null);
        }

        [TestMethod]
        public void ClosingOrDisposingTheDecoratedStreamDoesNotDisposeTheMirrorStream()
        {
            MirrorStreamDecorator decorator = null;

            using (var ms = new MemoryStream())
            {
                decorator = new MirrorStreamDecorator(ms);
                using (var sw = new StreamWriter(decorator))
                {
                    sw.Write($"Input stream {Guid.NewGuid()}");
                    sw.Flush();
                }

                ms.Close();
            }

            Assert.IsTrue(decorator.MirrorStream.CanRead);
            Assert.IsTrue(decorator.MirrorStream.CanWrite);
        }

        [TestMethod]
        public void DisposeAlsoDisposesDecoratedStream()
        {
            var ms = new MemoryStream();
            var decorator = new MirrorStreamDecorator(ms);

            decorator.Dispose();

            Assert.IsFalse(ms.CanRead);
            Assert.IsFalse(ms.CanWrite);
        }

        [TestMethod]
        public void CopiesContent()
        {
            string body = $"Input stream {Guid.NewGuid()}";
            string result = null;

            var decorator = new MirrorStreamDecorator(new MemoryStream());
            using (var sw = new StreamWriter(decorator))
            {
                sw.Write(body);
                sw.Flush();
            }

            using (StreamReader reader = new StreamReader(decorator.MirrorStream, decorator.Encoding))
            {
                decorator.MirrorStream.Position = 0;
                result = reader.ReadToEndAsync().Result;
            }

            Assert.AreEqual(body, result);
        }

        [TestMethod]
        public void UsesUtf8EncodingIfStreamEncodingIsNull()
        {
            var stream = new MirrorStreamDecorator(new MemoryStream());

            Assert.AreEqual(Encoding.UTF8, stream.Encoding);
        }
    }
}
