using KissLog.Internal;
using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace KissLog.AspNetCore
{
    internal class AspNetCoreLoggerFactory : IKissLoggerFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IKissLoggerFactory _defaultLoggerFactory;
        public AspNetCoreLoggerFactory()
        {
            _defaultLoggerFactory = new KissLog.Internal.DefaultLoggerFactory();
            _httpContextAccessor = new HttpContextAccessor();
        }

        public ILogger Get(string categoryName = null, string url = null)
        {
            return GetInstance(_httpContextAccessor, categoryName, url);
        }

        public IEnumerable<ILogger> GetAll()
        {
            return GetAll(_httpContextAccessor);
        }

        private ILogger GetInstance(IHttpContextAccessor httpContextAccessor, string categoryName, string url)
        {
            if (httpContextAccessor == null)
            {
                Debug.WriteLine("httpContextAccessor is null. Creating static instance");
                return GetNonWebInstance(categoryName, url);
            }

            HttpContext ctx = httpContextAccessor.HttpContext;
            return GetInstance(ctx, categoryName, url);
        }

        private ILogger GetInstance(HttpContext ctx, string categoryName, string url)
        {
            if (IsRequestContext(ctx) == false)
            {
                Debug.WriteLine("HttpContext is null. Creating static instance");
                return GetNonWebInstance(categoryName, url);
            }

            ConcurrentDictionary<string, ILogger> loggersDictionary = null;
            if (ctx.Items.ContainsKey(Constants.LoggersDictionaryKey))
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

            var logger = loggersDictionary.GetOrAdd(categoryName, (key) =>
            {
                var theLogger = new Logger(key);
                theLogger.DataContainer.AddProperty(InternalHelpers.IsCreatedByHttpRequest, true);

                return theLogger;
            });

            return logger;
        }

        private IEnumerable<ILogger> GetAll(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor == null)
            {
                Debug.WriteLine("httpContextAccessor is null. Returning static instances");
                return Enumerable.Empty<ILogger>();
            }

            HttpContext ctx = httpContextAccessor.HttpContext;
            return GetAll(ctx);
        }

        private IEnumerable<ILogger> GetAll(HttpContext ctx)
        {
            if (ctx == null)
            {
                Debug.WriteLine("HttpContext is null. Returning static instances");
                return Enumerable.Empty<ILogger>();
            }

            if (ctx.Items.ContainsKey(Constants.LoggersDictionaryKey) == false)
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
