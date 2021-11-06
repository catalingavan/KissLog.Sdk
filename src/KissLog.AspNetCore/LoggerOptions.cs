using System;

namespace KissLog.AspNetCore
{
    public class LoggerOptions
    {
        internal ILoggerFactory Factory { get; set; }
        public Func<FormatterArgs, string> Formatter { get; set; }
        public Action<BeginScopeArgs> OnBeginScope { get; set; }
        public Action<EndScopeArgs> OnEndScope { get; set; }
    }
}
