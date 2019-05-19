using KissLog.Internal;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace KissLog.AspNet.Web
{
    internal class AspNetWebLoggerFactory : IKissLoggerFactory
    {
        public ILogger Get(string categoryName = Logger.DefaultCategoryName)
        {
            return GetInstance(HttpContext.Current, categoryName);
        }

        public IEnumerable<ILogger> GetAll()
        {
            return GetAll(HttpContext.Current);
        }

        private ILogger GetInstance(HttpContext ctx, string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                categoryName = Logger.DefaultCategoryName;

            if (IsRequestContext(ctx) == false)
            {
                return GetStaticInstance(categoryName);
            }

            ConcurrentDictionary<string, ILogger> loggersDictionary = null;
            if (ctx.Items.Contains(Constants.LoggersDictionaryKey))
            {
                loggersDictionary = (ConcurrentDictionary<string, ILogger>)ctx.Items[Constants.LoggersDictionaryKey];
            }
            else
            {
                loggersDictionary = new ConcurrentDictionary<string, ILogger>();
                ctx.Items[Constants.LoggersDictionaryKey] = loggersDictionary;
            }

            return loggersDictionary.GetOrAdd(categoryName, (key) => {
                var theLogger = new Logger(key);
                theLogger.DataContainer.AddProperty(InternalHelpers.IsCreatedByHttpRequest, true);

                return theLogger;
            });
        }

        private IEnumerable<ILogger> GetAll(HttpContext ctx)
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

        private bool IsRequestContext(HttpContext ctx)
        {
            if (ctx == null)
                return false;

            try
            {
                return ctx.Request != null;
            }
            catch
            {
                return false;
            }
        }

        private static readonly ConcurrentDictionary<string, ILogger> StaticInstances = new ConcurrentDictionary<string, ILogger>();

        private static ILogger GetStaticInstance(string categoryName)
        {
            return StaticInstances.GetOrAdd(categoryName, (key) => new Logger(key));
        }
    }
}
