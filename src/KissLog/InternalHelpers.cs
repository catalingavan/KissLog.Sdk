using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog
{
    internal static class InternalHelpers
    {
        public static void WrapInTryCatch(Action fn)
        {
            if (fn == null)
                throw new ArgumentNullException(nameof(fn));

            try
            {
                fn.Invoke();
            }
            catch (Exception ex)
            {
                InternalLogger.LogException(ex);
            }
        }

        public static T WrapInTryCatch<T>(Func<T> fn)
        {
            if (fn == null)
                throw new ArgumentNullException(nameof(fn));

            try
            {
                return fn.Invoke();
            }
            catch (Exception ex)
            {
                InternalLogger.LogException(ex);
            }

            return default(T);
        }

        public static bool CanReadRequestInputStream(IEnumerable<KeyValuePair<string, string>> requestHeaders)
        {
            if (requestHeaders == null)
                return false;

            string[] allowedContentTypes = Constants.ReadInputStreamContentTypes;

            string contentType = requestHeaders.FirstOrDefault(p => string.Compare(p.Key, "Content-Type", StringComparison.OrdinalIgnoreCase) == 0).Value;
            if (string.IsNullOrEmpty(contentType))
                return false;

            contentType = contentType.Trim().ToLowerInvariant();

            return allowedContentTypes.Any(p => contentType.Contains(p));
        }

        public static bool CanReadResponseBody(IEnumerable<KeyValuePair<string, string>> responseHeaders)
        {
            if (responseHeaders == null)
                return false;

            string[] allowedContentTypes = Constants.ReadResponseBodyContentTypes;

            string contentType = responseHeaders.FirstOrDefault(p => string.Compare(p.Key, "Content-Type", StringComparison.OrdinalIgnoreCase) == 0).Value;
            if (string.IsNullOrEmpty(contentType))
                return false;

            contentType = contentType.Trim().ToLowerInvariant();

            return allowedContentTypes.Any(p => contentType.Contains(p));
        }

        public static string GenerateResponseFileName(IEnumerable<KeyValuePair<string, string>> responseHeaders)
        {
            string defaultResponseFileName = "Response.txt";

            if (responseHeaders == null)
                return defaultResponseFileName;

            string contentType = responseHeaders.FirstOrDefault(p => string.Compare(p.Key, "Content-Type", StringComparison.OrdinalIgnoreCase) == 0).Value ?? string.Empty;
            contentType = contentType.ToLowerInvariant();

            if(contentType.Contains("/json"))
                return "Response.json";

            if (contentType.Contains("/xml"))
                return "Response.xml";

            if (contentType.Contains("/html"))
                return "Response.html";

            return defaultResponseFileName;
        }

        public static IEnumerable<KeyValuePair<string, string>> Clone(IEnumerable<KeyValuePair<string, string>> items)
        {
            if (items == null)
                return new List<KeyValuePair<string, string>>();

            return items.Select(p => new KeyValuePair<string, string>(p.Key, p.Value)).ToList();
        }

        public static bool? GetExplicitLogResponseBody(IEnumerable<Logger> loggers)
        {
            if (loggers == null)
                throw new ArgumentNullException(nameof(loggers));

            Logger defaultLogger = loggers.FirstOrDefault(p => p.CategoryName == Constants.DefaultLoggerCategoryName);

            bool? result = defaultLogger?.DataContainer.LoggerProperties.ExplicitLogResponseBody;
            if (result.HasValue)
                return result.Value;

            result = loggers.FirstOrDefault(p => p.DataContainer.LoggerProperties.ExplicitLogResponseBody.HasValue == true)?.DataContainer.LoggerProperties.ExplicitLogResponseBody;

            return result;
        }
    }
}
