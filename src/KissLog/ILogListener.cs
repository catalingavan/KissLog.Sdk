using KissLog.Web;

namespace KissLog
{
    public interface ILogListener
    {
        int MinimumResponseHttpStatusCode { get; }
        LogLevel MinimumLogMessageLevel { get; }
        LogListenerParser Parser { get; }

        void OnBeginRequest(WebRequestProperties webRequestProperties, ILogger logger);

        void OnMessage(LogMessage message, ILogger logger);

        void OnFlush(FlushLogArgs args, ILogger logger);
    }
}
