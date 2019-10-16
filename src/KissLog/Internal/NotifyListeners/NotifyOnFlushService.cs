using KissLog.FlushArgs;
using KissLog.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog.Internal
{
    internal static class NotifyOnFlushService
    {
        public static void Notify(ILogger[] loggers)
        {
            if (loggers == null || !loggers.Any())
                return;

            if (KissLogConfiguration.Listeners.Get().Any() == false)
                return;

            Logger[] theLoggers = loggers.OfType<Logger>().ToArray();

            if (!theLoggers.Any())
                return;

            Logger defaultLogger = theLoggers.FirstOrDefault(p => p.CategoryName == Logger.DefaultCategoryName) ?? theLoggers.First();

            ArgsResult argsResult = CreateArgs(theLoggers);

            FlushLogArgs defaultArgs = argsResult.Args;
            List<LoggerFile> defaultFiles = argsResult.Files.ToList();

            string defaultArgsJsonJson = JsonConvert.SerializeObject(defaultArgs);

            foreach (LogListenerDecorator decorator in KissLogConfiguration.Listeners.Get())
            {
                ILogListener listener = decorator.Listener;

                if (decorator.ShouldSkipOnFlush(defaultArgs.WebProperties.Request))
                    continue;

                FlushLogArgs args = CreateFlushArgsForListener(defaultLogger, listener, defaultArgs, defaultArgsJsonJson, defaultFiles.ToList());

                if (ShouldUseListener(listener, args) == false)
                    continue;

                if(listener.Parser != null)
                    listener.Parser.BeforeFlush(args, listener);

                listener.OnFlush(args, defaultLogger);
            }

            foreach (Logger logger in theLoggers)
            {
                logger.Reset();
            }

            foreach (LogListenerDecorator decorator in KissLogConfiguration.Listeners.Get())
            {
                decorator.Reset();
            }
        }

        internal static ArgsResult CreateArgs(ILogger[] loggers)
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

            WebProperties webProperties = dataContainer.WebProperties;
            string errorMessage = dataContainer.Exceptions.LastOrDefault()?.ExceptionMessage;
            List<LogMessagesGroup> logMessages = new List<LogMessagesGroup>();
            List<CapturedException> exceptions = new List<CapturedException>();
            List<KeyValuePair<string, object>> customProperties = new List<KeyValuePair<string, object>>();

            foreach (Logger logger in loggers)
            {
                logMessages.Add(new LogMessagesGroup
                {
                    CategoryName = logger.CategoryName,
                    Messages = logger.DataContainer.LogMessages.ToList()
                });

                exceptions.AddRange(logger.DataContainer.Exceptions);

                var properties = GetCustomProperties(logger);
                customProperties.AddRange(properties);
            }

            exceptions = exceptions.Distinct(new CapturedExceptionComparer()).ToList();

            if(defaultLogger.IsCreatedByHttpRequest() == false && exceptions.Any())
            {
                webProperties.Response.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            webProperties.Response.EndDateTime = DateTime.UtcNow;

            List<LoggerFile> files = dataContainer.LoggerFiles.GetFiles().ToList();
            FlushLogArgs args = new FlushLogArgs
            {
                IsCreatedByHttpRequest = defaultLogger.IsCreatedByHttpRequest(),
                WebProperties = webProperties,
                MessagesGroups = logMessages,
                CapturedExceptions = exceptions,
                CustomProperties = customProperties
            };

            args.Files = files;

            return new ArgsResult
            {
                Args = args,
                Files = files
            };
        }

        private static FlushLogArgs CreateFlushArgsForListener(Logger defaultLogger, ILogListener listener, FlushLogArgs defaultArgs, string defaultArgsJson, List<LoggerFile> defaultFiles)
        {
            FlushLogArgs args = JsonConvert.DeserializeObject<FlushLogArgs>(defaultArgsJson);

            string inputStream = null;
            if (!string.IsNullOrEmpty(defaultArgs.WebProperties.Request.Properties.InputStream))
            {
                if (KissLogConfiguration.Options.ApplyShouldLogRequestInputStream(defaultLogger, listener, defaultArgs))
                {
                    inputStream = defaultArgs.WebProperties.Request.Properties.InputStream;
                }
            }

            args.WebProperties.Request.Properties.Headers = defaultArgs.WebProperties.Request.Properties.Headers.Where(p => KissLogConfiguration.Options.ApplyShouldLogRequestHeader(listener, defaultArgs, p.Key)).ToList();
            args.WebProperties.Request.Properties.Cookies = defaultArgs.WebProperties.Request.Properties.Cookies.Where(p => KissLogConfiguration.Options.ApplyShouldLogRequestCookie(listener, defaultArgs, p.Key)).ToList();
            args.WebProperties.Request.Properties.QueryString = defaultArgs.WebProperties.Request.Properties.QueryString.Where(p => KissLogConfiguration.Options.ApplyShouldLogRequestQueryString(listener, defaultArgs, p.Key)).ToList();
            args.WebProperties.Request.Properties.FormData = defaultArgs.WebProperties.Request.Properties.FormData.Where(p => KissLogConfiguration.Options.ApplyShouldLogRequestFormData(listener, defaultArgs, p.Key)).ToList();
            args.WebProperties.Request.Properties.ServerVariables = defaultArgs.WebProperties.Request.Properties.ServerVariables.Where(p => KissLogConfiguration.Options.ApplyShouldLogRequestServerVariable(listener, defaultArgs, p.Key)).ToList();
            args.WebProperties.Request.Properties.Claims = defaultArgs.WebProperties.Request.Properties.Claims.Where(p => KissLogConfiguration.Options.ApplyShouldLogRequestClaim(listener, defaultArgs, p.Key)).ToList();
            args.WebProperties.Request.Properties.InputStream = inputStream;

            args.WebProperties.Response.Properties.Headers = defaultArgs.WebProperties.Response.Properties.Headers.Where(p => KissLogConfiguration.Options.ApplyShouldLogResponseHeader(listener, defaultArgs, p.Key)).ToList();

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

            List<LoggerFile> files = defaultFiles.ToList();
            LoggerFile responseFile = GetResponseFile(files);

            if (responseFile != null && !InternalHelpers.ShouldLogResponseBody(defaultLogger, listener, defaultArgs))
            {
                files.Remove(responseFile);
            }

            args.Files = files;

            return args;
        }

        private static IEnumerable<KeyValuePair<string, object>> GetCustomProperties(Logger logger)
        {
            return logger.DataContainer.GetProperties().Where(p => p.Key.ToLowerInvariant().StartsWith("x-kisslog-") == false);
        }

        private static LoggerFile GetResponseFile(List<LoggerFile> files)
        {
            if (files == null || !files.Any())
                return null;

            return files.FirstOrDefault(p => string.Compare(p.FileName, "Response", StringComparison.OrdinalIgnoreCase) == 0);
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
