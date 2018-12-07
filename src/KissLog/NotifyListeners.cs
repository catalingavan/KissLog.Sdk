using KissLog.Web;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace KissLog
{
    internal static class NotifyListeners
    {
        public static void Notify(ILogger[] loggers)
        {
            if (loggers == null || !loggers.Any())
                return;

            if (KissLogConfiguration.Listeners == null || KissLogConfiguration.Listeners.Any() == false)
                return;

            ILogger defaultLogger = loggers.FirstOrDefault(p => p.CategoryName == Logger.DefaultCategoryName) ?? loggers.First();
            Logger logger = null;

            if ((defaultLogger is Logger) == false)
                return;

            logger = defaultLogger as Logger;

            FlushLogArgs args = CreateFlushLogArgs(logger, loggers);
            List<LoggerFile> files = logger.LoggerFiles.GetFiles().ToList();

            string argsJson = JsonConvert.SerializeObject(args);

            foreach (ILogListener listener in KissLogConfiguration.Listeners)
            {
                // we make a clone of FlushLogArgs, because each ILogListener can alter the parameters
                FlushLogArgs listenerArgs = JsonConvert.DeserializeObject<FlushLogArgs>(argsJson);

                if(ShouldLog(listener, listenerArgs) == false)
                    continue;

                PrepareArgsForListener(listener, listenerArgs);

                listenerArgs.Files = files.ToList();

                listener.OnFlush(listenerArgs);
            }

            foreach (ILogger ilogger in loggers)
            {
                if (ilogger is Logger myLogger)
                {
                    myLogger.Reset();
                }
            }
        }

        private static FlushLogArgs CreateFlushLogArgs(Logger logger, ILogger[] loggers)
        {
            WebRequestProperties webRequestProperties = logger.WebRequestProperties ?? WebRequestPropertiesFactory.CreateDefault();
            IEnumerable<LogMessagesGroup> logMessages = GetLogMessages(loggers);
            IEnumerable<CapturedException> capturedExceptions = GetCapturedExceptions(loggers);

            if (logger.HttpStatusCode.HasValue)
            {
                webRequestProperties.Response.HttpStatusCode = logger.HttpStatusCode.Value;
            }
            else if (webRequestProperties.Response.HttpStatusCode == HttpStatusCode.OK && IsLastMessageError(logger))
            {
                webRequestProperties.Response.HttpStatusCode = HttpStatusCode.InternalServerError;
            }

            FlushLogArgs args = new FlushLogArgs
            {
                IsCreatedByHttpRequest = logger.IsCreatedByHttpRequest(),
                ErrorMessage = GetErrorMessage(logger),
                WebRequestProperties = webRequestProperties,
                MessagesGroups = logMessages,
                CapturedExceptions = capturedExceptions
            };

            return args;
        }

        private static IEnumerable<LogMessagesGroup> GetLogMessages(ILogger[] loggers)
        {
            List<LogMessagesGroup> messages = new List<LogMessagesGroup>();

            foreach (var ilogger in loggers)
            {
                List<LogMessage> logMessages = new List<LogMessage>();

                if (ilogger is Logger logger)
                {
                    logMessages = logger.LogMessages.ToList();
                }

                messages.Add(new LogMessagesGroup
                {
                    CategoryName = ilogger.CategoryName,
                    Messages = logMessages
                });
            }

            return messages;
        }

        private static IEnumerable<CapturedException> GetCapturedExceptions(ILogger[] loggers)
        {
            List<CapturedException> exceptions = new List<CapturedException>();

            foreach (var ilogger in loggers)
            {
                if (ilogger is Logger logger)
                {
                    exceptions.AddRange(logger.CapturedExceptions);
                }
            }

            return exceptions.Distinct(new CapturedExceptionComparer()).ToList();
        }

        private static bool IsLastMessageError(Logger logger)
        {
            LogMessage message = logger.LogMessages.LastOrDefault();

            if (message?.LogLevel >= LogLevel.Error)
                return true;

            return false;
        }

        private static string GetErrorMessage(Logger logger)
        {
            IEnumerable<CapturedException> capturedExceptions = logger.CapturedExceptions;
            return capturedExceptions.LastOrDefault()?.ExceptionMessage;
        }

        private static bool ShouldLog(ILogListener listener, FlushLogArgs args)
        {
            LogListenerParser parser = listener.Parser;
            if (parser == null)
                return true;

            return parser.ShouldLog(args, listener);
        }

        private static IEnumerable<LogMessagesGroup> FilterMessagesForListener(ILogListener listener, IEnumerable<LogMessagesGroup> messageGroups)
        {
            List<LogMessagesGroup> result = new List<LogMessagesGroup>();

            foreach (var group in messageGroups)
            {
                List<LogMessage> filtered = group.Messages.Where(p => listener.Parser.ShouldLog(p, listener)).ToList();

                result.Add(new LogMessagesGroup
                {
                    CategoryName = group.CategoryName,
                    Messages = filtered
                });
            }

            return result;
        }

        private static void PrepareArgsForListener(ILogListener listener, FlushLogArgs args)
        {
            LogListenerParser parser = listener.Parser;
            if (parser == null)
                return;

            parser.AlterDataBeforePersisting(args);

            args.MessagesGroups = FilterMessagesForListener(listener, args.MessagesGroups);
        }
    }
}
