using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.Json;

namespace KissLog.Tests
{
    [TestClass]
    public class LogMessageTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullCreateOptionsThrowsException()
        {
            LogMessage item = new LogMessage(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void NullCategoryNameThrowsException(string categoryName)
        {
            LogMessage item = new LogMessage(new LogMessage.CreateOptions
            {
                CategoryName = categoryName,
                LogLevel = LogLevel.Trace,
                Message = "Message"
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void NullMessageThrowsException(string message)
        {
            LogMessage item = new LogMessage(new LogMessage.CreateOptions
            {
                CategoryName = Constants.DefaultLoggerCategoryName,
                LogLevel = LogLevel.Trace,
                Message = message
            });
        }

        [TestMethod]
        public void CreateOptionsConstructorUpdatesProperties()
        {
            var options = new LogMessage.CreateOptions
            {
                CategoryName = $"CategoryName {Guid.NewGuid()}",
                LogLevel = LogLevel.Warning,
                Message = $"Message {Guid.NewGuid()}",
                MemberType = $"MemberType {Guid.NewGuid()}",
                MemberName = $"MemberName {Guid.NewGuid()}",
                LineNumber = 10000
            };

            LogMessage item = new LogMessage(options);

            Assert.AreEqual(options.CategoryName, item.CategoryName);
            Assert.AreEqual(options.LogLevel, item.LogLevel);
            Assert.AreEqual(options.Message, item.Message);
            Assert.AreEqual(options.MemberType, item.MemberType);
            Assert.AreEqual(options.MemberName, item.MemberName);
            Assert.AreEqual(options.LineNumber, item.LineNumber);
        }

        [TestMethod]
        public void DateTimeIsInUtcFormat()
        {
            LogMessage item = new LogMessage(new LogMessage.CreateOptions
            {
                CategoryName = Constants.DefaultLoggerCategoryName,
                LogLevel = LogLevel.Trace,
                Message = "Message"
            });

            Assert.AreEqual(DateTimeKind.Utc, item.DateTime.Kind);
        }

        [TestMethod]
        public void DateTimeHasValue()
        {
            LogMessage item = new LogMessage(new LogMessage.CreateOptions
            {
                CategoryName = Constants.DefaultLoggerCategoryName,
                LogLevel = LogLevel.Trace,
                Message = "Message"
            });

            Assert.IsTrue(item.DateTime < DateTime.UtcNow);
            Assert.IsTrue(item.DateTime.Year > default(DateTime).Year);
        }

        [TestMethod]
        public void CloneCopiesAllTheProperties()
        {
            LogMessage item = CommonTestHelpers.Factory.CreateLogMessage();

            System.Threading.Thread.Sleep(100);

            LogMessage clone = item.Clone();

            Assert.AreEqual(JsonSerializer.Serialize(item), JsonSerializer.Serialize(clone));
        }
    }
}
