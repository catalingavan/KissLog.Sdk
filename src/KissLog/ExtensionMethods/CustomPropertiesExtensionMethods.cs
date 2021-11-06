using System;
using System.Collections.Generic;

namespace KissLog
{
    public static class CustomPropertiesExtensionMethods
    {
        public static void AddCustomProperty(this IKLogger logger, string name, string value)
        {
            if (logger != null && logger is Logger _logger)
            {
                Add(_logger, name, value);
            }
        }

        public static void AddCustomProperty(this IKLogger logger, string name, int value)
        {
            if (logger != null && logger is Logger _logger)
            {
                Add(_logger, name, value);
            }
        }

        public static void AddCustomProperty(this IKLogger logger, string name, double value)
        {
            if (logger != null && logger is Logger _logger)
            {
                Add(_logger, name, value);
            }
        }

        public static void AddCustomProperty(this IKLogger logger, string name, decimal value)
        {
            if (logger != null && logger is Logger _logger)
            {
                Add(_logger, name, value);
            }
        }

        public static void AddCustomProperty(this IKLogger logger, string name, bool value)
        {
            if (logger != null && logger is Logger _logger)
            {
                Add(_logger, name, value);
            }
        }

        public static void AddCustomProperty(this IKLogger logger, string name, DateTime value)
        {
            if (logger != null && logger is Logger _logger)
            {
                Add(_logger, name, value);
            }
        }

        public static void AddCustomProperty(this IKLogger logger, string name, Guid value)
        {
            if (logger != null && logger is Logger _logger)
            {
                Add(_logger, name, value);
            }
        }

        private static void Add(Logger logger, string name, object value)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;

            logger.DataContainer.LoggerProperties.CustomProperties.Add(new KeyValuePair<string, object>(name, value));
        }
    }
}
