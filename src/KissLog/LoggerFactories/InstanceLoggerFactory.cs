using System;
using System.Collections.Generic;

namespace KissLog.LoggerFactories
{
    internal class InstanceLoggerFactory : ILoggerFactory
    {
        private readonly Logger _logger;
        public InstanceLoggerFactory(Logger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Logger Get(string categoryName = null, string url = null)
        {
            return _logger;
        }

        public IEnumerable<Logger> GetAll()
        {
            return new List<Logger> { _logger };
        }
    }
}
