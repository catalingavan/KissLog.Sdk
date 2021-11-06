using KissLog.Formatters;
using KissLog.Http;
using KissLog.Json;
using KissLog.LoggerData;
using KissLog.NotifyListeners;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog
{
    public class Logger : IKLogger
    {
        internal Guid Id { get; }
        public LoggerDataContainer DataContainer { get; private set; }
        public string CategoryName { get; }

        public Logger(string categoryName = null, string url = null)
        {
            Id = Guid.NewGuid();
            CategoryName = string.IsNullOrWhiteSpace(categoryName) ? Constants.DefaultLoggerCategoryName : categoryName;
            DataContainer = new LoggerDataContainer(this);

            HttpProperties httpProperties = string.IsNullOrWhiteSpace(url) ? null : HttpPropertiesFactory.Create(url);

            if(httpProperties != null)
            {
                DataContainer.SetHttpProperties(httpProperties);

                InternalHelpers.WrapInTryCatch(() =>
                {
                    NotifyBeginRequest.Notify(httpProperties.Request);
                });
            }
        }

        public void Log(LogLevel logLevel, string message, string memberName = null, int lineNumber = 0, string memberType = null)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            LogMessage logMessage = new LogMessage(new LogMessage.CreateOptions
            {
                CategoryName = CategoryName,
                LogLevel = logLevel,
                Message = message,
                MemberType = memberType,
                MemberName = memberName,
                LineNumber = lineNumber
            });

            DataContainer.Add(logMessage);

            Guid? httpRequestId = DataContainer.HttpProperties == null ? (Guid?)null : DataContainer.HttpProperties.Request.Id;

            InternalHelpers.WrapInTryCatch(() =>
            {
                NotifyOnMessage.Notify(logMessage, httpRequestId);
            });
        }

        public void Log(LogLevel logLevel, object json, JsonSerializeOptions options = null, string memberName = null, int lineNumber = 0, string memberType = null)
        {
            string message = KissLogConfiguration.JsonSerializer.Serialize(json, options);
            Log(logLevel, message, memberName, lineNumber, memberType);
        }

        public void Log(LogLevel logLevel, Exception ex, string memberName = null, int lineNumber = 0, string memberType = null)
        {
            if (ex == null)
                return;

            var formatter = new ExceptionFormatter();
            string message = formatter.Format(ex, this);

            Log(logLevel, message, memberName, lineNumber, memberType);
        }

        public void Log(LogLevel logLevel, Args args, JsonSerializeOptions options = null, string memberName = null, int lineNumber = 0, string memberType = null)
        {
            if (args == null)
                return;

            List<string> values = new List<string>();

            foreach (var arg in args.GetArgs())
            {
                string message = null;

                if (arg is string stringArg)
                {
                    message = stringArg;
                }
                else if (arg is Exception exceptionArg)
                {
                    var formatter = new ExceptionFormatter();
                    message = formatter.Format(exceptionArg, this);
                }
                else
                {
                    message = KissLogConfiguration.JsonSerializer.Serialize(arg, options);
                }

                if (string.IsNullOrEmpty(message))
                    continue;

                values.Add(message);
            }

            string value = string.Join(Environment.NewLine, values);

            Log(logLevel, value, memberName, lineNumber, memberType);
        }

        internal void Reset()
        {
            HttpProperties httpProperties = DataContainer.HttpProperties;

            DataContainer.Dispose();
            DataContainer = new LoggerDataContainer(this);

            if (httpProperties != null)
                DataContainer.SetHttpProperties(httpProperties);
        }

        public static void NotifyListeners(Logger logger)
        {
            if (logger == null)
                return;

            InternalHelpers.WrapInTryCatch(() =>
            {
                NotifyFlush.Notify(new[] { logger });
            });
        }

        public static void NotifyListeners(IEnumerable<Logger> loggers)
        {
            if (loggers == null || !loggers.Any())
                return;

            InternalHelpers.WrapInTryCatch(() =>
            {
                NotifyFlush.Notify(loggers.ToArray());
            });
        }

        public static ILoggerFactory Factory { get; internal set; } = new LoggerFactory();

        public static void SetFactory(ILoggerFactory factory)
        {
            Factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }
    }
}
