using KissLog.Web;

namespace KissLog.Listeners
{
    public interface ITextFormatter
    {
        string Format(WebRequestProperties webRequestProperties);

        string Format(LogMessage logMessage);
    }
}
