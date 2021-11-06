using KissLog.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KissLog.Tests
{
    [TestClass]
    public class OptionsTests
    {
        [TestMethod]
        public void HandlersIsNotNull()
        {
            Options options = new Options();

            Assert.IsNotNull(options.Handlers);
        }

        [TestMethod]
        public void AppendExceptionDetailsUpdatesHandlers()
        {
            Options options = new Options();

            Func<Exception, string> handler = (Exception ex) => "This exception should be handled";

            options.AppendExceptionDetails(handler);

            Assert.AreSame(handler, options.Handlers.AppendExceptionDetails);
        }

        [TestMethod]
        public void ShouldLogRequestHeaderForListenerUpdatesHandlers()
        {
            Options options = new Options();

            Func<OptionsArgs.LogListenerHeaderArgs, bool> handler = (OptionsArgs.LogListenerHeaderArgs args) => true;

            options.ShouldLogRequestHeader(handler);

            Assert.AreSame(handler, options.Handlers.ShouldLogRequestHeaderForListener);
        }

        [TestMethod]
        public void ShouldLogRequestCookieForListenerUpdatesHandlers()
        {
            Options options = new Options();

            Func<OptionsArgs.LogListenerCookieArgs, bool> handler = (OptionsArgs.LogListenerCookieArgs args) => true;

            options.ShouldLogRequestCookie(handler);

            Assert.AreSame(handler, options.Handlers.ShouldLogRequestCookieForListener);
        }

        [TestMethod]
        public void ShouldLogFormDataForListenerUpdatesHandlers()
        {
            Options options = new Options();

            Func<OptionsArgs.LogListenerFormDataArgs, bool> handler = (OptionsArgs.LogListenerFormDataArgs args) => true;

            options.ShouldLogFormData(handler);

            Assert.AreSame(handler, options.Handlers.ShouldLogFormDataForListener);
        }

        [TestMethod]
        public void ShouldLogServerVariableForListenerUpdatesHandlers()
        {
            Options options = new Options();

            Func<OptionsArgs.LogListenerServerVariableArgs, bool> handler = (OptionsArgs.LogListenerServerVariableArgs args) => true;

            options.ShouldLogServerVariable(handler);

            Assert.AreSame(handler, options.Handlers.ShouldLogServerVariableForListener);
        }

        [TestMethod]
        public void ShouldLogClaimForListenerUpdatesHandlers()
        {
            Options options = new Options();

            Func<OptionsArgs.LogListenerClaimArgs, bool> handler = (OptionsArgs.LogListenerClaimArgs args) => true;

            options.ShouldLogClaim(handler);

            Assert.AreSame(handler, options.Handlers.ShouldLogClaimForListener);
        }

        [TestMethod]
        public void ShouldLogInputStreamForListenerUpdatesHandlers()
        {
            Options options = new Options();

            Func<OptionsArgs.LogListenerInputStreamArgs, bool> handler = (OptionsArgs.LogListenerInputStreamArgs args) => true;

            options.ShouldLogInputStream(handler);

            Assert.AreSame(handler, options.Handlers.ShouldLogInputStreamForListener);
        }

        [TestMethod]
        public void ShouldLogResponseHeaderForListenerUpdatesHandlers()
        {
            Options options = new Options();

            Func<OptionsArgs.LogListenerHeaderArgs, bool> handler = (OptionsArgs.LogListenerHeaderArgs args) => true;

            options.ShouldLogResponseHeader(handler);

            Assert.AreSame(handler, options.Handlers.ShouldLogResponseHeaderForListener);
        }

        [TestMethod]
        public void ShouldLogFormDataUpdatesHandlers()
        {
            Options options = new Options();

            Func<HttpRequest, bool> handler = (HttpRequest args) => true;

            options.ShouldLogFormData(handler);

            Assert.AreSame(handler, options.Handlers.ShouldLogFormData);
        }

        [TestMethod]
        public void ShouldLogInputStreamUpdatesHandlers()
        {
            Options options = new Options();

            Func<HttpRequest, bool> handler = (HttpRequest args) => true;

            options.ShouldLogInputStream(handler);

            Assert.AreSame(handler, options.Handlers.ShouldLogInputStream);
        }

        [TestMethod]
        public void ShouldLogResponseBodyUpdatesHandlers()
        {
            Options options = new Options();

            Func<HttpProperties, bool> handler = (HttpProperties args) => true;

            options.ShouldLogResponseBody(handler);

            Assert.AreSame(handler, options.Handlers.ShouldLogResponseBody);
        }
    }
}
