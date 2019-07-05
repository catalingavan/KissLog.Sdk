using KissLog.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KissLog.Internal
{
    public static class InternalHelpers
    {
        public static string SdkName { get; set; }
        public static string SdkVersion { get; set; }

        public static readonly string[] InputStreamContentTypes = { "text/plain", "application/json", "application/xml", "text/xml", "text/html" };
        public static readonly string[] LogResponseBodyContentTypes = { "application/json" };

        public static bool ShouldLogInputStream(IEnumerable<KeyValuePair<string, string>> requestHeaders)
        {
            string contentType = requestHeaders.FirstOrDefault(p => string.Compare(p.Key, "Content-Type", StringComparison.OrdinalIgnoreCase) == 0).Value;
            if (string.IsNullOrEmpty(contentType))
                return false;

            contentType = contentType.ToLowerInvariant();
            return InputStreamContentTypes.Any(p => contentType.Contains(p.ToLowerInvariant()));
        }

        public static bool PreFilterShouldLogResponseBody(Logger defaultLogger, TemporaryFile responseBodyFile, ResponseProperties response)
        {
            // could add more restrictions, like Content-Type: ["text/plain", "application/json"]

            if (string.IsNullOrEmpty(responseBodyFile?.FileName))
                return false;

            FileInfo fi = new FileInfo(responseBodyFile.FileName);
            if (!fi.Exists || fi.Length > Constants.LoggerFileMaximumSizeBytes)
                return false;

            var logResponse = defaultLogger.DataContainer.GetProperty(Constants.LogResponseBodyProperty);
            if (logResponse != null && logResponse is bool asBoolean)
            {
                return asBoolean;
            }

            return true;
        }

        public static bool PreFilterShouldLogResponseBody(Logger defaultLogger, Stream responseStream, ResponseProperties response)
        {
            // could add more restrictions, like Content-Type: ["text/plain", "application/json"]

            if (responseStream == null || responseStream.CanRead == false)
                return false;

            if (responseStream.Length > Constants.LoggerFileMaximumSizeBytes)
                return false;

            var logResponse = defaultLogger.DataContainer.GetProperty(Constants.LogResponseBodyProperty);
            if (logResponse != null && logResponse is bool asBoolean)
            {
                return asBoolean;
            }

            return true;
        }

        public static bool ShouldLogResponseBody(Logger defaultLogger, ILogListener listener, FlushLogArgs args)
        {
            var logResponse = defaultLogger.DataContainer.GetProperty(Constants.LogResponseBodyProperty);
            if (logResponse != null && logResponse is bool asBoolean)
            {
                return asBoolean;
            }

            bool defaultValue = false;

            string contentType = args.WebRequestProperties?.Response?.Headers?.FirstOrDefault(p => string.Compare(p.Key, "Content-Type", StringComparison.OrdinalIgnoreCase) == 0).Value;
            if (!string.IsNullOrEmpty(contentType))
            {
                contentType = contentType.ToLowerInvariant();
                defaultValue = LogResponseBodyContentTypes.Any(p => contentType.Contains(p.ToLowerInvariant()));
            }

            return KissLogConfiguration.Options.ApplyShouldLogResponseBody(listener, args, defaultValue);
        }

        public static string ResponseFileName(IList<KeyValuePair<string, string>> responseHeaders)
        {
            if (responseHeaders == null || !responseHeaders.Any())
                return Constants.DefaultResponseFileName;

            string contentType = responseHeaders.FirstOrDefault(p => string.Compare(p.Key, "Content-Type", StringComparison.OrdinalIgnoreCase) == 0).Value;
            if (string.IsNullOrEmpty(contentType))
                return Constants.DefaultResponseFileName;

            contentType = contentType.ToLowerInvariant();

            if (contentType.Contains("application/json"))
                return "Response.json";

            if (contentType.Contains("text/html"))
                return "Response.html";

            if (contentType.Contains("application/xml") || contentType.Contains("text/xml"))
                return "Response.xml";

            return Constants.DefaultResponseFileName;
        }

        public static void Log(string message, LogLevel logLevel)
        {
            if (KissLogConfiguration.InternalLog == null)
                return;

            KissLogConfiguration.InternalLog.Invoke(message, logLevel);
        }
    }
}
