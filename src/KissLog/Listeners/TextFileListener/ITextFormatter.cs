using KissLog.FlushArgs;

namespace KissLog.Listeners.TextFileListener
{
    public interface ITextFormatter
    {
        string FormatBeginRequest(BeginRequestArgs args);

        string FormatEndRequest(EndRequestArgs args);

        string FormatLogMessage(LogMessage logMessage);

        string FormatFlush(FormatFlushArgs args);
    }
}
