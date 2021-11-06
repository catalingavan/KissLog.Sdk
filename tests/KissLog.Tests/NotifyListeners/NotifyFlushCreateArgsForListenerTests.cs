using KissLog.Http;
using KissLog.NotifyListeners;
using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace KissLog.Tests.NotifyListeners
{
    [TestClass]
    public class NotifyFlushCreateArgsForListenerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionForNullFlushLogArgs()
        {
            NotifyFlush.CreateArgsForListener(null, new CustomLogListener());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionForNullLogListener()
        {
            FlushLogArgs flushLogArgs = CommonTestHelpers.Factory.CreateFlushLogArgs();
            NotifyFlush.CreateArgsForListener(flushLogArgs, null);
        }

        [TestMethod]
        public void EvaluatesOptionsShouldLogRequestHeader()
        {
            CommonTestHelpers.ResetContext();

            KissLogConfiguration.Options.ShouldLogRequestHeader((OptionsArgs.LogListenerHeaderArgs args) => false);

            FlushLogArgs flushLogArgs = CommonTestHelpers.Factory.CreateFlushLogArgs();

            FlushLogArgs result = NotifyFlush.CreateArgsForListener(flushLogArgs, new CustomLogListener());

            Assert.AreEqual(0, result.HttpProperties.Request.Properties.Headers.Count());
            Assert.AreNotEqual(0, flushLogArgs.HttpProperties.Request.Properties.Headers.Count());
        }

        [TestMethod]
        public void EvaluatesOptionsShouldLogRequestCookie()
        {
            CommonTestHelpers.ResetContext();

            KissLogConfiguration.Options.ShouldLogRequestCookie((OptionsArgs.LogListenerCookieArgs args) => false);

            FlushLogArgs flushLogArgs = CommonTestHelpers.Factory.CreateFlushLogArgs();

            FlushLogArgs result = NotifyFlush.CreateArgsForListener(flushLogArgs, new CustomLogListener());

            Assert.AreEqual(0, result.HttpProperties.Request.Properties.Cookies.Count());
            Assert.AreNotEqual(0, flushLogArgs.HttpProperties.Request.Properties.Cookies.Count());
        }

        [TestMethod]
        public void EvaluatesOptionsShouldLogRequestFormData()
        {
            CommonTestHelpers.ResetContext();

            KissLogConfiguration.Options.ShouldLogFormData((OptionsArgs.LogListenerFormDataArgs args) => false);

            FlushLogArgs flushLogArgs = CommonTestHelpers.Factory.CreateFlushLogArgs();

            FlushLogArgs result = NotifyFlush.CreateArgsForListener(flushLogArgs, new CustomLogListener());

            Assert.AreEqual(0, result.HttpProperties.Request.Properties.FormData.Count());
            Assert.AreNotEqual(0, flushLogArgs.HttpProperties.Request.Properties.FormData.Count());
        }

        [TestMethod]
        public void EvaluatesOptionsShouldLogRequestServerVariable()
        {
            CommonTestHelpers.ResetContext();

            KissLogConfiguration.Options.ShouldLogServerVariable((OptionsArgs.LogListenerServerVariableArgs args) => false);

            FlushLogArgs flushLogArgs = CommonTestHelpers.Factory.CreateFlushLogArgs();

            FlushLogArgs result = NotifyFlush.CreateArgsForListener(flushLogArgs, new CustomLogListener());

            Assert.AreEqual(0, result.HttpProperties.Request.Properties.ServerVariables.Count());
            Assert.AreNotEqual(0, flushLogArgs.HttpProperties.Request.Properties.ServerVariables.Count());
        }

        [TestMethod]
        public void EvaluatesOptionsShouldLogClaim()
        {
            CommonTestHelpers.ResetContext();

            KissLogConfiguration.Options.ShouldLogClaim((OptionsArgs.LogListenerClaimArgs args) => false);

            FlushLogArgs flushLogArgs = CommonTestHelpers.Factory.CreateFlushLogArgs();

            FlushLogArgs result = NotifyFlush.CreateArgsForListener(flushLogArgs, new CustomLogListener());

            Assert.AreEqual(0, result.HttpProperties.Request.Properties.Claims.Count());
            Assert.AreNotEqual(0, flushLogArgs.HttpProperties.Request.Properties.Claims.Count());
        }

        [TestMethod]
        public void EvaluatesOptionsShouldLogInputStream()
        {
            CommonTestHelpers.ResetContext();

            KissLogConfiguration.Options.ShouldLogInputStream((OptionsArgs.LogListenerInputStreamArgs args) => false);

            FlushLogArgs flushLogArgs = CommonTestHelpers.Factory.CreateFlushLogArgs();

            FlushLogArgs result = NotifyFlush.CreateArgsForListener(flushLogArgs, new CustomLogListener());

            Assert.IsNull(result.HttpProperties.Request.Properties.InputStream);
            Assert.IsNotNull(flushLogArgs.HttpProperties.Request.Properties.InputStream);
        }

        [TestMethod]
        public void EvaluatesOptionsShouldLogResponseHeader()
        {
            CommonTestHelpers.ResetContext();

            KissLogConfiguration.Options.ShouldLogResponseHeader((OptionsArgs.LogListenerHeaderArgs args) => false);

            FlushLogArgs flushLogArgs = CommonTestHelpers.Factory.CreateFlushLogArgs();

            FlushLogArgs result = NotifyFlush.CreateArgsForListener(flushLogArgs, new CustomLogListener());

            Assert.AreEqual(0, result.HttpProperties.Response.Properties.Headers.Count());
            Assert.AreNotEqual(0, flushLogArgs.HttpProperties.Response.Properties.Headers.Count());
        }
    }
}
