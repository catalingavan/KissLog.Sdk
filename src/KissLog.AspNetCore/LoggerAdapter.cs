using Microsoft.Extensions.Logging;
using System;

namespace KissLog.AspNetCore
{
    internal class LoggerAdapter : Microsoft.Extensions.Logging.ILogger
    {
        private readonly string _category;
        private readonly IKissLoggerFactory _factory;
        private readonly Func<object, Exception, string> _formatter;
        public LoggerAdapter(string category) : this(category, null)
        {
        }
        public LoggerAdapter(string category, KissLogAspNetCoreOptions options)
        {
            _category = category;
            _factory = options?.Factory;
            _formatter = options?.Formatter;
        }

        private ILogger Logger
        {
            get
            {
                IKissLoggerFactory factory = _factory == null ? KissLog.Logger.Factory : _factory;
                return factory.Get(categoryName: _category);
            }
        }

        public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string message = null;
            if(_formatter != null)
            {
                message = _formatter(state, exception);
            }

            if(string.IsNullOrEmpty(message))
            {
                message = formatter(state, exception);
            }

            switch (logLevel)
            {
                case Microsoft.Extensions.Logging.LogLevel.Trace:
                default:
                    Logger.Trace(message);
                    break;

                case Microsoft.Extensions.Logging.LogLevel.Debug:
                    Logger.Debug(message);
                    break;

                case Microsoft.Extensions.Logging.LogLevel.Information:
                    Logger.Info(message);
                    break;

                case Microsoft.Extensions.Logging.LogLevel.Warning:
                    Logger.Warn(message);
                    break;

                case Microsoft.Extensions.Logging.LogLevel.Error:
                    Logger.Error(message);
                    break;

                case Microsoft.Extensions.Logging.LogLevel.Critical:
                    Logger.Critical(message);
                    break;

                case Microsoft.Extensions.Logging.LogLevel.None:
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
