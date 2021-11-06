using Microsoft.Extensions.Logging;

namespace KissLog.AspNetCore
{
    internal class LoggerProvider : ILoggerProvider
    {
        private readonly LoggerOptions _options;
        public LoggerProvider() : this(null)
        {
        }
        public LoggerProvider(LoggerOptions options)
        {
            _options = options;
        }

        public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
        {
            return new LoggerAdapter(_options, categoryName);
        }

        public void Dispose()
        {

        }
    }
}
