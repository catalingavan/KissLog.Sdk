using System;

namespace KissLog
{
    public static partial class ExtensionMethods
    {
        public static void AddCustomProperty(this ILogger logger, string key, string value)
        {
            InternalAddCustomProperty(logger, key, value);
        }

        public static void AddCustomProperty(this ILogger logger, string key, int value)
        {
            InternalAddCustomProperty(logger, key, value);
        }

        public static void AddCustomProperty(this ILogger logger, string key, double value)
        {
            InternalAddCustomProperty(logger, key, value);
        }

        public static void AddCustomProperty(this ILogger logger, string key, decimal value)
        {
            InternalAddCustomProperty(logger, key, value);
        }

        public static void AddCustomProperty(this ILogger logger, string key, bool value)
        {
            InternalAddCustomProperty(logger, key, value);
        }

        public static void AddCustomProperty(this ILogger logger, string key, DateTime value)
        {
            InternalAddCustomProperty(logger, key, value);
        }

        private static void InternalAddCustomProperty(this ILogger logger, string key, object value)
        {
            if (string.IsNullOrEmpty(key))
                return;

            if (key.ToLowerInvariant().StartsWith("x-kisslog-"))
                return;

            if (logger is Logger theLogger)
            {
                theLogger.DataContainer.AddProperty(key, value);
            }
        }
    }
}
