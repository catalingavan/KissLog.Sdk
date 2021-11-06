namespace KissLog
{
    public static class LoggerExtensionMethods
    {
        public static void SetStatusCode(this IKLogger logger, int statusCode)
        {
            if (logger != null && logger is Logger _logger)
            {
                _logger.DataContainer.LoggerProperties.ExplicitStatusCode = statusCode;
            }
        }

        public static void LogResponseBody(this IKLogger logger, bool value)
        {
            if (logger != null && logger is Logger _logger)
            {
                _logger.DataContainer.LoggerProperties.ExplicitLogResponseBody = value;
            }
        }

        public static bool AutoFlush(this IKLogger logger)
        {
            if (logger != null && logger is Logger _logger)
            {
                return _logger.DataContainer.LoggerProperties.IsManagedByHttpRequest == true;
            }

            return false;
        }
    }
}
