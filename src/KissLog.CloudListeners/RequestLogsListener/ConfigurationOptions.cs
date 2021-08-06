using KissLog.CloudListeners.RequestLogsListener;
using KissLog.FlushArgs;
using System;
using System.Collections.Generic;

namespace KissLog
{
    internal class ConfigurationOptions
    {
        internal static Func<FlushLogArgs, IList<string>, IList<string>> GenerateKeywordsFn = (FlushLogArgs args, IList<string> defaultKeywords) => defaultKeywords;
        internal static Action<ExceptionArgs> OnRequestLogsApiListenerExceptionFn = (ExceptionArgs args) => { };

        internal static IList<string> ApplyGenerateKeywords(FlushLogArgs args, IList<string> defaultKeywords)
        {
            if (GenerateKeywordsFn == null)
                return null;

            return GenerateKeywordsFn(args, defaultKeywords);
        }

        internal static void ApplyOnRequestLogsApiListenerException(ExceptionArgs args)
        {
            if (OnRequestLogsApiListenerExceptionFn == null)
                return;

            OnRequestLogsApiListenerExceptionFn(args);
        }
    }

    public static class ExtensionMethods
    {
        public static Options GenerateKeywords(this Options options, Func<FlushLogArgs, IList<string>, IList<string>> handler)
        {
            ConfigurationOptions.GenerateKeywordsFn = handler;
            return options;
        }

        public static Options OnRequestLogsApiListenerException(this Options options, Action<ExceptionArgs> handler)
        {
            ConfigurationOptions.OnRequestLogsApiListenerExceptionFn = handler;
            return options;
        }
    }
}
