using KissLog.Http;

namespace KissLog.Listeners
{
    public interface ITextFormatter
    {
        string FormatBeginRequest(HttpRequest httpRequest);
        string FormatEndRequest(HttpRequest httpRequest, HttpResponse httpResponse);
        string FormatLogMessage(LogMessage logMessage);
    }
}
