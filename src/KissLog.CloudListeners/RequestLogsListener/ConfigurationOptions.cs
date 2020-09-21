using KissLog.FlushArgs;
using System;
using System.Collections.Generic;

namespace KissLog
{
    internal class ConfigurationOptions
    {
        internal static Func<FlushLogArgs, IList<string>, IList<string>> GenerateKeywordsFn = (FlushLogArgs args, IList<string> defaultKeywords) => defaultKeywords;

        internal static IList<string> ApplyGenerateKeywords(FlushLogArgs args, IList<string> defaultKeywords)
        {
            if (GenerateKeywordsFn == null)
                return null;

            return GenerateKeywordsFn(args, defaultKeywords);
        }
    }

    public static class ExtensionMethods
    {
        public static Options GenerateKeywords(this Options options, Func<FlushLogArgs, IList<string>, IList<string>> handler)
        {
            ConfigurationOptions.GenerateKeywordsFn = handler;
            return options;
        }
    }
}
