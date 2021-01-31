using System;

namespace KissLog.AspNetCore
{
    public class KissLogAspNetCoreOptions
    {
        public IKissLoggerFactory Factory { get; set; }
        public Func<object, Exception, string> Formatter { get; set; }
    }
}
