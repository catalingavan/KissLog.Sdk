namespace KissLog
{
    public enum LogLevel
    {
        /// <summary>
        /// Used for the most detailed log messages, typically only valuable to a developer debugging an issue. These messages may contain sensitive application data and so should not be enabled in a production environment
        /// </summary>
        Trace,

        /// <summary>
        /// These messages have short-term usefulness during development. They contain information that may be useful for debugging, but have no long-term value
        /// </summary>
        Debug,

        /// <summary>
        /// These messages are used to track the general flow of the application. These logs should have some long term value, as opposed to Verbose level messages, which do not
        /// </summary>
        Information,

        /// <summary>
        /// The Warning level should be used for abnormal or unexpected events in the application flow. These may include errors or other conditions that do not cause the application to stop, but which may need to be investigated in the future. Handled exceptions are a common place to use the Warning log level
        /// </summary>
        Warning,

        /// <summary>
        /// An error should be logged when the current flow of the application must stop due to some failure, such as an exception that cannot be handled or recovered from. These messages should indicate a failure in the current activity or operation (such as the current HTTP request), not an application-wide failure
        /// </summary>
        Error,

        /// <summary>
        /// A critical log level should be reserved for unrecoverable application or system crashes, or catastrophic failure that requires immediate attention. Examples: data loss scenarios, out of disk space
        /// </summary>
        Critical
    }
}
