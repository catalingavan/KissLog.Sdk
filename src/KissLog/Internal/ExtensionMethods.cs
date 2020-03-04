using System;
using System.Collections.Generic;
using KissLog.FlushArgs;
using KissLog.Web;

namespace KissLog.Internal
{
    public static class ExtensionMethods
    {
        public static UserDetails ApplyGetUser(this Options options, RequestProperties request)
        {
            return options.GetUserFn(request);
        }

        public static bool ApplyShouldLogRequestHeader(this Options options, ILogListener listener, FlushLogArgs args, string name)
        {
            return options.ShouldLogRequestHeaderKeyFn(listener, args, name);
        }

        public static bool ApplyShouldLogRequestCookie(this Options options, ILogListener listener, FlushLogArgs args, string name)
        {
            return options.ShouldLogRequestCookieKeyFn(listener, args, name);
        }

        public static bool ApplyShouldLogRequestQueryString(this Options options, ILogListener listener, FlushLogArgs args, string name)
        {
            return options.ShouldLogRequestQueryStringKeyFn(listener, args, name);
        }

        public static bool ApplyShouldLogRequestFormData(this Options options, HttpRequest request)
        {
            return options.ShouldLogRequestFormDataFn(request);
        }

        public static bool ApplyShouldLogRequestFormDataKey(this Options options, ILogListener listener, FlushLogArgs args, string name)
        {
            return options.ShouldLogRequestFormDataKeyFn(listener, args, name);
        }

        public static bool ApplyShouldLogRequestServerVariable(this Options options, ILogListener listener, FlushLogArgs args, string name)
        {
            return options.ShouldLogRequestServerVariableKeyFn(listener, args, name);
        }

        public static bool ApplyShouldLogRequestClaim(this Options options, ILogListener listener, FlushLogArgs args, string name)
        {
            return options.ShouldLogRequestClaimKeyFn(listener, args, name);
        }

        public static bool ApplyShouldLogRequestInputStream(this Options options, HttpRequest request)
        {
            return options.ShouldLogRequestInputStreamFn(request);
        }

        public static bool ApplyShouldLogRequestInputStream(this Options options, ILogger logger, ILogListener listener, FlushLogArgs args)
        {
            return options.ShouldLogRequestInputStreamForListenerFn(listener, args);
        }

        public static bool ApplyShouldLogResponseHeader(this Options options, ILogListener listener, FlushLogArgs args, string name)
        {
            return options.ShouldLogResponseHeaderFn(listener, args, name);
        }

        public static bool ApplyShouldLogResponseBody(this Options options, ILogListener listener, FlushLogArgs args, bool defaultValue)
        {
            return options.ShouldLogResponseBodyFn(listener, args, defaultValue);
        }

        public static string ApplyAppendExceptionDetails(this Options options, Exception ex)
        {
            return options.AppendExceptionDetailsFn(ex);
        }

        public static bool ApplyToggleListener(this Options options, ILogListener listener, FlushLogArgs args)
        {
            return options.ToggleListenerFn(listener, args);
        }
    }
}
