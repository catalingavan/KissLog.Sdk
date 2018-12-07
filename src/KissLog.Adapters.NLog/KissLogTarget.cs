using NLog;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog.Adapters.NLog
{
    [Target("KissLog")]
    public sealed class KissLogTarget : TargetWithLayout
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

        protected override void Write(LogEventInfo logEvent)
        {
            if (logEvent == null)
                return;

            LogLevel logLevel = GetLogLevel(logEvent);
            string message = this.Layout.Render(logEvent);

            string memberName = logEvent.CallerMemberName;
            string memberType = logEvent.CallerClassName;
            int lineNumber = logEvent.CallerLineNumber;

            ILogger logger = Logger.Factory.Get();

            if (!string.IsNullOrEmpty(memberType))
            {
                logger.Log(logLevel, message, memberName, lineNumber, memberType);
            }
            else
            {
                logger.Log(logLevel, message, string.Empty, 0, string.Empty);
            }
        }

        private LogLevel GetLogLevel(LogEventInfo logEvent)
        {
            foreach (var mapping in LevelsMapping)
            {
                if (mapping.Value.Any(p => string.Compare(p, logEvent.Level.Name, StringComparison.OrdinalIgnoreCase) == 0))
                    return mapping.Key;
            }

            return LogLevel.Debug;
        }
    }
}
