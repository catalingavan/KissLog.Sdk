using System;

namespace KissLog.AspNet.Web
{
    public static class ExtensionMethods
    {
        [Obsolete("Use logger.IsCreatedByHttpRequest() instead", true)]
        public static bool WillHandleTheRequest(this ILogger logger)
        {
            return (logger as Logger)?.GetProperty(InternalHelpers.IsCreatedByHttpRequest) != null;
        }
    }
}
