using KissLog.LoggerFactories;
using System;
using System.Collections.Generic;

namespace KissLog
{
    public class LoggerFactory : ILoggerFactory
    {
        internal readonly ILoggerFactory _factory;
        public LoggerFactory(Logger logger)
        {
            _factory = new InstanceLoggerFactory(logger);
        }
        public LoggerFactory(Func<string, string, Logger> loggerFn)
        {
            _factory = new DelegateLoggerFactory(loggerFn);
        }
        public LoggerFactory()
        {
            _factory = new DefaultLoggerFactory();
        }

        public Logger Get(string categoryName = null, string url = null)
        {
            return _factory.Get(categoryName, url);
        }

        public IEnumerable<Logger> GetAll()
        {
            return _factory.GetAll();
        }
    }
}
