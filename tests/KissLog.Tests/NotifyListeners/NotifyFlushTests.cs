using KissLog.Http;
using KissLog.NotifyListeners;
using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace KissLog.Tests.NotifyListeners
{
    [TestClass]
    public class NotifyFlushTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionForNullLoggers()
        {
            NotifyFlush.Notify(null);
        }

        [TestMethod]
        public void DoesNotThrowExceptionForEmptyList()
        {
            NotifyFlush.Notify(new Logger[] { });
        }

        [TestMethod]
        public void NotifyIsInvokedForEachLogListener()
        {
            CommonTestHelpers.ResetContext();

            List<FlushLogArgs> flushArgs = new List<FlushLogArgs>();

            KissLogConfiguration.Listeners.Add(new CustomLogListener(onFlush: (FlushLogArgs arg) => { flushArgs.Add(arg); }));
            KissLogConfiguration.Listeners.Add(new CustomLogListener(onFlush: (FlushLogArgs arg) => { flushArgs.Add(arg); }));
            KissLogConfiguration.Listeners.Add(new CustomLogListener(onFlush: (FlushLogArgs arg) => { flushArgs.Add(arg); }));

            Logger logger = new Logger();

            NotifyFlush.Notify(new[] { logger });

            Assert.AreEqual(3, flushArgs.Count);
        }

        [TestMethod]
        public void NotifyContinuesForOtherListenersWhenOneThrowsAnException()
        {
            CommonTestHelpers.ResetContext();

            List<FlushLogArgs> flushArgs = new List<FlushLogArgs>();

            KissLogConfiguration.Listeners.Add(new CustomLogListener(onFlush: (FlushLogArgs arg) => { throw new Exception(); }));
            KissLogConfiguration.Listeners.Add(new CustomLogListener(onFlush: (FlushLogArgs arg) => { flushArgs.Add(arg); }));
            KissLogConfiguration.Listeners.Add(new CustomLogListener(onFlush: (FlushLogArgs arg) => { flushArgs.Add(arg); }));

            Logger logger = new Logger();

            NotifyFlush.Notify(new[] { logger });

            Assert.AreEqual(2, flushArgs.Count);
        }

        [TestMethod]
        public void NotifyResetsLogger()
        {
            Logger logger = new Logger();
            logger.Trace("Message");
            logger.Error(new Exception());
            logger.LogAsFile("My file");

            // before flush
            Assert.AreEqual(2, logger.DataContainer.LogMessages.Count());
            Assert.AreEqual(1, logger.DataContainer.Exceptions.Count());
            Assert.AreEqual(1, logger.DataContainer.FilesContainer.GetLoggedFiles().Count);

            NotifyFlush.Notify(new[] { logger });

            // after flush
            Assert.AreEqual(0, logger.DataContainer.LogMessages.Count());
            Assert.AreEqual(0, logger.DataContainer.Exceptions.Count());
            Assert.AreEqual(0, logger.DataContainer.FilesContainer.GetLoggedFiles().Count);
        }

        [TestMethod]
        [DataRow(100, false)]
        [DataRow(200, false)]
        [DataRow(300, false)]
        [DataRow(400, true)]
        [DataRow(401, true)]
        [DataRow(403, true)]
        [DataRow(500, true)]
        public void LogListenerInterceptorShouldLogIsEvaluated(int statusCode, bool expectedResult)
        {
            CommonTestHelpers.ResetContext();

            List<FlushLogArgs> args = new List<FlushLogArgs>();

            KissLogConfiguration.Listeners.Add(new CustomLogListener(onFlush: (FlushLogArgs arg) => args.Add(arg))
            {
                Interceptor = new CustomLogListenerInterceptor
                {
                    ShouldLogFlush = (FlushLogArgs args) =>
                    {
                        return args.HttpProperties.Response.StatusCode >= 400;
                    }
                }
            });

            Logger logger = new Logger(url: $"/MyUrl/{statusCode}");
            logger.DataContainer.HttpProperties.SetResponse(new HttpResponse(new HttpResponse.CreateOptions
            {
                StatusCode = statusCode
            }));

            NotifyFlush.Notify(new[] { logger });

            Assert.AreEqual(expectedResult, args.Count == 1);
        }

        [TestMethod]
        public void NotifyShouldNotBeInvokedIfInterceptorShouldLogBeginRequestReturnsFalse()
        {
            CommonTestHelpers.ResetContext();

            List<FlushLogArgs> flushLogArgs = new List<FlushLogArgs>();

            ILogListener listener1 = new CustomLogListener(onFlush: (FlushLogArgs args) => { flushLogArgs.Add(args); })
            {
                Interceptor = new CustomLogListenerInterceptor
                {
                    ShouldLogBeginRequest = (HttpRequest httpRequest) => httpRequest.Url.LocalPath == "/App/Method2"
                }
            };
            KissLogConfiguration.Listeners.Add(listener1);

            ILogListener listener2 = new CustomLogListener(onFlush: (FlushLogArgs args) => { flushLogArgs.Add(args); });
            KissLogConfiguration.Listeners.Add(listener2);

            Logger logger = new Logger(url: "/App/Method1");
            NotifyFlush.Notify(new[] { logger });

            logger = new Logger(url: "/App/Method2");
            NotifyFlush.Notify(new[] { logger });

            logger = new Logger(url: "/App/Method3");
            NotifyFlush.Notify(new[] { logger });

            Assert.AreEqual(4, flushLogArgs.Count);
        }

        [TestMethod]
        public void LogMessagesAreFilteredByInterceptorShouldLog()
        {
            CommonTestHelpers.ResetContext();

            List<FlushLogArgs> args = new List<FlushLogArgs>();

            KissLogConfiguration.Listeners.Add(new CustomLogListener(onFlush: (FlushLogArgs arg) => args.Add(arg))
            {
                Interceptor = new CustomLogListenerInterceptor
                {
                    ShouldLogMessage = (LogMessage message) =>
                    {
                        return message.LogLevel >= LogLevel.Warning;
                    }
                }
            });

            Logger logger = new Logger();
            logger.Trace("Trace");
            logger.Debug("Debug");
            logger.Info("Info");
            logger.Warn("Warn");
            logger.Error("Error");
            logger.Critical("Critical");

            NotifyFlush.Notify(new[] { logger });

            Assert.AreEqual(3, args[0].MessagesGroups.ElementAt(0).Messages.Count());
        }

        [TestMethod]
        public void OptionsAreEvaluated()
        {
            CommonTestHelpers.ResetContext();

            FlushLogArgs flushLogArgs = null;

            KissLogConfiguration.Options.ShouldLogRequestHeader((OptionsArgs.LogListenerHeaderArgs args) => false);
            KissLogConfiguration.Options.ShouldLogRequestCookie((OptionsArgs.LogListenerCookieArgs args) => false);
            KissLogConfiguration.Options.ShouldLogFormData((OptionsArgs.LogListenerFormDataArgs args) => false);
            KissLogConfiguration.Options.ShouldLogServerVariable((OptionsArgs.LogListenerServerVariableArgs args) => false);
            KissLogConfiguration.Options.ShouldLogClaim((OptionsArgs.LogListenerClaimArgs args) => false);
            KissLogConfiguration.Options.ShouldLogInputStream((OptionsArgs.LogListenerInputStreamArgs args) => false);
            KissLogConfiguration.Options.ShouldLogResponseHeader((OptionsArgs.LogListenerHeaderArgs args) => false);

            KissLogConfiguration.Listeners.Add(new CustomLogListener(onFlush: (FlushLogArgs arg) => flushLogArgs = arg));

            Logger logger = new Logger();
            logger.DataContainer.SetHttpProperties(CommonTestHelpers.Factory.CreateHttpProperties());

            NotifyFlush.Notify(new[] { logger });

            Assert.AreEqual(0, flushLogArgs.HttpProperties.Request.Properties.Headers.Count());
            Assert.AreEqual(0, flushLogArgs.HttpProperties.Request.Properties.Cookies.Count());
            Assert.AreEqual(0, flushLogArgs.HttpProperties.Request.Properties.FormData.Count());
            Assert.AreEqual(0, flushLogArgs.HttpProperties.Request.Properties.ServerVariables.Count());
            Assert.AreEqual(0, flushLogArgs.HttpProperties.Request.Properties.Claims.Count());
            Assert.IsNull(flushLogArgs.HttpProperties.Request.Properties.InputStream);

            Assert.AreEqual(0, flushLogArgs.HttpProperties.Response.Properties.Headers.Count());
        }
    }
}
