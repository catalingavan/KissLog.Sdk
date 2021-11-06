using KissLog.Listeners;
using KissLog.Listeners.FileListener;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace KissLog.Tests.Listeners.FileListener
{
    [TestClass]
    public class FlushTextFileListenerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionForNullLocalTextFileListener()
        {
            ITextFormatter textFormatter = new Mock<ITextFormatter>().Object;
            var listener = new FlushTextFileListener(null, textFormatter);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionForNullTextFormatter()
        {
            var localTextFileListener = new LocalTextFileListener("logs");
            var listener = new FlushTextFileListener(localTextFileListener, null);
        }
    }
}
