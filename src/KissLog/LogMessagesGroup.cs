using System.Collections.Generic;

namespace KissLog
{
    public class LogMessagesGroup
    {
        public string CategoryName { get; set; }
        public List<LogMessage> Messages { get; set; } = new List<LogMessage>();
    }
}
