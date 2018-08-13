using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog
{
    public static class InternalHelpers
    {
        private const int RequestPropertyKeyLength = 100;
        private const int RequestPropertyValueLength = 1000;
        private const int RequestPropertyInputStreamLength = 2000;

        private const string DefaultResponseFileName = "Response.txt";

        public const string LogRequestInputStreamProperty = "X-KissLog-LogRequestInputStream";
        public const string LogResponseBodyProperty = "X-KissLog-LogResponseBody";

        public static KeyValuePair<string, string> TruncateRequestPropertyValue(string key, string value)
        {
            if (!string.IsNullOrEmpty(key) && key.Length > RequestPropertyKeyLength)
            {
                key = $"{key.Substring(0, RequestPropertyKeyLength - 3)}***";
            }

            if (!string.IsNullOrEmpty(value) && value.Length > RequestPropertyValueLength)
            {
                value = $"{value.Substring(0, RequestPropertyValueLength - 3)}***";
            }

            return new KeyValuePair<string, string>(key, value);
        }

        public static string TruncateInputStream(string inputStream)
        {
            if (string.IsNullOrEmpty(inputStream) || inputStream.Length <= RequestPropertyInputStreamLength)
                return inputStream;

            if (inputStream.Trim().StartsWith("{") == false)
                return $"{inputStream.Substring(0, RequestPropertyInputStreamLength - 3)}***";

            try
            {
                Dictionary<string, object> asDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(inputStream);
                if (asDictionary == null || !asDictionary.Any())
                    return $"{inputStream.Substring(0, RequestPropertyInputStreamLength - 3)}***";

                Dictionary<string, object> result = new Dictionary<string, object>();

                foreach (var item in asDictionary)
                {
                    string key = item.Key;
                    string value = item.Value == null ? null : item.Value.ToString();
                    bool valueChanged = false;

                    if (!string.IsNullOrEmpty(key) && key.Length > RequestPropertyKeyLength)
                    {
                        key = $"{key.Substring(0, RequestPropertyKeyLength - 3)}***";
                    }

                    if (!string.IsNullOrEmpty(value) && value.Length > RequestPropertyValueLength)
                    {
                        value = $"{value.Substring(0, RequestPropertyValueLength - 3)}***";
                        valueChanged = true;
                    }

                    result.Add(key, valueChanged ? value : item.Value);
                }

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }
            catch
            {
                return $"{inputStream.Substring(0, RequestPropertyInputStreamLength - 3)}***";
            }
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
    }
}
