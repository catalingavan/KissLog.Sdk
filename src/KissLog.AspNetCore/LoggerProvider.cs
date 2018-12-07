using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace KissLog.AspNetCore
{
    public class LoggerProvider : ILoggerProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoggerProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
        {
            return new LoggerAdapter(_httpContextAccessor, categoryName);
        }

        public void Dispose()
        {
            
        }
    }
}
