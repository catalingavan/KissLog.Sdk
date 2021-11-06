using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KissLog.CloudListeners.Tests.RequestLogsListener
{
    [TestClass]
    public class OptionsTests
    {
        [TestMethod]
        public void HandlersIsNotNull()
        {
            KissLog.CloudListeners.RequestLogsListener.Options options = new KissLog.CloudListeners.RequestLogsListener.Options();

            Assert.IsNotNull(options.Handlers);
        }
    }
}
