using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KissLog.AspNet.Web.Tests.KissLogHttpModuleTests
{
    [TestClass]
    public class KissLogHttpModuleTests
    {
        [TestMethod]
        public void UpdatesTheLoggerFactory()
        {
            KissLog.Tests.Common.CommonTestHelpers.ResetContext();

            var module = new KissLogHttpModule();

            Assert.IsNotNull(Logger.Factory);
            Assert.IsInstanceOfType(Logger.Factory, typeof(KissLog.AspNet.Web.LoggerFactory));
        }
    }
}
