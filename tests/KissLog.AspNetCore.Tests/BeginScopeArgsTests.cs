using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace KissLog.AspNetCore.Tests
{
    [TestClass]
    public class BeginScopeArgsTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenLoggerIsNull()
        {
            var args = new BeginScopeArgs("", null, new Dictionary<string, object>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenScopeDataDictionaryIsNull()
        {
            var args = new BeginScopeArgs("", new Logger(), null);
        }

        [TestMethod]
        public void DoesNotThrowExceptionWhenStateIsNull()
        {
            var args = new BeginScopeArgs(null, new Logger(), new Dictionary<string, object>());
        }
    }
}
