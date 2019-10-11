using KissLog.FlushArgs;

namespace KissLog
{
    public interface ILogListener
    {
        int MinimumResponseHttpStatusCode { get; }
        LogLevel MinimumLogMessageLevel { get; }
        LogListenerParser Parser { get; }

        void OnBeginRequest(BeginRequestArgs args, ILogger logger);

        void OnMessage(LogMessage message, ILogger logger);

        void OnFlush(FlushLogArgs args, ILogger logger);
    }
}
