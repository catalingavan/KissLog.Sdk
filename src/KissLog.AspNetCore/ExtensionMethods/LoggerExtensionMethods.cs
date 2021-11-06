using Microsoft.Extensions.Logging;

namespace KissLog.AspNetCore
{
    public static class LoggerExtensionMethods
    {
        public static void SetStatusCode(this ILogger logger, int statusCode)
        {
            if (logger == null)
                return;

            Logger kisslogger = KissLog.InternalHelpers.WrapInTryCatch(() => InternalHelpers.TryGetKissLogLogger(logger));
            if (kisslogger != null)
            {
                kisslogger.SetStatusCode(statusCode);
            }
        }

        public static void LogResponseBody(this ILogger logger, bool value)
        {
            if (logger == null)
                return;

            Logger kisslogger = KissLog.InternalHelpers.WrapInTryCatch(() => InternalHelpers.TryGetKissLogLogger(logger));
            if (kisslogger != null)
            {
                kisslogger.LogResponseBody(value);
            }
        }

        public static bool AutoFlush(this ILogger logger)
        {
            if (logger == null)
                return false;

            Logger kisslogger = KissLog.InternalHelpers.WrapInTryCatch(() => InternalHelpers.TryGetKissLogLogger(logger));
            if (kisslogger != null)
            {
                return kisslogger.AutoFlush();
            }

            return false;
        }
    }
}
