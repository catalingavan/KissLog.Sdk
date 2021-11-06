using KissLog.Http;
using System;
using System.Collections.Generic;

namespace KissLog.CloudListeners.RequestLogsListener
{
    public static class OptionsExtensionMethods
    {
        public static KissLog.Options CreateUserPayload(this KissLog.Options options, Func<HttpRequest, KissLog.RestClient.Requests.CreateRequestLog.User> handler)
        {
            if (handler == null)
                return options;

            RequestLogsApiListener.Options.Handlers.CreateUserPayload = handler;
            return options;
        }

        public static KissLog.Options GenerateSearchKeywords(this KissLog.Options options, Func<FlushLogArgs, IEnumerable<string>> handler)
        {
            if (handler == null)
                return options;

            RequestLogsApiListener.Options.Handlers.GenerateSearchKeywords = handler;
            return options;
        }
    }
}
