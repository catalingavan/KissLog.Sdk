using System;
using KissLog.Web;

namespace KissLog.Internal
{
    public static class ExtensionMethods
    {
        public static UserDetails ApplyGetUser(this Options options, RequestProperties request)
        {
            return options.GetUserFn(request);
        }

        public static bool ApplyShouldLogRequestHeader(this Options options, ILogListener listener, WebRequestProperties request, string name)
        {
            return options.ShouldLogRequestHeaderFn(listener, request, name);
        }

        public static bool ApplyShouldLogRequestCookie(this Options options, ILogListener listener, WebRequestProperties request, string cookieName)
        {
            return options.ShouldLogRequestCookieFn(listener, request, cookieName);
        }

        public static bool ApplyShouldLogRequestInputStream(this Options options, ILogger logger, ILogListener listener, WebRequestProperties request)
        {
            if (logger is Logger theLogger)
            {
                var logResponse = theLogger.GetProperty(InternalHelpers.LogRequestInputStreamProperty);
                if (logResponse != null && logResponse is bool asBoolean)
                {
                    return asBoolean;
                }
            }

            return options.ShouldLogRequestInputStreamFn(listener, request);
        }

        public static bool ApplyShouldLogResponseHeader(this Options options, ILogListener listener, WebRequestProperties request, string name)
        {
            return options.ShouldLogResponseHeaderFn(listener, request, name);
        }

        public static bool ApplyShouldLogResponseBody(this Options options, ILogger logger, ILogListener listener, WebRequestProperties request)
        {
            if (logger is Logger theLogger)
            {
                var logResponse = theLogger.GetProperty(InternalHelpers.LogResponseBodyProperty);
                if (logResponse != null && logResponse is bool asBoolean)
                {
                    return asBoolean;
                }
            }

            return options.ShouldLogResponseBodyFn(listener, request);
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
