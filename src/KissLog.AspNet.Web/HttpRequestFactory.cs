using KissLog.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace KissLog.AspNet.Web
{
    internal static class HttpRequestFactory
    {
        private static readonly string[] ServerVariablesKeysToIgnore = { "all_http", "all_raw" };

        public static KissLog.Web.HttpRequest Create(HttpRequest request)
        {
            KissLog.Web.HttpRequest result = new KissLog.Web.HttpRequest();

            if (request == null)
                return result;

            result.StartDateTime = DateTime.UtcNow;
            result.UserAgent = request.UserAgent;
            result.Url = request.Url;
            result.HttpMethod = request.HttpMethod;
            result.HttpReferer = request.UrlReferrer?.AbsolutePath;
            result.RemoteAddress = request.UserHostAddress;
            result.MachineName = GetMachineName(request);

            KissLog.Web.RequestProperties properties = new KissLog.Web.RequestProperties();
            result.Properties = properties;

            var headers = DataParser.ToDictionary(request.Unvalidated.Headers);
            headers = FilterHeaders(headers);
            
            var serverVariables = DataParser.ToDictionary(request.ServerVariables);
            serverVariables = FilterServerVariables(serverVariables);

            properties.Headers = headers;
            properties.QueryString = DataParser.ToDictionary(request.Unvalidated.QueryString);
            properties.ServerVariables = serverVariables;
            properties.Cookies = DataParser.ToDictionary(request.Unvalidated.Cookies);

            if(KissLogConfiguration.Options.ApplyShouldLogRequestFormData(result))
            {
                properties.FormData = DataParser.ToDictionary(request.Unvalidated.Form);
            }

            if (InternalHelpers.ShouldLogInputStream(headers) && KissLogConfiguration.Options.ApplyShouldLogRequestInputStream(result))
            {
                properties.InputStream = ReadInputStream(request);
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
            try
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
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("ReadInputStream error:");
                sb.AppendLine(ex.ToString());

                KissLog.Internal.InternalHelpers.Log(sb.ToString(), LogLevel.Error);
            }

            return string.Empty;

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

                if (string.Compare(item.Key, "Cookie", StringComparison.OrdinalIgnoreCase) == 0)
                    continue;

                result.Add(new KeyValuePair<string, string>(item.Key, item.Value));
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
                if (string.IsNullOrEmpty(item.Key))
                    continue;

                string key = item.Key.ToLower();

                if (ServerVariablesKeysToIgnore.Contains(key))
                    continue;

                if (key.StartsWith("http_"))
                    continue;

                result.Add(new KeyValuePair<string, string>(item.Key, item.Value));
            }

            return result;
        }
    }
}
