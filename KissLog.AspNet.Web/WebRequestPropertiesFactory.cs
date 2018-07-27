using KissLog.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace KissLog.AspNet.Web
{
    internal static class WebRequestPropertiesFactory
    {
        private static readonly int MaxKeyLength = 100;
        private static readonly int MaxValueLength = 1000;
        private static readonly int MaxInputStreamLength = 2000;
        private static readonly string[] ServerVariablesKeysToIgnore = {"all_http", "all_raw"};

        public static WebRequestProperties Create(HttpRequest request)
        {
            WebRequestProperties result = new WebRequestProperties();

            if (request == null)
                return result;

            result.StartDateTime = DateTime.UtcNow;
            result.UserAgent = request.UserAgent;
            result.Url = request.Url;
            result.HttpMethod = request.HttpMethod;
            result.HttpReferer = request.UrlReferrer?.AbsolutePath;
            result.RemoteAddress = request.UserHostAddress;
            result.MachineName = GetMachineName(request);

            RequestProperties requestProperties = new RequestProperties();
            result.Request = requestProperties;

            var headers = DataParser.ToDictionary(request.Unvalidated.Headers);
            headers = FilterHeaders(headers);

            var queryString = DataParser.ToDictionary(request.Unvalidated.QueryString);
            queryString = queryString.Select(TruncateValue).ToList();

            var formData = DataParser.ToDictionary(request.Unvalidated.Form);
            formData = formData.Select(TruncateValue).ToList();

            var serverVariables = DataParser.ToDictionary(request.ServerVariables);
            serverVariables = FilterServerVariables(serverVariables);

            var cookies = DataParser.ToDictionary(request.Unvalidated.Cookies);
            cookies = FilterCookies(cookies);

            requestProperties.Headers = headers;
            requestProperties.QueryString = queryString;
            requestProperties.FormData = formData;
            requestProperties.ServerVariables = serverVariables;
            requestProperties.Cookies = cookies;

            if (KissLogConfiguration.ShouldReadInputStream(result))
            {
                string inputStream = ReadInputStream(request);
                if (string.IsNullOrEmpty(inputStream) == false)
                {
                    requestProperties.InputStream = TruncateInputStream(inputStream);
                }
            }

            return result;
        }

        private static string GetMachineName(HttpRequest request)
        {
            string machineName = string.Empty;

            try
            {
                machineName = Environment.MachineName;
            }
            catch
            {
                // ignored
            }

            if (string.IsNullOrEmpty(machineName))
            {
                if (!string.IsNullOrEmpty(request.ServerVariables["SERVER_NAME"]))
                {
                    machineName = request.ServerVariables["SERVER_NAME"];
                }
            }

            return machineName;
        }

        private static string ReadInputStream(HttpRequest request)
        {
            string content = string.Empty;
            if (request.InputStream.CanRead == false)
                return content;

            using (MemoryStream ms = new MemoryStream())
            {
                request.InputStream.CopyTo(ms);

                request.InputStream.Position = 0;
                ms.Position = 0;

                using (StreamReader readStream = new StreamReader(ms, request.ContentEncoding))
                {
                    content = readStream.ReadToEndAsync().Result;
                }
            }

            return content;
        }

        private static List<KeyValuePair<string, string>> FilterHeaders(List<KeyValuePair<string, string>> values)
        {
            if (values == null || !values.Any())
                return values;

            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();

            foreach (var item in values)
            {
                if (string.IsNullOrEmpty(item.Key))
                    continue;

                if(string.Compare(item.Key, "Cookie", StringComparison.OrdinalIgnoreCase) == 0)
                    continue;

                result.Add(TruncateValue(item));
            }

            return result;
        }

        private static List<KeyValuePair<string, string>> FilterServerVariables(List<KeyValuePair<string, string>> values)
        {
            if (values == null || !values.Any())
                return values;

            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();

            foreach (var item in values)
            {
                if(string.IsNullOrEmpty(item.Key))
                    continue;

                string key = item.Key.ToLower();

                if(ServerVariablesKeysToIgnore.Contains(key))
                    continue;

                if(key.StartsWith("http_"))
                    continue;

                result.Add(TruncateValue(item));
            }

            return result;
        }

        private static List<KeyValuePair<string, string>> FilterCookies(List<KeyValuePair<string, string>> values)
        {
            if (values == null || !values.Any())
                return values;

            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();

            foreach (var item in values)
            {
                if(KissLogConfiguration.ShouldReadCookie(item.Key) == false)
                    continue;

                result.Add(TruncateValue(item));
            }

            return result;
        }

        private static KeyValuePair<string, string> TruncateValue(KeyValuePair<string, string> keyValuePair)
        {
            string key = keyValuePair.Key;
            string value = keyValuePair.Value;

            if (!string.IsNullOrEmpty(key) && key.Length > MaxKeyLength)
            {
                key = $"{key.Substring(0, MaxKeyLength - 3)}***";
            }

            if (!string.IsNullOrEmpty(value) && value.Length > MaxValueLength)
            {
                value = $"{value.Substring(0, MaxValueLength - 3)}***";
            }

            return new KeyValuePair<string, string>(key, value);
        }

        private static string TruncateInputStream(string inputStream)
        {
            if (string.IsNullOrEmpty(inputStream) || inputStream.Length <= MaxInputStreamLength)
                return inputStream;

            if (inputStream.Trim().StartsWith("{") == false)
                return $"{inputStream.Substring(0, MaxInputStreamLength - 3)}***";

            try
            {
                Dictionary<string, object> asDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(inputStream);
                if (asDictionary == null || !asDictionary.Any())
                    return $"{inputStream.Substring(0, MaxInputStreamLength - 3)}***";

                Dictionary<string, object> result = new Dictionary<string, object>();

                foreach (var item in asDictionary)
                {
                    string key = item.Key;
                    string value = item.Value == null ? null : item.Value.ToString();
                    bool valueChanged = false;

                    if (!string.IsNullOrEmpty(key) && key.Length > MaxKeyLength)
                    {
                        key = $"{key.Substring(0, MaxKeyLength - 3)}***";
                    }

                    if (!string.IsNullOrEmpty(value) && value.Length > MaxValueLength)
                    {
                        value = $"{value.Substring(0, MaxValueLength - 3)}***";
                        valueChanged = true;
                    }

                    result.Add(key, valueChanged ? value : item.Value);
                }

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }
            catch
            {
                return $"{inputStream.Substring(0, MaxInputStreamLength - 3)}***";
            }
        }
    }
}
