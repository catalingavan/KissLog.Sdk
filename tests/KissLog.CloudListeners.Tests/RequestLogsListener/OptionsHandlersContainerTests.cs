using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KissLog.CloudListeners.Tests.RequestLogsListener
{
    [TestClass]
    public class OptionsHandlersContainerTests
    {
        [TestMethod]
        public void CreateUserPayloadIsNotNull()
        {
            var options = new KissLog.CloudListeners.RequestLogsListener.Options();

            Assert.IsNotNull(options.Handlers.CreateUserPayload);
        }

        [TestMethod]
        public void GenerateSearchKeywordsIsNotNull()
        {
            var options = new KissLog.CloudListeners.RequestLogsListener.Options();

            Assert.IsNotNull(options.Handlers.GenerateSearchKeywords);
        }
    }
}
