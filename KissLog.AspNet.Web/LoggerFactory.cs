using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace KissLog.AspNet.Web
{
    public static class LoggerFactory
    {
        public static ILogger GetInstance(string categoryName = Logger.DefaultCategoryName)
        {
            return GetInstance(HttpContext.Current, categoryName);
        }

        public static ILogger GetInstance(HttpContext ctx, string categoryName = Logger.DefaultCategoryName)
        {
            if (ctx == null)
            {
                Debug.WriteLine("HttpContext is null. Creating a new instance");
                return new Logger(categoryName);
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

            return loggersDictionary.GetOrAdd(categoryName, new Logger(categoryName));
        }

        public static IEnumerable<ILogger> GetAll(HttpContext ctx)
        {
            if (ctx == null || ctx.Items.Contains(Constants.LoggersDictionaryKey) == false)
            {
                Debug.WriteLine("HttpContext is null. Returning empty list");
                return Enumerable.Empty<ILogger>();
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
