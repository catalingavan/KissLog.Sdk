using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

namespace KissLog.AspNetCore
{
    internal static class InternalHelpers
    {
        public static string GetMachineName()
        {
            string name = null;

            try
            {
                name =
                    Environment.GetEnvironmentVariable("CUMPUTERNAME") ??
                    Environment.GetEnvironmentVariable("HOSTNAME") ??
                    System.Net.Dns.GetHostName();
            }
            catch
            {
                // ignored
            }

            return name;
        }

        public static List<KeyValuePair<string, string>> ToKeyValuePair(IRequestCookieCollection collection)
        {
            if (collection == null)
                return new List<KeyValuePair<string, string>>();

            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();

            foreach (string key in collection.Keys)
            {
                if (string.IsNullOrWhiteSpace(key))
                    continue;

                string value = collection[key];

                result.Add(new KeyValuePair<string, string>(key, value));
            }

            return result;
        }

        public static List<KeyValuePair<string, string>> ToKeyValuePair(IHeaderDictionary collection)
        {
            if (collection == null)
                return new List<KeyValuePair<string, string>>();

            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();

            foreach (string key in collection.Keys)
            {
                if (string.IsNullOrWhiteSpace(key))
                    continue;

                string value = collection[key];

                result.Add(new KeyValuePair<string, string>(key, value));
            }

            return result;
        }

        public static List<KeyValuePair<string, string>> ToKeyValuePair(IFormCollection collection)
        {
            if (collection == null)
                return new List<KeyValuePair<string, string>>();

            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();

            foreach (string key in collection.Keys)
            {
                if (string.IsNullOrWhiteSpace(key))
                    continue;

                string value = collection[key];

                result.Add(new KeyValuePair<string, string>(key, value));
            }

            return result;
        }

        public static List<KeyValuePair<string, string>> ToKeyValuePair(IQueryCollection collection)
        {
            if (collection == null)
                return new List<KeyValuePair<string, string>>();

            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();

            foreach (string key in collection.Keys)
            {
                if (string.IsNullOrWhiteSpace(key))
                    continue;

                string value = collection[key];

                result.Add(new KeyValuePair<string, string>(key, value));
            }

            return result;
        }

        public static List<KeyValuePair<string, string>> ToKeyValuePair(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null || claimsIdentity.Claims == null)
                return new List<KeyValuePair<string, string>>();

            List<KeyValuePair<string, string>> result = claimsIdentity.Claims.Where(p => !string.IsNullOrWhiteSpace(p.Type)).Select(p => new KeyValuePair<string, string>(p.Type, p.Value)).ToList();

            return result;
        }

        internal static Logger TryGetKissLogLogger(ILogger logger)
        {
            LoggerAdapter loggerAdapter = TryGetLoggerAdapter(logger);
            if (loggerAdapter != null)
            {
                return loggerAdapter.GetLogger();
            }

            var factory = new LoggerFactory();
            return factory.Get();
        }

        private static LoggerAdapter TryGetLoggerAdapter(ILogger logger)
        {
            if (logger == null)
                return null;

            if (logger is KissLog.AspNetCore.LoggerAdapter)
            {
                return logger as KissLog.AspNetCore.LoggerAdapter;
            }

            FieldInfo loggerField = logger.GetType().GetField("_logger", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (loggerField == null)
                return null;

            object loggerFieldValue = loggerField.GetValue(logger);
            if (loggerFieldValue == null)
                return null;

            PropertyInfo loggersProperty = loggerFieldValue.GetType().GetProperty("Loggers", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (loggersProperty == null)
                return null;

            Array loggersArray = loggersProperty.GetValue(loggerFieldValue) as Array;
            if (loggersArray == null)
                return null;

            foreach (var loggerInformation in loggersArray)
            {
                if (loggerInformation == null)
                    continue;

                PropertyInfo loggerProperty = loggerInformation.GetType().GetProperty("Logger", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (loggerProperty != null)
                {
                    var loggerItem = loggerProperty.GetValue(loggerInformation);
                    if (loggerItem is KissLog.AspNetCore.LoggerAdapter loggerAdapter)
                    {
                        return loggerAdapter;
                    }
                }
            }

            return null;
        }
    }
}
