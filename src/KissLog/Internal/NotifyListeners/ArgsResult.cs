using KissLog.FlushArgs;
using System.Collections.Generic;

namespace KissLog.Internal
{
    internal class ArgsResult
    {
        public FlushLogArgs Args { get; set; }
        public List<LoggerFile> Files { get; set; } = new List<LoggerFile>();
    }
}
