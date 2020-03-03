using KissLog.FlushArgs;
using System;
using System.Collections.Generic;

namespace KissLog
{
    public static class ExtensionMethods
    {
        #region Obsolete >= 03.03.2020

        [Obsolete("AddRequestKeywords is obsolete. Use GenerateKeywords(FlushLogArgs args, IList<string> defaultKeywords) instead.", true)]
        public static Options AddRequestKeywords(this Options options, Func<FlushLogArgs, IEnumerable<string>> handler)
        {
            return options;
        }

        #endregion

        public static Options GenerateKeywords(this Options options, Func<FlushLogArgs, IList<string>, IList<string>> handler)
        {
            KissLog.Apis.v1.Configuration.Configuration.Options.GenerateKeywordsFn = handler;
            return options;
        }
    }
}
