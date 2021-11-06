using Microsoft.Extensions.Logging;

namespace KissLog.AspNetCore
{
    public static class LogFilesExtensionMethods
    {
        public static void LogAsFile(this ILogger logger, string contents, string fileName = null)
        {
            if (logger == null)
                return;

            Logger kisslogger = KissLog.InternalHelpers.WrapInTryCatch(() => InternalHelpers.TryGetKissLogLogger(logger));
            if(kisslogger != null)
            {
                kisslogger.LogAsFile(contents, fileName);
            }
        }

        public static void LogAsFile(this ILogger logger, byte[] contents, string fileName = null)
        {
            if (logger == null)
                return;

            Logger kisslogger = KissLog.InternalHelpers.WrapInTryCatch(() => InternalHelpers.TryGetKissLogLogger(logger));
            if (kisslogger != null)
            {
                kisslogger.LogAsFile(contents, fileName);
            }
        }

        public static void LogFile(this ILogger logger, string sourceFilePath, string fileName = null)
        {
            if (logger == null)
                return;

            Logger kisslogger = KissLog.InternalHelpers.WrapInTryCatch(() => InternalHelpers.TryGetKissLogLogger(logger));
            if (kisslogger != null)
            {
                kisslogger.LogFile(sourceFilePath, fileName);
            }
        }
    }
}
