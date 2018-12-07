using KissLog.Web;

namespace KissLog.Listeners
{
    public interface ITextFormatter
    {
        string Format(LogMessage logMessage);
        string Format(WebRequestProperties webRequestProperties);
    }
}
