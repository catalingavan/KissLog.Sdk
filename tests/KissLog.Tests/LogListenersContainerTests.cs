using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace KissLog.Tests
{
    [TestClass]
    public class LogListenersContainerTests
    {
        [TestMethod]
        public void LogListenersAreEmpty()
        {
            LogListenersContainer item = new LogListenersContainer();

            int count = item.GetAll().Count;

            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void AddNullLogListenersDoesNotThrowException()
        {
            LogListenersContainer item = new LogListenersContainer();

            item.Add(null);
        }

        [TestMethod]
        public void AddNullLogListenersIsIgnored()
        {
            LogListenersContainer item = new LogListenersContainer();

            item.Add(null);

            int count = item.GetAll().Count;

            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void AddLogListenersUpdatesTheList()
        {
            LogListenersContainer item = new LogListenersContainer();

            item.Add(new CustomLogListener());

            int count = item.GetAll().Count;

            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void GetAllIsNotReferenced()
        {
            LogListenersContainer item = new LogListenersContainer();

            List<LogListenerDecorator> list = item.GetAll();
            list.Add(null);

            int count = item.GetAll().Count;

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(0, count);
        }
    }
}
