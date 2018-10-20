using System.Collections.Generic;
using KissLog.Web;

namespace KissLog
{
    public class FlushLogArgs
    {
        public bool IsCreatedByHttpRequest { get; set; }
        public WebRequestProperties WebRequestProperties { get; set; }
        public IEnumerable<LogMessagesGroup> MessagesGroups { get; set; }
        public string ErrorMessage { get; set; }
        public IEnumerable<string> SearchKeywords { get; set; }
        public IEnumerable<LoggerFile> Files { get; set; }

        public FlushLogArgs()
        {
            MessagesGroups = new List<LogMessagesGroup>();
            SearchKeywords = new List<string>();
            Files = new List<LoggerFile>();
        }
    }
}
