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

        public static bool ApplyShouldLogRequestHeader(this Options options, WebRequestProperties request, string name)
        {
            return options.ShouldLogRequestHeaderFn(request, name);
        }

        public static bool ApplyShouldLogRequestCookie(this Options options, WebRequestProperties request, string cookieName)
        {
            return options.ShouldLogRequestCookieFn(request, cookieName);
        }

        public static bool ApplyShouldLogRequestInputStream(this Options options, ILogger logger, WebRequestProperties request)
        {
            if (logger is Logger theLogger)
            {
                var logResponse = theLogger.GetProperty(InternalHelpers.LogRequestInputStreamProperty);
                if (logResponse != null && logResponse is bool asBoolean)
                {
                    return asBoolean;
                }
            }

            return options.ShouldLogRequestInputStreamFn(request);
        }

        public static bool ApplyShouldLogResponseHeader(this Options options, WebRequestProperties request, string name)
        {
            return options.ShouldLogResponseHeaderFn(request, name);
        }

        public static bool ApplyShouldLogResponseBody(this Options options, ILogger logger, WebRequestProperties request)
        {
            if (logger is Logger theLogger)
            {
                var logResponse = theLogger.GetProperty(InternalHelpers.LogResponseBodyProperty);
                if (logResponse != null && logResponse is bool asBoolean)
                {
                    return asBoolean;
                }
            }

            return options.ShouldLogResponseBodyFn(request);
        }

        public static string ApplyAppendExceptionDetails(this Options options, Exception ex)
        {
            return options.AppendExceptionDetailsFn(ex);
        }

        public static bool ApplyToggleListener(this Options options, ILogListener listener, WebRequestProperties request)
        {
            return options.ToggleListenerFn(listener, request);
        }
    }
}
