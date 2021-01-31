using Microsoft.Extensions.Logging;

namespace KissLog.AspNetCore
{
    internal class LoggerProvider : ILoggerProvider
    {
        private readonly KissLogAspNetCoreOptions _options;
        public LoggerProvider() :this(null)
        {
        }
        public LoggerProvider(KissLogAspNetCoreOptions options)
        {
            _options = options;
        }

        public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
        {
            return new LoggerAdapter(categoryName, _options);
        }

        public void Dispose()
        {
            
        }
    }
}
