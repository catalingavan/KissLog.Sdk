using System;

namespace KissLog.Apis.v1
{
    public class Diagnostics
    {
        public bool IsEnabled { get; set; }

        public Action BeforeFlush { get; } = () => { };
        public Action OnFlushComplete { get; } = () => { };
    }
}
