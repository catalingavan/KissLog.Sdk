using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace KissLog.Tests
{
    [TestClass]
    public class LogMessagesGroupTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void NullCategoryNameThrowsException(string categoryName)
        {
            LogMessagesGroup item = new LogMessagesGroup(categoryName, new List<LogMessage>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullMessagesThrowsException()
        {
            LogMessagesGroup item = new LogMessagesGroup(Constants.DefaultLoggerCategoryName, null);
        }

        [TestMethod]
        public void CloneCopiesAllTheProperties()
        {
            LogMessagesGroup item = CommonTestHelpers.Factory.CreateLogMessagesGroup();

            System.Threading.Thread.Sleep(100);

            LogMessagesGroup clone = item.Clone();

            Assert.AreEqual(JsonSerializer.Serialize(item), JsonSerializer.Serialize(clone));
        }
    }
}
