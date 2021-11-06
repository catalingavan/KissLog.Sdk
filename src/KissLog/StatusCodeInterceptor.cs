using KissLog.Http;
using System;

namespace KissLog
{
    public class StatusCodeInterceptor : ILogListenerInterceptor
    {
        public int MinimumResponseHttpStatusCode { get; set; }
        public LogLevel MinimumLogMessageLevel { get; set; }

        public bool ShouldLog(HttpRequest httpRequest, ILogListener listener)
        {
            return true;
        }

        public bool ShouldLog(LogMessage message, ILogListener listener)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (message.LogLevel < MinimumLogMessageLevel)
                return false;

            return true;
        }

        public bool ShouldLog(FlushLogArgs args, ILogListener listener)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            if (args.IsCreatedByHttpRequest == false)
                return true;

            int statusCode = args.HttpProperties.Response.StatusCode;

            if (statusCode < MinimumResponseHttpStatusCode)
                return false;

            return true;
        }
    }
}
