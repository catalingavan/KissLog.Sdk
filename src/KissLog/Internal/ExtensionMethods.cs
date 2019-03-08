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

        public static bool ApplyShouldLogRequestInputStream(this Options options, WebRequestProperties request)
        {
            return options.ShouldLogRequestInputStreamFn(request);
        }

        public static bool ApplyShouldLogResponseBody(this Options options, WebRequestProperties request)
        {
            return options.ShouldLogResponseBodyFn(request);
        }

        public static bool ApplyShouldLogCookie(this Options options, WebRequestProperties request, string cookieName)
        {
            return options.ShouldLogCookieFn(request, cookieName);
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
