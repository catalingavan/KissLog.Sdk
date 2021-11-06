using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KissLog.AspNetCore.Tests
{
    [TestClass]
    public class FormatterArgsTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullCreateOptionsThrowsException()
        {
            FormatterArgs item = new FormatterArgs(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullLoggerThrowsException()
        {
            FormatterArgs item = new FormatterArgs(new FormatterArgs.CreateOptions
            {
                Logger = null
            });
        }

        [TestMethod]
        public void CreateOptionsConstructorUpdatesProperties()
        {
            var options = new FormatterArgs.CreateOptions
            {
                State = new { },
                Exception = new Exception(),
                DefaultValue = $"Log-{Guid.NewGuid()}",
                Logger = new Logger()
            };

            FormatterArgs item = new FormatterArgs(options);

            Assert.AreSame(options.State, item.State);
            Assert.AreSame(options.Exception, item.Exception);
            Assert.AreEqual(options.DefaultValue, item.DefaultValue);
        }
    }
}
