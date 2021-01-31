using System;
using System.Collections.Generic;

namespace KissLog
{
    public class LocalInstanceLoggerFactory : IKissLoggerFactory
    {
        private readonly ILogger _logger;
        public LocalInstanceLoggerFactory(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ILogger Get(string categoryName = null, string url = null)
        {
            return _logger;
        }

        public IEnumerable<ILogger> GetAll()
        {
            return new List<ILogger> { _logger };
        }
    }
}
