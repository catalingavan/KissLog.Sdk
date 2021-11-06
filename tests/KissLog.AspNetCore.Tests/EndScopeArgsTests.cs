using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace KissLog.AspNetCore.Tests
{
    [TestClass]
    public class EndScopeArgsTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenLoggerIsNull()
        {
            var args = new EndScopeArgs(null, new Dictionary<string, object>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenScopeDataDictionaryIsNull()
        {
            var args = new EndScopeArgs(new Logger(), null);
        }
    }
}
