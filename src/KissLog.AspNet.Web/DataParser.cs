using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Claims;
using System.Web;
using System.Linq;

namespace KissLog.AspNet.Web
{
    internal static class DataParser
    {
        public static List<KeyValuePair<string, string>> ToDictionary(NameValueCollection collection)
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();

            foreach (string key in collection.AllKeys)
            {
                string[] values = collection.GetValues(key);
                string value = values == null ? string.Empty : string.Join("; ", values);

                result.Add(
                    new KeyValuePair<string, string>(key, value)
                );
            }

            return result;
        }

        public static List<KeyValuePair<string, string>> ToDictionary(HttpCookieCollection collection)
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();

            foreach (string key in collection)
            {
                HttpCookie cookie = collection.Get(key);
                string value = cookie == null ? string.Empty : string.Join("; ", cookie.Values);

                result.Add(
                    new KeyValuePair<string, string>(key, value)
                );
            }

            return result;
        }

        public static List<KeyValuePair<string, string>> ToDictionary(ClaimsIdentity identity)
        {
            List<KeyValuePair<string, string>> claims =
                identity.Claims
                    .Where(p => string.IsNullOrEmpty(p.Type) == false)
                    .Select(p => new KeyValuePair<string, string>(p.Type, p.Value))
                    .ToList();

            return claims;
        }
    }
}
