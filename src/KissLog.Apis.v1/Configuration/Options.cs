using KissLog.FlushArgs;
using System;
using System.Collections.Generic;

namespace KissLog.Apis.v1.Configuration
{
    internal class Options
    {
        internal Func<FlushLogArgs, IList<string>, IList<string>> GenerateKeywordsFn = (FlushLogArgs args, IList<string> defaultKeywords) => defaultKeywords;

        internal IList<string> ApplyGenerateKeywords(FlushLogArgs args, IList<string> defaultKeywords)
        {
            if (GenerateKeywordsFn == null)
                return null;

            return GenerateKeywordsFn(args, defaultKeywords);
        }
    }
}
