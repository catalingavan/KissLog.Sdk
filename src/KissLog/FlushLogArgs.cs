using System.Collections.Generic;
using KissLog.Web;

namespace KissLog
{
    public class FlushLogArgs
    {
        public bool IsCreatedByHttpRequest { get; set; }
        public WebRequestProperties WebRequestProperties { get; set; }
        public IEnumerable<LogMessagesGroup> MessagesGroups { get; set; }
        public IEnumerable<CapturedException> CapturedExceptions { get; set; }
        public string ErrorMessage { get; set; }
        public IEnumerable<LoggerFile> Files { get; set; }

        public FlushLogArgs()
        {
            MessagesGroups = new List<LogMessagesGroup>();
            CapturedExceptions = new List<CapturedException>();
            Files = new List<LoggerFile>();
        }
    }
}
