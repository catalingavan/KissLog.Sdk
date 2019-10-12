using KissLog.FlushArgs;
using System;
using System.Collections.Generic;

namespace KissLog
{
    public static class ExtensionMethods
    {
        public static Options AddRequestKeywords(this Options options, Func<FlushLogArgs, IEnumerable<string>> handler)
        {
            KissLog.Apis.v1.Configuration.Configuration.Options.AddRequestKeywordsFn = handler;
            return options;
        }
    }
}
