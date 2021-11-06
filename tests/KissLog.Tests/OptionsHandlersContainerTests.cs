using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KissLog.Tests
{
    [TestClass]
    public class OptionsHandlersContainerTests
    {
        [TestMethod]
        public void AppendExceptionDetailsIsNotNull()
        {
            var options = new Options();

            Assert.IsNotNull(options.Handlers.AppendExceptionDetails);
        }

        [TestMethod]
        public void ShouldLogRequestHeaderForListenerIsNotNull()
        {
            var options = new Options();

            Assert.IsNotNull(options.Handlers.ShouldLogRequestHeaderForListener);
        }

        [TestMethod]
        public void ShouldLogRequestCookieForListenerIsNotNull()
        {
            var options = new Options();

            Assert.IsNotNull(options.Handlers.ShouldLogRequestCookieForListener);
        }

        [TestMethod]
        public void ShouldLogFormDataForListenerIsNotNull()
        {
            var options = new Options();

            Assert.IsNotNull(options.Handlers.ShouldLogFormDataForListener);
        }

        [TestMethod]
        public void ShouldLogServerVariableForListenerIsNotNull()
        {
            var options = new Options();

            Assert.IsNotNull(options.Handlers.ShouldLogServerVariableForListener);
        }

        [TestMethod]
        public void ShouldLogClaimForListenerIsNotNull()
        {
            var options = new Options();

            Assert.IsNotNull(options.Handlers.ShouldLogClaimForListener);
        }

        [TestMethod]
        public void ShouldLogInputStreamForListenerIsNotNull()
        {
            var options = new Options();

            Assert.IsNotNull(options.Handlers.ShouldLogInputStreamForListener);
        }

        [TestMethod]
        public void ShouldLogResponseHeaderForListenerIsNotNull()
        {
            var options = new Options();

            Assert.IsNotNull(options.Handlers.ShouldLogResponseHeaderForListener);
        }

        [TestMethod]
        public void ShouldLogFormDataIsNotNull()
        {
            var options = new Options();

            Assert.IsNotNull(options.Handlers.ShouldLogFormData);
        }

        [TestMethod]
        public void ShouldLogResponseBodyIsNotNull()
        {
            var options = new Options();

            Assert.IsNotNull(options.Handlers.ShouldLogResponseBody);
        }
    }
}
