using KissLog.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace KissLog
{
    public static class FlushLogArgsFactory
    {
        public static FlushLogArgs Create(Logger[] loggers)
        {
            if (loggers == null || !loggers.Any())
                throw new ArgumentNullException();

            Logger defaultLogger = loggers.FirstOrDefault(p => p.CategoryName == Constants.DefaultLoggerCategoryName) ?? loggers.First();

            var options = new FlushLogArgs.CreateOptions
            {
                MessagesGroups = GetLogMessages(loggers),
                Exceptions = GetExceptions(loggers),
                Files = GetFiles(loggers),
                CustomProperties = GetCustomProperties(loggers),
                IsCreatedByHttpRequest = defaultLogger.DataContainer.LoggerProperties.IsManagedByHttpRequest
            };
            
            HttpProperties httpProperties = defaultLogger.DataContainer.HttpProperties;
            if(httpProperties == null)
            {
                httpProperties = new HttpProperties(new HttpRequest(new HttpRequest.CreateOptions
                {
                    HttpMethod = "GET",
                    Url = UrlParser.GenerateUri(null),
                    StartDateTime = defaultLogger.DataContainer.DateTimeCreated
                }));
            }

            if(httpProperties.Response == null)
            {
                int statusCode = options.Exceptions.Any() ? (int)HttpStatusCode.InternalServerError : (int)HttpStatusCode.OK;
                httpProperties.SetResponse(new HttpResponse(new HttpResponse.CreateOptions
                {
                    StatusCode = statusCode
                }));
            }

            int? explicitStatusCode = GetExplicitStatusCode(loggers);
            if(explicitStatusCode.HasValue)
            {
                httpProperties.Response.SetStatusCode(explicitStatusCode.Value);
            }

            options.HttpProperties = httpProperties;

            return new FlushLogArgs(options);
        }

        private static List<LogMessagesGroup> GetLogMessages(Logger[] loggers)
        {
            List<LogMessagesGroup> result = new List<LogMessagesGroup>();

            foreach(Logger logger in loggers)
            {
                if (!logger.DataContainer.LogMessages.Any())
                    continue;

                List<LogMessage> messages = logger.DataContainer.LogMessages.ToList();
                result.Add(new LogMessagesGroup(logger.CategoryName, messages));
            }

            return result;
        }

        private static List<CapturedException> GetExceptions(Logger[] loggers)
        {
            List<CapturedException> result = new List<CapturedException>();

            foreach(Logger logger in loggers)
            {
                var exceptions = logger.DataContainer.Exceptions.Select(p => new CapturedException(p)).ToList();
                result.AddRange(exceptions);
            }

            return result;
        }

        private static List<LoggedFile> GetFiles(Logger[] loggers)
        {
            List<LoggedFile> result = new List<LoggedFile>();

            foreach (Logger logger in loggers)
            {
                var files = logger.DataContainer.FilesContainer.GetLoggedFiles();
                result.AddRange(files);
            }

            return result;
        }

        private static List<KeyValuePair<string, object>> GetCustomProperties(Logger[] loggers)
        {
            List<KeyValuePair<string, object>> result = new List<KeyValuePair<string, object>>();

            foreach(Logger logger in loggers)
            {
                result.AddRange(logger.DataContainer.LoggerProperties.CustomProperties);
            }

            return result;
        }

        internal static int? GetExplicitStatusCode(IEnumerable<Logger> loggers)
        {
            if (loggers == null)
                throw new ArgumentNullException(nameof(loggers));

            Logger defaultLogger = loggers.FirstOrDefault(p => p.CategoryName == Constants.DefaultLoggerCategoryName);

            int? result = defaultLogger?.DataContainer.LoggerProperties.ExplicitStatusCode;
            if (result.HasValue)
                return result.Value;

            result = loggers.FirstOrDefault(p => p.DataContainer.LoggerProperties.ExplicitStatusCode.HasValue == true)?.DataContainer.LoggerProperties.ExplicitStatusCode;

            return result;
        }
    }
}
