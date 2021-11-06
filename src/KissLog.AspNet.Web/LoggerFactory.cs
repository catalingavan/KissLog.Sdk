using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KissLog.AspNet.Web
{
    internal class LoggerFactory : ILoggerFactory
    {
        public const string DictionaryKey = "KissLog-Loggers";

        public Logger Get(string categoryName = null, string url = null)
        {
            HttpContext context = HttpContext.Current;
            if (context == null)
                return new Logger(categoryName: categoryName, url: url);

            var contextWrapper = new HttpContextWrapper(context);
            return GetInstance(contextWrapper, categoryName: categoryName);
        }

        public IEnumerable<Logger> GetAll()
        {
            HttpContext context = HttpContext.Current;
            if (context == null)
                return Enumerable.Empty<Logger>();

            var contextWrapper = new HttpContextWrapper(context);
            return GetAll(contextWrapper);
        }

        internal Logger GetInstance(HttpContextBase context, string categoryName = null)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            ConcurrentDictionary<string, Logger> container = null;
            if(context.Items.Contains(DictionaryKey))
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

        internal IEnumerable<Logger> GetAll(HttpContextBase context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if(context.Items.Contains(DictionaryKey) == false)
                return Enumerable.Empty<Logger>();

            ConcurrentDictionary<string, Logger> container = context.Items[DictionaryKey] as ConcurrentDictionary<string, Logger>;
            List<Logger> loggers = new List<Logger>();

            foreach(string key in container.Keys)
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
