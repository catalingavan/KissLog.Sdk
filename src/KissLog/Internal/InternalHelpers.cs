using KissLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog.Internal
{
    public static class InternalHelpers
    {
        public static readonly string[] InputStreamContentTypes = { "text/plain", "application/json", "application/xml", "text/xml", "text/html" };
        public static readonly string[] LogResponseBodyContentTypes = { "application/json" };

        private const string DefaultResponseFileName = "Response.txt";

        public const string LogResponseBodyProperty = "X-KissLog-LogResponseBody";
        public const string IsCreatedByHttpRequest = "X-KissLog-IsCreatedByHttpRequest";

        public static bool ShouldLogInputStream(IEnumerable<KeyValuePair<string, string>> requestHeaders)
        {
            string contentType = requestHeaders.FirstOrDefault(p => string.Compare(p.Key, "Content-Type", StringComparison.OrdinalIgnoreCase) == 0).Value;
            if (string.IsNullOrEmpty(contentType))
                return false;

            contentType = contentType.ToLowerInvariant();
            return InputStreamContentTypes.Any(p => contentType.Contains(p.ToLowerInvariant()));
        }

        public static bool ShouldLogResponseBody(Logger logger, ResponseProperties response)
        {
            var logResponse = logger.DataContainer.GetProperty(LogResponseBodyProperty);
            if (logResponse != null && logResponse is bool asBoolean)
            {
                return asBoolean;
            }

            string contentType = response.Headers.FirstOrDefault(p => string.Compare(p.Key, "Content-Type", StringComparison.OrdinalIgnoreCase) == 0).Value;
            if (string.IsNullOrEmpty(contentType))
                return false;

            contentType = contentType.ToLowerInvariant();
            return LogResponseBodyContentTypes.Any(p => contentType.Contains(p.ToLowerInvariant()));
        }

        public static string ResponseFileName(IEnumerable<KeyValuePair<string, string>> responseHeaders)
        {
            if (responseHeaders == null || !responseHeaders.Any())
                return DefaultResponseFileName;

            string contentType = responseHeaders.FirstOrDefault(p => string.Compare(p.Key, "Content-Type", StringComparison.OrdinalIgnoreCase) == 0).Value;
            if (string.IsNullOrEmpty(contentType))
                return DefaultResponseFileName;

            contentType = contentType.ToLowerInvariant();

            if (contentType.Contains("application/json"))
                return "Response.json";

            if (contentType.Contains("text/html"))
                return "Response.html";

            if (contentType.Contains("application/xml") || contentType.Contains("text/xml"))
                return "Response.xml";

            return DefaultResponseFileName;
        }

        public static string SdkName { get; set; }
        public static string SdkVersion { get; set; }
    }
}
