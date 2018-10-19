using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace KissLog.AspNet.Web
{
    public static class LoggerFactory
    {
        private static readonly ConcurrentDictionary<string, ILogger> StaticInstances = new ConcurrentDictionary<string, ILogger>();

        private static ILogger GetStaticInstance(string categoryName)
        {
            return StaticInstances.GetOrAdd(categoryName, (key) => new Logger(key));
        }

        public static ILogger GetInstance(string categoryName = Logger.DefaultCategoryName)
        {
            return GetInstance(HttpContext.Current, categoryName);
        }

        public static ILogger GetInstance(HttpContext ctx, string categoryName = Logger.DefaultCategoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                categoryName = Logger.DefaultCategoryName;

            if (ctx == null)
            {
                return GetStaticInstance(categoryName);
            }

            ConcurrentDictionary<string, ILogger> loggersDictionary = null;
            if (ctx.Items.Contains(Constants.LoggersDictionaryKey))
            {
                loggersDictionary = (ConcurrentDictionary<string, ILogger>) ctx.Items[Constants.LoggersDictionaryKey];
            }
            else
            {
                loggersDictionary = new ConcurrentDictionary<string, ILogger>();
                ctx.Items[Constants.LoggersDictionaryKey] = loggersDictionary;
            }

            var logger = loggersDictionary.GetOrAdd(categoryName, (key) => new Logger(key));
            (logger as Logger)?.AddCustomProperty(InternalHelpers.IsCreatedByHttpRequest, true);

            return logger;
        }

        public static IEnumerable<ILogger> GetAll(HttpContext ctx)
        {
            if (ctx == null)
            {
                return StaticInstances.Values.ToList();
            }

            if (ctx.Items.Contains(Constants.LoggersDictionaryKey) == false)
            {
                Debug.WriteLine("HttpContext Items does not contains ILoggers dictionary. Returning empty list");
                return Enumerable.Empty<ILogger>();
            }

            var dictionary = (ConcurrentDictionary<string, ILogger>)ctx.Items[Constants.LoggersDictionaryKey];
            if (dictionary == null)
            {
                Debug.WriteLine("HttpContext Items does not contains ILoggers dictionary. Returning empty list");
                return Enumerable.Empty<ILogger>();
            }

            return dictionary.Select(p => p.Value).ToList();
        }
    }
}
