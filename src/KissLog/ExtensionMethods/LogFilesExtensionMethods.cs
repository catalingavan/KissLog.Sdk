namespace KissLog
{
    public static class LogFilesExtensionMethods
    {
        public static void LogAsFile(this IKLogger logger, string contents, string fileName = null)
        {
            if(logger != null && logger is Logger _logger)
            {
                _logger.DataContainer.FilesContainer.LogAsFile(contents, fileName);
            }
        }

        public static void LogAsFile(this IKLogger logger, byte[] contents, string fileName = null)
        {
            if (logger != null && logger is Logger _logger)
            {
                _logger.DataContainer.FilesContainer.LogAsFile(contents, fileName);
            }
        }

        public static void LogFile(this IKLogger logger, string sourceFilePath, string fileName = null)
        {
            if (logger != null && logger is Logger _logger)
            {
                _logger.DataContainer.FilesContainer.LogFile(sourceFilePath, fileName);
            }
        }
    }
}
