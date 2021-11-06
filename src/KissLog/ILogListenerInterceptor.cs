using KissLog.Http;

namespace KissLog
{
    public interface ILogListenerInterceptor
    {
        bool ShouldLog(HttpRequest httpRequest, ILogListener listener);
        bool ShouldLog(LogMessage message, ILogListener listener);
        bool ShouldLog(FlushLogArgs args, ILogListener listener);
    }
}
