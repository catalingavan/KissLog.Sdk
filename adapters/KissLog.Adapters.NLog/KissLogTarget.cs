using NLog;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog.Adapters.NLog
{
    [Target("KissLog")]
    public class KissLogTarget : TargetWithLayout
    {
        private static readonly Dictionary<LogLevel, string[]> LevelsMapping = new Dictionary<LogLevel, string[]>
        {
            { LogLevel.Trace, new [] { "Trace" } },
            { LogLevel.Debug, new [] { "Debug" } },
            { LogLevel.Information, new [] { "Info", "Information" } },
            { LogLevel.Warning, new [] { "Warn", "Warning" } },
            { LogLevel.Error, new [] { "Error" } },
            { LogLevel.Critical, new [] { "Fatal" } }
        };

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

        protected override void Write(LogEventInfo logEvent)
        {
            if (logEvent == null)
                return;

            LogLevel logLevel = GetLogLevel(logEvent.Level.Name);
            string message = Layout.Render(logEvent);

            IKLogger logger = Logger.Factory.Get();

            if(!string.IsNullOrEmpty(logEvent.CallerClassName))
            {
                logger.Log(logLevel, message, logEvent.CallerMemberName, logEvent.CallerLineNumber, logEvent.CallerClassName);
            }
            else
            {
                logger.Log(logLevel, message, string.Empty, 0, string.Empty);
            }
        }
    }
}
