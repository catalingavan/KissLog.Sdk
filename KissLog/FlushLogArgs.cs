using System.Collections.Generic;
using KissLog.Web;

namespace KissLog
{
    public class FlushLogArgs
    {
        public WebRequestProperties WebRequestProperties { get; set; }
        public IEnumerable<LogMessagesGroup> MessagesGroups { get; set; }
        public string ErrorMessage { get; set; }
        public IEnumerable<LoggerFile> Files { get; set; }

        public FlushLogArgs()
        {
            MessagesGroups = new List<LogMessagesGroup>();
            Files = new List<LoggerFile>();
        }
    }
}
