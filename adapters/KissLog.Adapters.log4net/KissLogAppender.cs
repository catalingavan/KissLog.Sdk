using log4net.Appender;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog.Adapters.log4net
{
    public class KissLogAppender : AppenderSkeleton
    {
        private static readonly Dictionary<LogLevel, string[]> LevelsMapping = new Dictionary<LogLevel, string[]>
        {
            { LogLevel.Trace, new [] { "Trace" } },
            { LogLevel.Debug, new [] { "Debug" } },
            { LogLevel.Information, new [] { "Info", "Information" } },
            { LogLevel.Warning, new [] { "Warn", "Warning" } },
            { LogLevel.Error, new [] { "Error", "Severe" } },
            { LogLevel.Critical, new [] { "Critical", "Fatal" } }
        };

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
                return;

            LogLevel logLevel = GetLogLevel(loggingEvent.Level.Name);
            string message = RenderLoggingEvent(loggingEvent);

            IKLogger logger = Logger.Factory.Get();

            if (!string.IsNullOrEmpty(loggingEvent.LocationInformation?.ClassName))
            {
                int lineNumber = 0;
                int.TryParse(loggingEvent.LocationInformation?.LineNumber, out lineNumber);

                logger.Log(logLevel, message, loggingEvent.LocationInformation.MethodName, lineNumber, loggingEvent.LocationInformation.ClassName);
            }
            else
            {
                logger.Log(logLevel, message, string.Empty, 0, string.Empty);
            }
        }

        internal LogLevel GetLogLevel(string logLevelName)
        {
            if (string.IsNullOrWhiteSpace(logLevelName))
                return LogLevel.Trace;

            logLevelName = logLevelName.Trim();

            foreach (var mapping in LevelsMapping)
            {
                if (mapping.Value.Any(p => string.Compare(p, logLevelName, StringComparison.OrdinalIgnoreCase) == 0))
                    return mapping.Key;
            }

            return LogLevel.Trace;
        }
    }
}
