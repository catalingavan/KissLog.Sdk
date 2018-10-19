using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Linq;

namespace KissLog.AspNetCore
{
    public static class LoggerFactory
    {
        private const string SdkName = "KissLog.AspNetCore";

        private static readonly ConcurrentDictionary<string, ILogger> StaticInstances = new ConcurrentDictionary<string, ILogger>();

        private static ILogger GetStaticInstance(string categoryName)
        {
            return StaticInstances.GetOrAdd(categoryName, (key) => new Logger(key)
            {
                SdkName = GetSdkName(),
                SdkVersion = GetSdkVersion()
            });
        }

        public static ILogger GetInstance(IHttpContextAccessor httpContextAccessor, string categoryName = Logger.DefaultCategoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                categoryName = Logger.DefaultCategoryName;

            if (httpContextAccessor == null)
            {
                Debug.WriteLine("httpContextAccessor is null. Creating static instance");
                return GetStaticInstance(categoryName);
            }

            HttpContext ctx = httpContextAccessor.HttpContext;
            return GetInstance(ctx, categoryName);
        }

        public static ILogger GetInstance(HttpContext ctx, string categoryName = Logger.DefaultCategoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                categoryName = Logger.DefaultCategoryName;

            if (ctx == null)
            {
                Debug.WriteLine("HttpContext is null. Creating static instance");
                return GetStaticInstance(categoryName);
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

            var logger = loggersDictionary.GetOrAdd(categoryName, (key) => new Logger(key)
            {
                SdkName = GetSdkName(),
                SdkVersion = GetSdkVersion()
            });
            (logger as Logger)?.AddCustomProperty(InternalHelpers.IsCreatedByHttpRequest, true);

            return logger;
        }

        public static IEnumerable<ILogger> GetAll(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor == null)
            {
                Debug.WriteLine("httpContextAccessor is null. Returning static instances");
                return StaticInstances.Values.ToList();
            }

            HttpContext ctx = httpContextAccessor.HttpContext;
            return GetAll(ctx);
        }

        public static IEnumerable<ILogger> GetAll(HttpContext ctx)
        {
            if (ctx == null)
            {
                Debug.WriteLine("HttpContext is null. Returning static instances");
                return StaticInstances.Values.ToList();
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

        private static string GetSdkName()
        {
            return SdkName;
        }

        private static string GetSdkVersion()
        {
            try
            {
                Version version = typeof(LoggerFactory).Assembly.GetName().Version;
                return $"{version.Major}.{version.Minor}.{version.Build}";
            }
            catch
            {
                return "1.0.0";
            }
        }
    }
}
