namespace KissLog
{
    public interface ILogListener
    {
        int MinimumResponseHttpStatusCode { get; }

        LogLevel MinimumLogMessageLevel { get; }

        LogListenerParser Parser { get; }

        void OnFlush(FlushLogArgs args);
    }
}
