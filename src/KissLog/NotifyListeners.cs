using KissLog.Internal;
using KissLog.Web;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace KissLog
{
    class ArgsResult
    {
        public FlushLogArgs Args { get; set; }
        public List<LoggerFile> Files { get; set; } = new List<LoggerFile>();
    }

    internal static class NotifyListeners
    {
        public static void Notify(ILogger[] loggers)
        {
            if (loggers == null || !loggers.Any())
                return;

            if (KissLogConfiguration.Listeners == null || KissLogConfiguration.Listeners.Any() == false)
                return;

            Logger[] theLoggers = loggers.OfType<Logger>().ToArray();

            if (!theLoggers.Any())
                return;

            Logger defaultLogger = theLoggers.FirstOrDefault(p => p.CategoryName == Logger.DefaultCategoryName) ?? theLoggers.First();

            ArgsResult argsResult = CreateArgs(theLoggers);

            FlushLogArgs defaultArgs = argsResult.Args;
            string defaultArgsJsonJson = JsonConvert.SerializeObject(defaultArgs);

            foreach (ILogListener listener in KissLogConfiguration.Listeners)
            {
                FlushLogArgs args = CreateFlushArgsForListener(defaultLogger, listener, defaultArgs, defaultArgsJsonJson);
                args.Files = argsResult.Files.ToList();

                if(ShouldUseListener(listener, args) == false)
                    continue;

                listener.Parser?.BeforeFlush(args, listener);

                listener.OnFlush(args);
            }

            foreach (Logger logger in theLoggers)
            {
                logger.Reset();
            }
        }

        public static ArgsResult CreateArgs(ILogger[] loggers)
        {
            if (loggers == null || !loggers.Any())
                return null;

            Logger[] theLoggers = loggers.OfType<Logger>().ToArray();

            if (!theLoggers.Any())
                return null;

            return CreateArgs(theLoggers);
        }

        private static ArgsResult CreateArgs(Logger[] loggers)
        {
            Logger defaultLogger = loggers.FirstOrDefault(p => p.CategoryName == Logger.DefaultCategoryName) ?? loggers.First();

            LoggerDataContainer dataContainer = defaultLogger.DataContainer;

            WebRequestProperties webRequestProperties = dataContainer.WebRequestProperties;
            string errorMessage = dataContainer.Exceptions.LastOrDefault()?.ExceptionMessage;
            List<LogMessagesGroup> logMessages = new List<LogMessagesGroup>();
            List<CapturedException> exceptions = new List<CapturedException>();

            foreach (Logger logger in loggers)
            {
                logMessages.Add(new LogMessagesGroup
                {
                    CategoryName = logger.CategoryName,
                    Messages = dataContainer.LogMessages.ToList()
                });

                exceptions.AddRange(dataContainer.Exceptions);
            }

            exceptions = exceptions.Distinct(new CapturedExceptionComparer()).ToList();

            List<LoggerFile> files = dataContainer.LoggerFiles.GetFiles().ToList();
            FlushLogArgs args = new FlushLogArgs
            {
                IsCreatedByHttpRequest = defaultLogger.IsCreatedByHttpRequest(),
                ErrorMessage = errorMessage,
                WebRequestProperties = webRequestProperties,
                MessagesGroups = logMessages,
                CapturedExceptions = exceptions
            };

            return new ArgsResult
            {
                Args = args,
                Files = files
            };
        }

        private static FlushLogArgs CreateFlushArgsForListener(Logger defaultLogger, ILogListener listener, FlushLogArgs defaultArgs, string defaultArgsJson)
        {
            FlushLogArgs args = JsonConvert.DeserializeObject<FlushLogArgs>(defaultArgsJson);

            string inputStream = null;
            if (!string.IsNullOrEmpty(defaultArgs.WebRequestProperties.Request.InputStream))
            {
                if(KissLogConfiguration.Options.ApplyShouldLogRequestInputStream(defaultLogger, listener, defaultArgs))
                {
                    inputStream = defaultArgs.WebRequestProperties.Request.InputStream;
                }
            }

            args.WebRequestProperties.Request.Headers = defaultArgs.WebRequestProperties.Request.Headers.Where(p => KissLogConfiguration.Options.ApplyShouldLogRequestHeader(listener, defaultArgs, p.Key)).ToList();
            args.WebRequestProperties.Request.Cookies = defaultArgs.WebRequestProperties.Request.Cookies.Where(p => KissLogConfiguration.Options.ApplyShouldLogRequestCookie(listener, defaultArgs, p.Key)).ToList();
            args.WebRequestProperties.Request.QueryString = defaultArgs.WebRequestProperties.Request.QueryString.Where(p => KissLogConfiguration.Options.ApplyShouldLogRequestQueryString(listener, defaultArgs, p.Key)).ToList();
            args.WebRequestProperties.Request.FormData = defaultArgs.WebRequestProperties.Request.FormData.Where(p => KissLogConfiguration.Options.ApplyShouldLogRequestFormData(listener, defaultArgs, p.Key)).ToList();
            args.WebRequestProperties.Request.ServerVariables = defaultArgs.WebRequestProperties.Request.ServerVariables.Where(p => KissLogConfiguration.Options.ApplyShouldLogRequestServerVariable(listener, defaultArgs, p.Key)).ToList();
            args.WebRequestProperties.Request.Claims = defaultArgs.WebRequestProperties.Request.Claims.Where(p => KissLogConfiguration.Options.ApplyShouldLogRequestClaim(listener, defaultArgs, p.Key)).ToList();
            args.WebRequestProperties.Request.InputStream = inputStream;

            args.WebRequestProperties.Response.Headers = defaultArgs.WebRequestProperties.Response.Headers.Where(p => KissLogConfiguration.Options.ApplyShouldLogResponseHeader(listener, defaultArgs, p.Key)).ToList();

            List<LogMessagesGroup> messages = new List<LogMessagesGroup>();
            foreach (var group in defaultArgs.MessagesGroups)
            {
                messages.Add(new LogMessagesGroup
                {
                    CategoryName = group.CategoryName,
                    Messages = group.Messages.Where(p => listener.Parser.ShouldLog(p, listener)).ToList()
                });
            }

            args.MessagesGroups = messages;

            return args;
        }

        private static bool ShouldUseListener(ILogListener listener, FlushLogArgs args)
        {
            if (KissLogConfiguration.Options.ApplyToggleListener(listener, args) == false)
                return false;

            LogListenerParser parser = listener.Parser;
            if (parser == null)
                return true;

            return parser.ShouldLog(args, listener);
        }
    }
}
