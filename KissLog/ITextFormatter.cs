using KissLog.Web;

namespace KissLog
{
    public interface ITextFormatter
    {
        string Format(LogMessage logMessage);
        string Format(WebRequestProperties webRequestProperties);
    }
}
