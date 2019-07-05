namespace KissLog.Internal
{
    public class Constants
    {
        internal const string DefaultResponseFileName = "Response.txt";

        public const string LogResponseBodyProperty = "X-KissLog-LogResponseBody";
        public const string AutoFlushProperty = "X-KissLog-AutoFlush";
        public const string FactoryNameProperty = "X-KissLog-FactoryName";
        public const string IsCreatedByHttpRequestProperty = "X-KissLog-IsCreatedByHttpRequest";

        internal const long LoggerFileMaximumSizeBytes = 5 * 1024 * 1024;
    }
}
