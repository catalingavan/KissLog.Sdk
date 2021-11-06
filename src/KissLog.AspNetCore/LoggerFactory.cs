using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace KissLog.AspNetCore
{
    internal class LoggerFactory : ILoggerFactory
    {
        public const string DictionaryKey = "KissLog-Loggers";

        internal readonly IHttpContextAccessor _httpContextAccessor;
        public LoggerFactory(IHttpContextAccessor httpContextAccessor = null)
        {
            _httpContextAccessor = httpContextAccessor ?? new HttpContextAccessor();
        }

        public Logger Get(string categoryName = null, string url = null)
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            if(httpContext == null)
                return new Logger(categoryName: categoryName, url: url);

            return GetInstance(httpContext, categoryName: categoryName);
        }

        public IEnumerable<Logger> GetAll()
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                return Enumerable.Empty<Logger>();

            return GetAll(httpContext);
        }

        internal Logger GetInstance(HttpContext context, string categoryName = null)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            ConcurrentDictionary<string, Logger> container = null;
            if (context.Items.ContainsKey(DictionaryKey))
            {
                container = context.Items[DictionaryKey] as ConcurrentDictionary<string, Logger>;
            }
            else
            {
                container = new ConcurrentDictionary<string, Logger>();
                context.Items.Add(DictionaryKey, container);
            }

            Logger logger = new Logger(categoryName: categoryName);
            logger.DataContainer.LoggerProperties.IsManagedByHttpRequest = true;

            return container.GetOrAdd(logger.CategoryName, (key) =>
            {
                return logger;
            });
        }

        internal IEnumerable<Logger> GetAll(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Items.ContainsKey(DictionaryKey) == false)
                return Enumerable.Empty<Logger>();

            ConcurrentDictionary<string, Logger> container = context.Items[DictionaryKey] as ConcurrentDictionary<string, Logger>;
            List<Logger> loggers = new List<Logger>();

            foreach (string key in container.Keys)
            {
                if (container.TryGetValue(key, out Logger logger) && logger != null)
                {
                    loggers.Add(logger);
                }
            }

            return loggers;
        }
    }
}
