using KissLog.CloudListeners.RequestLogsListener;
using KissLog.RestClient;
using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KissLog.CloudListeners.Tests.RequestLogsListener
{
    [TestClass]
    public class ExceptionArgsTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullFlushLogArgsThrowsException()
        {
            var result = new ExceptionArgs(null, new RestClient.ApiResult());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullApiResultThrowsException()
        {
            FlushLogArgs flushLogArgs = CommonTestHelpers.Factory.CreateFlushLogArgs();

            var result = new ExceptionArgs(flushLogArgs, null);
        }

        [TestMethod]
        public void ConstructorUpdatesTheProperties()
        {
            FlushLogArgs flushLogArgs = CommonTestHelpers.Factory.CreateFlushLogArgs();
            ApiResult apiResult = new ApiResult();

            var result = new ExceptionArgs(flushLogArgs, apiResult);

            Assert.AreSame(flushLogArgs, result.FlushArgs);
            Assert.AreSame(apiResult, result.ApiResult);
        }
    }
}
