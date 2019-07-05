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
        private readonly IKissLoggerFactory _defaultLoggerFactory;
        public AspNetWebLoggerFactory()
        {
            _defaultLoggerFactory = new KissLog.Internal.DefaultLoggerFactory();
        }

        public ILogger Get(string categoryName = null, string url = null)
        {
            return GetInstance(HttpContext.Current, categoryName, url);
        }

        public IEnumerable<ILogger> GetAll()
        {
            return GetAll(HttpContext.Current);
        }

        private ILogger GetInstance(HttpContext ctx, string categoryName, string url)
        {
            if (IsRequestContext(ctx) == false)
            {
                return GetNonWebInstance(categoryName, url);
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

            if (string.IsNullOrWhiteSpace(categoryName))
                categoryName = Logger.DefaultCategoryName;

            return loggersDictionary.GetOrAdd(categoryName, (key) => {
                var logger = new Logger(key);
                logger.DataContainer.AddProperty(KissLog.Internal.Constants.FactoryNameProperty, nameof(AspNetWebLoggerFactory));
                logger.DataContainer.AddProperty(KissLog.Internal.Constants.AutoFlushProperty, true);
                logger.DataContainer.AddProperty(KissLog.Internal.Constants.IsCreatedByHttpRequestProperty, true);

                return logger;
            });
        }

        private IEnumerable<ILogger> GetAll(HttpContext ctx)
        {
            if (ctx == null)
            {
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

        private ILogger GetNonWebInstance(string categoryName, string url)
        {
            return _defaultLoggerFactory.Get(categoryName, url);
        }
    }
}
