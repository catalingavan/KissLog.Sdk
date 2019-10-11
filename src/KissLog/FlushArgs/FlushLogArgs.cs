using System.Collections.Generic;

namespace KissLog.FlushArgs
{
    public class FlushLogArgs
    {
        public bool IsCreatedByHttpRequest { get; set; }
        public BeginRequestArgs BeginRequestArgs { get; set; }
        public EndRequestArgs EndRequestArgs { get; set; }
        public IEnumerable<LogMessagesGroup> MessagesGroups { get; set; }
        public IEnumerable<CapturedException> CapturedExceptions { get; set; }
        public IEnumerable<LoggerFile> Files { get; set; }
        public IEnumerable<KeyValuePair<string, object>> CustomProperties { get; set; }

        public FlushLogArgs()
        {
            MessagesGroups = new List<LogMessagesGroup>();
            CapturedExceptions = new List<CapturedException>();
            Files = new List<LoggerFile>();
            CustomProperties = new List<KeyValuePair<string, object>>();
        }
    }
}
