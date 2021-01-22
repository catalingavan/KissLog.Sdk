using Microsoft.Extensions.Logging;

namespace KissLog.AspNetCore
{
    internal class LoggerProvider : ILoggerProvider
    {
        public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
        {
            // ILogger logger = Logger.Factory.Get(categoryName: categoryName);
            return new LoggerAdapter(categoryName);
        }

        public void Dispose()
        {
            
        }
    }
}
