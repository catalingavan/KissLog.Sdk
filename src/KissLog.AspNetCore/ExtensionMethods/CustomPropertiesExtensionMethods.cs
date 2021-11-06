using Microsoft.Extensions.Logging;
using System;

namespace KissLog.AspNetCore
{
    public static class CustomPropertiesExtensionMethods
    {
        public static void AddCustomProperty(this ILogger logger, string name, string value)
        {
            if (logger == null)
                return;

            if (logger == null)
                return;

            Logger kisslogger = KissLog.InternalHelpers.WrapInTryCatch(() => InternalHelpers.TryGetKissLogLogger(logger));
            if (kisslogger != null)
            {
                kisslogger.AddCustomProperty(name, value);
            }
        }

        public static void AddCustomProperty(this ILogger logger, string name, int value)
        {
            if (logger == null)
                return;

            Logger kisslogger = KissLog.InternalHelpers.WrapInTryCatch(() => InternalHelpers.TryGetKissLogLogger(logger));
            if (kisslogger != null)
            {
                kisslogger.AddCustomProperty(name, value);
            }
        }

        public static void AddCustomProperty(this ILogger logger, string name, double value)
        {
            if (logger == null)
                return;

            Logger kisslogger = KissLog.InternalHelpers.WrapInTryCatch(() => InternalHelpers.TryGetKissLogLogger(logger));
            if (kisslogger != null)
            {
                kisslogger.AddCustomProperty(name, value);
            }
        }

        public static void AddCustomProperty(this ILogger logger, string name, decimal value)
        {
            if (logger == null)
                return;

            Logger kisslogger = KissLog.InternalHelpers.WrapInTryCatch(() => InternalHelpers.TryGetKissLogLogger(logger));
            if (kisslogger != null)
            {
                kisslogger.AddCustomProperty(name, value);
            }
        }

        public static void AddCustomProperty(this ILogger logger, string name, bool value)
        {
            if (logger == null)
                return;

            Logger kisslogger = KissLog.InternalHelpers.WrapInTryCatch(() => InternalHelpers.TryGetKissLogLogger(logger));
            if (kisslogger != null)
            {
                kisslogger.AddCustomProperty(name, value);
            }
        }

        public static void AddCustomProperty(this ILogger logger, string name, DateTime value)
        {
            if (logger == null)
                return;

            Logger kisslogger = KissLog.InternalHelpers.WrapInTryCatch(() => InternalHelpers.TryGetKissLogLogger(logger));
            if (kisslogger != null)
            {
                kisslogger.AddCustomProperty(name, value);
            }
        }

        public static void AddCustomProperty(this ILogger logger, string name, Guid value)
        {
            if (logger == null)
                return;

            Logger kisslogger = KissLog.InternalHelpers.WrapInTryCatch(() => InternalHelpers.TryGetKissLogLogger(logger));
            if (kisslogger != null)
            {
                kisslogger.AddCustomProperty(name, value);
            }
        }
    }
}
