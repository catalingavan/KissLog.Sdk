using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Linq;

namespace KissLog.AspNetCore
{
    public static class LoggerFactory
    {
        public static ILogger GetInstance(IHttpContextAccessor httpContextAccessor, string categoryName = Logger.DefaultCategoryName)
        {
            if (httpContextAccessor == null)
            {
                Debug.WriteLine("httpContextAccessor is null. Creating a new instance");
                return new Logger(categoryName);
            }

            HttpContext ctx = httpContextAccessor.HttpContext;
            return GetInstance(ctx, categoryName);
        }

        public static ILogger GetInstance(HttpContext ctx, string categoryName = Logger.DefaultCategoryName)
        {
            if (ctx == null)
            {
                Debug.WriteLine("HttpContext is null. Creating a new instance");
                return new Logger(categoryName);
            }

            ConcurrentDictionary<string, ILogger> loggersDictionary = null;
            if (ctx.Items.ContainsKey(Constants.LoggersDictionaryKey))
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

        public static IEnumerable<ILogger> GetAll(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor == null)
            {
                Debug.WriteLine("httpContextAccessor is null. Returning empty list");
                return Enumerable.Empty<ILogger>();
            }

            HttpContext ctx = httpContextAccessor.HttpContext;
            return GetAll(ctx);
        }

        public static IEnumerable<ILogger> GetAll(HttpContext ctx)
        {
            if (ctx == null || ctx.Items.ContainsKey(Constants.LoggersDictionaryKey) == false)
            {
                Debug.WriteLine("HttpContext is null. Returning empty list");
                return Enumerable.Empty<ILogger>();
            }

            if (ctx.Items.ContainsKey(Constants.LoggersDictionaryKey) == false)
            {
                Debug.WriteLine("HttpContext Items does not contains ILoggers dictionary. Returning empty list");
                return Enumerable.Empty<ILogger>();
            }

            var dictionary = (ConcurrentDictionary<string, ILogger>) ctx.Items[Constants.LoggersDictionaryKey];
            if (dictionary == null)
            {
                Debug.WriteLine("HttpContext Items does not contains ILoggers dictionary. Returning empty list");
                return Enumerable.Empty<ILogger>();
            }

            return dictionary.Select(p => p.Value).ToList();
        }
    }
}
