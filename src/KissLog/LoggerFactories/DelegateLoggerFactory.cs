using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace KissLog.LoggerFactories
{
    internal class DelegateLoggerFactory : ILoggerFactory
    {
        private readonly ConcurrentDictionary<Guid, Logger> _loggers;
        private readonly Func<string, string, Logger> _loggerFn;
        public DelegateLoggerFactory(Func<string, string, Logger> loggerFn)
        {
            _loggerFn = loggerFn ?? throw new ArgumentNullException(nameof(loggerFn));
            _loggers = new ConcurrentDictionary<Guid, Logger>();
        }
     
        public Logger Get(string categoryName = null, string url = null)
        {
            Logger logger = _loggerFn.Invoke(categoryName, url);

            if (logger == null)
                logger = new Logger(categoryName: categoryName, url: url);

            return _loggers.GetOrAdd(logger.Id, logger);
        }

        public IEnumerable<Logger> GetAll()
        {
            List<Logger> loggers = new List<Logger>();

            foreach (Guid id in _loggers.Keys)
            {
                if (_loggers.TryGetValue(id, out Logger logger) && logger != null)
                {
                    loggers.Add(logger);
                }
            }

            return loggers;
        }
    }
}
