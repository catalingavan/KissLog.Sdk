using KissLog.Web;

namespace KissLog.Listeners.TextFileListener
{
    public interface ITextFormatter
    {
        string FormatBeginRequest(HttpRequest httpRequest);

        string FormatEndRequest(HttpRequest httpRequest, HttpResponse httpResponse);

        string FormatLogMessage(LogMessage logMessage);

        string FormatFlush(WebProperties webProperties);
    }
}
