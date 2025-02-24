using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace KissLog.AspNet.Web
{
    internal static class InternalHelpers
    {
        public static string GetMachineName(HttpRequestBase httpRequest = null)
        {
            string machineName = null;

            try
            {
                machineName = Environment.MachineName;
            }
            catch
            {
                // ignored
            }

            if (string.IsNullOrEmpty(machineName) && httpRequest != null)
            {
                machineName = GetServerNameFromHttpRequest(httpRequest);
            }
            
            return machineName;
        }

        internal static string GetServerNameFromHttpRequest(HttpRequestBase httpRequest)
        {
            if (httpRequest == null)
                throw new ArgumentNullException(nameof(httpRequest));

            if(httpRequest.ServerVariables != null)
            {
                string key = httpRequest.ServerVariables.AllKeys?.FirstOrDefault(p => string.Compare(p, "SERVER_NAME", true) == 0);
                if (!string.IsNullOrEmpty(key))
                {
                    return httpRequest.ServerVariables[key];
                }
            }

            return null;
        }

        internal static string GetRemoteAddress(HttpRequestBase httpRequest, IEnumerable<KeyValuePair<string, string>> requestHeaders)
        {
            if (httpRequest == null)
                throw new ArgumentNullException(nameof(httpRequest));

            if (requestHeaders == null)
                throw new ArgumentNullException(nameof(requestHeaders));

            string forwadedFor = KissLog.InternalHelpers.GetRemoteIPAddressFromRequestHeaders(requestHeaders);
            if (!string.IsNullOrWhiteSpace(forwadedFor))
                return forwadedFor;

            return httpRequest.UserHostAddress;
        }

        public static List<KeyValuePair<string, string>> ToKeyValuePair(NameValueCollection collection)
        {
            if (collection == null)
                return new List<KeyValuePair<string, string>>();

            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();

            foreach (string key in collection.AllKeys)
            {
                if (string.IsNullOrWhiteSpace(key))
                    continue;

                string[] values = collection.GetValues(key);
                string value = values == null ? string.Empty : string.Join("; ", values);

                result.Add(new KeyValuePair<string, string>(key, value));
            }

            return result;
        }

        public static List<KeyValuePair<string, string>> ToKeyValuePair(HttpCookieCollection collection)
        {
            if (collection == null)
                return new List<KeyValuePair<string, string>>();

            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();

            foreach (string key in collection)
            {
                if (string.IsNullOrWhiteSpace(key))
                    continue;

                HttpCookie cookie = collection.Get(key);
                string value = cookie == null ? string.Empty : string.Join("; ", cookie.Values);

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

        public static string ReadInputStream(HttpRequestBase httpRequest)
        {
            if (httpRequest == null)
                throw new ArgumentNullException(nameof(httpRequest));

            if (httpRequest.InputStream == null || httpRequest.InputStream.CanRead == false)
                return null;

            using (MemoryStream ms = new MemoryStream())
            {
                httpRequest.InputStream.CopyTo(ms);

                httpRequest.InputStream.Position = 0;
                ms.Position = 0;

                Encoding encoding = httpRequest.ContentEncoding == null ? Encoding.UTF8 : httpRequest.ContentEncoding;

                using (StreamReader readStream = new StreamReader(ms, encoding))
                {
                    return readStream.ReadToEndAsync().Result;
                }
            }
        }
    }
}
