using KissLog.FlushArgs;

namespace KissLog.Listeners.TextFileListener
{
    public class FormatFlushArgs
    {
        public BeginRequestArgs BeginRequest { get; set; }
        public EndRequestArgs EndRequest { get; set; }
    }
}
