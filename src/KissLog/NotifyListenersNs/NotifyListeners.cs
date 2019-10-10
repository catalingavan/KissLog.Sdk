using KissLog.Internal;
using KissLog.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog.NotifyListenersNs
{
    public static class NotifyListeners
    {
        public static void NotifyBeginRequest(ILogger logger)
        {
            if (KissLogConfiguration.Listeners == null || KissLogConfiguration.Listeners.Any() == false)
                return;

            if (logger is Logger == false)
                return;

            Logger defaultLogger = logger as Logger;
            WebRequestProperties webRequestProperties = defaultLogger.DataContainer.WebRequestProperties;

            foreach (ILogListener listener in KissLogConfiguration.Listeners)
            {
                ILogListenerNew listner2 = null;

                listner2.OnBeginRequest(webRequestProperties, logger);
            }
        }

        public static void NotifyMessage(LogMessage message, ILogger logger)
        {
            foreach (ILogListener listener in KissLogConfiguration.Listeners)
            {
                if (listener.Parser.ShouldLog(message, listener) == false)
                    continue;

                listener.OnMessage(message, logger);
            }
        }

        public static void NotifyFlush(ILogger logger)
        {
            if (KissLogConfiguration.Listeners == null || KissLogConfiguration.Listeners.Any() == false)
                return;

            Logger defaultLogger = logger as Logger;

            ArgsResult argsResult = CreateArgs(new[] { defaultLogger });

            FlushLogArgs defaultArgs = argsResult.Args;
            List<LoggerFile> defaultFiles = argsResult.Files.ToList();

            string defaultArgsJsonJson = JsonConvert.SerializeObject(defaultArgs);

            foreach (ILogListener listener in KissLogConfiguration.Listeners)
            {
                FlushLogArgs args = CreateFlushArgsForListener(defaultLogger, listener, defaultArgs, defaultArgsJsonJson, defaultFiles.ToList());

                //if (ShouldUseListener(listener, args) == false)
                //    continue;

                listener.Parser?.BeforeFlush(args, listener);

                listener.OnFlush(args, logger);
            }
        }

        private static ArgsResult CreateArgs(Logger[] loggers)
        {
            Logger defaultLogger = loggers.FirstOrDefault(p => p.CategoryName == Logger.DefaultCategoryName) ?? loggers.First();

            LoggerDataContainer dataContainer = defaultLogger.DataContainer;

            WebRequestProperties webRequestProperties = dataContainer.WebRequestProperties;
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

            if (defaultLogger.IsCreatedByHttpRequest() == false && exceptions.Any())
            {
                webRequestProperties.Response.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            if (!webRequestProperties.EndDateTime.HasValue)
            {
                webRequestProperties.EndDateTime = DateTime.UtcNow;
            }

            List<LoggerFile> files = dataContainer.LoggerFiles.GetFiles().ToList();
            FlushLogArgs args = new FlushLogArgs
            {
                IsCreatedByHttpRequest = defaultLogger.IsCreatedByHttpRequest(),
                WebRequestProperties = webRequestProperties,
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
            if (!string.IsNullOrEmpty(defaultArgs.WebRequestProperties.Request.InputStream))
            {
                if (KissLogConfiguration.Options.ApplyShouldLogRequestInputStream(defaultLogger, listener, defaultArgs))
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

            List<LoggerFile> files = defaultFiles.ToList();
            LoggerFile responseFile = GetResponseFile(files);

            if (responseFile != null && !InternalHelpers.ShouldLogResponseBody(defaultLogger, listener, defaultArgs))
            {
                files.Remove(responseFile);
            }

            args.Files = files;

            return args;
        }

        private static LoggerFile GetResponseFile(List<LoggerFile> files)
        {
            if (files == null || !files.Any())
                return null;

            return files.FirstOrDefault(p => string.Compare(p.FileName, "Response", StringComparison.OrdinalIgnoreCase) == 0);
        }

        private static IEnumerable<KeyValuePair<string, object>> GetCustomProperties(Logger logger)
        {
            return logger.DataContainer.GetProperties().Where(p => p.Key.ToLowerInvariant().StartsWith("x-kisslog-") == false);
        }
    }
}
