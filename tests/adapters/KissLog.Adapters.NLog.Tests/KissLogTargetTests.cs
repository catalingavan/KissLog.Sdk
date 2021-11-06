using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KissLog.Adapters.NLog.Tests
{
    [TestClass]
    public class KissLogTargetTests
    {
        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void GetLogLevelReturnsTraceForNullLogLevelName(string logLevelName)
        {
            var target = new KissLogTarget();
            LogLevel result = target.GetLogLevel(logLevelName);

            Assert.AreEqual(LogLevel.Trace, result);
        }

        [TestMethod]
        [DataRow("Trace", LogLevel.Trace)]
        [DataRow("Debug", LogLevel.Debug)]
        [DataRow("Info", LogLevel.Information)]
        [DataRow("Information", LogLevel.Information)]
        [DataRow("Warn", LogLevel.Warning)]
        [DataRow("Warning", LogLevel.Warning)]
        [DataRow("Error", LogLevel.Error)]
        [DataRow("Fatal", LogLevel.Critical)]
        public void GetLogLevelReturnsCorrectValue(string logLevelName, LogLevel expectedLogLevel)
        {
            var target = new KissLogTarget();
            LogLevel result = target.GetLogLevel(logLevelName);

            Assert.AreEqual(expectedLogLevel, result);
        }

        [TestMethod]
        [DataRow(" trace ", LogLevel.Trace)]
        [DataRow(" debug ", LogLevel.Debug)]
        [DataRow(" info ", LogLevel.Information)]
        [DataRow(" information ", LogLevel.Information)]
        [DataRow(" warn ", LogLevel.Warning)]
        [DataRow(" warning ", LogLevel.Warning)]
        [DataRow(" error ", LogLevel.Error)]
        [DataRow(" fatal ", LogLevel.Critical)]
        public void GetLogLevelHandlesUnformattedLogLevelName(string logLevelName, LogLevel expectedLogLevel)
        {
            var target = new KissLogTarget();
            LogLevel result = target.GetLogLevel(logLevelName);

            Assert.AreEqual(expectedLogLevel, result);
        }
    }
}
