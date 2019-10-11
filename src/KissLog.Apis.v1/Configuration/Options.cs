using KissLog.FlushArgs;
using System;
using System.Collections.Generic;

namespace KissLog.Apis.v1.Configuration
{
    internal class Options
    {
        internal Func<FlushLogArgs, IEnumerable<string>> AddRequestKeywordsFn = (FlushLogArgs args) => null;

        internal IEnumerable<string> ApplyAddRequestKeywordstHeader(FlushLogArgs args)
        {
            if (AddRequestKeywordsFn == null)
                return null;

            return AddRequestKeywordsFn(args);
        }
    }
}
