using System.Collections.Generic;
using KissLog.Web;

namespace KissLog
{
    public class FlushLogArgs
    {
        public WebRequestProperties WebRequestProperties { get; set; }
        public IEnumerable<LogMessagesGroup> MessagesGroups { get; set; }
        // Last error message
        public string ErrorMessage { get; set; }

        public FlushLogArgs()
        {
            MessagesGroups = new List<LogMessagesGroup>();
        }
    }
}
