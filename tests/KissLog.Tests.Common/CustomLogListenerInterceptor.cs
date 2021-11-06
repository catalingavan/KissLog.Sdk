using KissLog.Http;
using System;

namespace KissLog.Tests.Common
{
    public class CustomLogListenerInterceptor : ILogListenerInterceptor
    {
        public Func<HttpRequest, bool> ShouldLogBeginRequest { get; set; }
        public Func<LogMessage, bool> ShouldLogMessage { get; set; }
        public Func<FlushLogArgs, bool> ShouldLogFlush { get; set; }

        public bool ShouldLog(HttpRequest httpRequest, ILogListener listener)
        {
            if (ShouldLogBeginRequest != null)
                return ShouldLogBeginRequest.Invoke(httpRequest);

            return true;
        }

        public bool ShouldLog(LogMessage message, ILogListener listener)
        {
            if (ShouldLogMessage != null)
                return ShouldLogMessage.Invoke(message);

            return true;
        }

        public bool ShouldLog(FlushLogArgs args, ILogListener listener)
        {
            if (ShouldLogFlush != null)
                return ShouldLogFlush.Invoke(args);

            return true;
        }
    }
}
