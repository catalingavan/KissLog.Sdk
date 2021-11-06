using KissLog.Http;
using KissLog.NotifyListeners;
using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace KissLog.Tests.NotifyListeners
{
    [TestClass]
    public class NotifyOnMessageTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenLogMessageIsNull()
        {
            NotifyOnMessage.Notify(null);
        }

        [TestMethod]
        public void NotifyIsInvokedForEachLogListener()
        {
            CommonTestHelpers.ResetContext();

            List<LogMessage> messageArgs = new List<LogMessage>();

            KissLogConfiguration.Listeners.Add(new CustomLogListener(onMessage: (LogMessage arg) => { messageArgs.Add(arg); }));
            KissLogConfiguration.Listeners.Add(new CustomLogListener(onMessage: (LogMessage arg) => { messageArgs.Add(arg); }));
            KissLogConfiguration.Listeners.Add(new CustomLogListener(onMessage: (LogMessage arg) => { messageArgs.Add(arg); }));

            LogMessage message = new LogMessage(new LogMessage.CreateOptions
            {
                CategoryName = Constants.DefaultLoggerCategoryName,
                LogLevel = LogLevel.Trace,
                Message = "Message"
            });
            NotifyOnMessage.Notify(message);

            Assert.AreEqual(3, messageArgs.Count);
        }

        [TestMethod]
        public void NotifyContinuesForOtherListenersWhenOneThrowsAnException()
        {
            CommonTestHelpers.ResetContext();

            List<LogMessage> messageArgs = new List<LogMessage>();

            KissLogConfiguration.Listeners.Add(new CustomLogListener(onMessage: (LogMessage arg) => { throw new Exception(); }));
            KissLogConfiguration.Listeners.Add(new CustomLogListener(onMessage: (LogMessage arg) => { messageArgs.Add(arg); }));
            KissLogConfiguration.Listeners.Add(new CustomLogListener(onMessage: (LogMessage arg) => { messageArgs.Add(arg); }));

            LogMessage message = new LogMessage(new LogMessage.CreateOptions
            {
                CategoryName = Constants.DefaultLoggerCategoryName,
                LogLevel = LogLevel.Trace,
                Message = "Message"
            });
            NotifyOnMessage.Notify(message);

            Assert.AreEqual(2, messageArgs.Count);
        }

        [TestMethod]
        [DataRow(LogLevel.Trace, false)]
        [DataRow(LogLevel.Debug, false)]
        [DataRow(LogLevel.Information, false)]
        [DataRow(LogLevel.Warning, true)]
        [DataRow(LogLevel.Error, true)]
        [DataRow(LogLevel.Critical, true)]
        public void LogListenerInterceptorShouldLogIsEvaluated(LogLevel logLevel, bool expectedResult)
        {
            CommonTestHelpers.ResetContext();

            List<LogMessage> messageArgs = new List<LogMessage>();

            ILogListener listener = new CustomLogListener(onMessage: (LogMessage message) => { messageArgs.Add(message); })
            {
                Interceptor = new CustomLogListenerInterceptor
                {
                    ShouldLogMessage = (LogMessage message) =>
                    {
                        return message.LogLevel >= LogLevel.Warning;
                    }
                }
            };
            KissLogConfiguration.Listeners.Add(listener);

            LogMessage message = new LogMessage(new LogMessage.CreateOptions
            {
                CategoryName = Constants.DefaultLoggerCategoryName,
                LogLevel = logLevel,
                Message = "Message"
            });
            NotifyOnMessage.Notify(message);

            Assert.AreEqual(expectedResult, messageArgs.Count == 1);
        }

        [TestMethod]
        public void NotifyShouldNotBeInvokedIfInterceptorShouldLogBeginRequestReturnsFalse()
        {
            CommonTestHelpers.ResetContext();

            List<LogMessage> messageArgs = new List<LogMessage>();

            ILogListener listener1 = new CustomLogListener(onMessage: (LogMessage message) => { messageArgs.Add(message); })
            {
                Interceptor = new CustomLogListenerInterceptor
                {
                    ShouldLogBeginRequest = (HttpRequest httpRequest) => httpRequest.Url.LocalPath == "/App/Method2"
                }
            };
            KissLogConfiguration.Listeners.Add(listener1);

            ILogListener listener2 = new CustomLogListener(onMessage: (LogMessage message) => { messageArgs.Add(message); });
            KissLogConfiguration.Listeners.Add(listener2);

            Logger logger = new Logger(url: "/App/Method1");
            logger.Trace("Message 1");
            logger.Trace("Message 2");

            logger = new Logger(url: "/App/Method2");
            logger.Trace("Message 1");
            logger.Trace("Message 2");

            Assert.AreEqual(6, messageArgs.Count);
        }
    }
}
