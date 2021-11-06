using Microsoft.Extensions.Logging;
using System;

namespace KissLog.AspNetCore
{
    internal class LoggerAdapter : ILogger
    {
        private readonly string _category;
        private readonly LoggerOptions _options;
        public LoggerAdapter(LoggerOptions options, string category = null)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _category = category;
        }

        public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Logger logger = GetLogger();

            string message = formatter(state, exception);

            if (_options.Formatter != null)
            {
                FormatterArgs formatterArgs = new FormatterArgs(new FormatterArgs.CreateOptions
                {
                    State = state,
                    Exception = exception,
                    DefaultValue = message,
                    Logger = logger
                });

                string custom = _options.Formatter.Invoke(formatterArgs);

                message = string.IsNullOrEmpty(custom) ? message : custom;
            }

            switch (logLevel)
            {
                case Microsoft.Extensions.Logging.LogLevel.Trace:
                default:
                    logger.Trace(message);
                    break;

                case Microsoft.Extensions.Logging.LogLevel.Debug:
                    logger.Debug(message);
                    break;

                case Microsoft.Extensions.Logging.LogLevel.Information:
                    logger.Info(message);
                    break;

                case Microsoft.Extensions.Logging.LogLevel.Warning:
                    logger.Warn(message);
                    break;

                case Microsoft.Extensions.Logging.LogLevel.Error:
                    logger.Error(message);
                    break;

                case Microsoft.Extensions.Logging.LogLevel.Critical:
                    logger.Critical(message);
                    break;

                case Microsoft.Extensions.Logging.LogLevel.None:
                    break;
            }
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            Logger logger = GetLogger();

            return new KissLogScope<TState>(state, logger, _options);
        }

        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            return true;
        }

        internal Logger GetLogger()
        {
            ILoggerFactory factory = _options.Factory == null ? Logger.Factory : _options.Factory;
            return factory.Get(categoryName: _category);
        }
    }
}
