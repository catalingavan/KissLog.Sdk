namespace KissLog
{
    public static partial class ExtensionMethods
    {
        /// <summary>
        /// <para>Logs a file</para>
        /// </summary>
        public static void LogFile(this ILogger logger, string sourceFilePath, string fileName)
        {
            if (logger is Logger theLogger)
            {
                theLogger.DataContainer.LoggerFiles.LogFile(sourceFilePath, fileName);
            }
        }

        /// <summary>
        /// <para>Logs a file</para>
        /// </summary>
        public static void LogAsFile(this ILogger logger, byte[] content, string fileName)
        {
            if (logger is Logger theLogger)
            {
                theLogger.DataContainer.LoggerFiles.LogAsFile(content, fileName);
            }
        }

        /// <summary>
        /// <para>Logs a file</para>
        /// </summary>
        public static void LogAsFile(this ILogger logger, string content, string fileName)
        {
            if (logger is Logger theLogger)
            {
                theLogger.DataContainer.LoggerFiles.LogAsFile(content, fileName);
            }
        }
    }
}
