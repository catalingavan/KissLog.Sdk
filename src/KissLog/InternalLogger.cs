using System;

namespace KissLog
{
    internal static class InternalLogger
    {
        public static void LogException(Exception ex)
        {
            if (ex == null)
                return;

            Log(ex.ToString(), LogLevel.Error);
        }

        public static void Log(string message, LogLevel logLevel)
        {
            if (string.IsNullOrEmpty(message))
                return;

            if (KissLogConfiguration.InternalLog == null)
                return;

            string date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            date = string.Format("{0,-22}", $"{date}");

            string log = logLevel.ToString();
            log = $"[{log}] ";

            message = $"'KissLog' {date}{log}{message}";

            try
            {
                KissLogConfiguration.InternalLog.Invoke(message);
            }
            catch
            {
                // ignored
            }
        }
    }
}
