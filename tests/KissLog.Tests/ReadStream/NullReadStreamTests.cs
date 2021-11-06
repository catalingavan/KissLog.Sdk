using KissLog.ReadStream;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KissLog.Tests.ReadStream
{
    [TestClass]
    public class NullReadStreamTests
    {
        [TestMethod]
        public void DoesNotReturnNull()
        {
            var service = new NullReadStream();

            var result = service.Read();

            Assert.IsNotNull(result);
        }
    }
}
