using KissLog.Internal;
using KissLog.Web;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace KissLog
{
    internal static class NotifyListenersV2
    {
        public static void Notify(ILogger[] loggers)
        {
            if (loggers == null || !loggers.Any())
                return;

            if (KissLogConfiguration.Listeners == null || KissLogConfiguration.Listeners.Any() == false)
                return;

            Logger[] theLoggers = loggers.Where(p => p is Logger).Select(p => p as Logger).ToArray();

            if (theLoggers == null || !theLoggers.Any())
                return;

            FlushLogArgs defaultArgs = CreateFlushArgs(theLoggers);

            foreach (ILogListener listener in KissLogConfiguration.Listeners)
            {

            }
        }

        private static FlushLogArgs CreateFlushArgs(Logger[] loggers)
        {
            Logger defaultLogger = loggers.FirstOrDefault(p => p.CategoryName == Logger.DefaultCategoryName) ?? loggers.First();

            WebRequestProperties webRequestProperties = defaultLogger.WebRequestProperties;
            string errorMessage = defaultLogger.CapturedExceptions.LastOrDefault()?.ExceptionMessage;
            List<LogMessagesGroup> logMessages = new List<LogMessagesGroup>();
            List<CapturedException> exceptions = new List<CapturedException>();

            foreach (Logger logger in loggers)
            {
                logMessages.Add(new LogMessagesGroup
                {
                    CategoryName = logger.CategoryName,
                    Messages = logger.LogMessages.ToList()
                });

                exceptions.AddRange(logger.CapturedExceptions);

                exceptions = exceptions.Distinct(new CapturedExceptionComparer()).ToList();
            }

            FlushLogArgs args = new FlushLogArgs
            {
                IsCreatedByHttpRequest = defaultLogger.IsCreatedByHttpRequest(),
                ErrorMessage = errorMessage,
                WebRequestProperties = webRequestProperties,
                MessagesGroups = logMessages,
                CapturedExceptions = exceptions
            };

            return args;
        }

        private static FlushLogArgs CreateFlushArgsForListener(ILogListener listener, FlushLogArgs defaultArgs)
        {
            FlushLogArgs args = JsonConvert.DeserializeObject<FlushLogArgs>(JsonConvert.SerializeObject(defaultArgs));

            var requestHeaders = defaultArgs.WebRequestProperties.Request.Headers.Where(p => KissLogConfiguration.Options.ApplyShouldLogRequestHeader(defaultArgs.WebRequestProperties, p.Key)).ToList();
            var requestCookies = defaultArgs.WebRequestProperties.Request.Cookies.Where(p => KissLogConfiguration.Options.ApplyShouldLogRequestCookie(defaultArgs.WebRequestProperties, p.Key)).ToList();
            string requestInputStream = null;

            if(!string.IsNullOrEmpty(defaultArgs.WebRequestProperties.Request.InputStream) && KissLogConfiguration.Options.ApplyShouldLogRequestInputStream(null, defaultArgs.WebRequestProperties))
            {
                requestInputStream = defaultArgs.WebRequestProperties.Request.InputStream;
            }

            args.WebRequestProperties.Request.Headers = requestHeaders;
            args.WebRequestProperties.Request.Cookies = requestCookies;
            args.WebRequestProperties.Request.InputStream = requestInputStream;

            return args;
        }

        private static bool ShouldUseListener(ILogListener listener, FlushLogArgs args)
        {
            if (args.IsCreatedByHttpRequest == false)
                return true;

            int statusCode = (int)args.WebRequestProperties.Response.HttpStatusCode;
            if (statusCode < listener.MinimumResponseHttpStatusCode)
                return false;

            LogListenerParser parser = listener.Parser;
            if (parser == null)
                return true;
        }
    }
}
