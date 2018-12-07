using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace KissLog.AspNetCore
{
    internal class LoggerAdapter : Microsoft.Extensions.Logging.ILogger
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _categoryName;
        public LoggerAdapter(IHttpContextAccessor httpContextAccessor, string categoryName)
        {
            _httpContextAccessor = httpContextAccessor;
            _categoryName = categoryName;
        }

        private ILogger GetLogger() => Logger.Factory.Get(_categoryName);

        public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string message = $"{formatter(state, exception)}";

            switch (logLevel)
            {
                case Microsoft.Extensions.Logging.LogLevel.Trace:
                    GetLogger().Trace(message);
                    break;

                case Microsoft.Extensions.Logging.LogLevel.Debug:
                    GetLogger().Debug(message);
                    break;

                case Microsoft.Extensions.Logging.LogLevel.Information:
                    GetLogger().Info(message);
                    break;

                case Microsoft.Extensions.Logging.LogLevel.Warning:
                    GetLogger().Warn(message);
                    break;

                case Microsoft.Extensions.Logging.LogLevel.Critical:
                case Microsoft.Extensions.Logging.LogLevel.Error:
                    GetLogger().Error(message);
                    break;

                case Microsoft.Extensions.Logging.LogLevel.None:
                    break;

                default:
                    GetLogger().Trace(message);
                    break;
            }
        }

        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new NoopDisposable();
        }
    }

    class NoopDisposable : IDisposable
    {
        public void Dispose()
        {

        }
    }
}
