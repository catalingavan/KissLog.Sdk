using KissLog.FlushArgs;
using System.Collections.Generic;

namespace KissLog.CloudListeners.RequestLogsListener
{
    public class ExceptionArgs
    {
        public FlushLogArgs FlushArgs { get; set; }
        public string Payload { get; set; }
        public IList<LoggerFile> Files { get; set; }
        public int HttpStatusCode { get; set; }
        public string Exception { get; set; }

        public ExceptionArgs()
        {
            Files = new List<LoggerFile>();
        }
    }
}
