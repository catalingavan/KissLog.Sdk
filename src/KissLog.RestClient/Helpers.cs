using System;
using System.Linq;

namespace KissLog.RestClient
{
    internal static class Helpers
    {
        public static Uri BuildUri(string baseUrl, string resource)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentNullException(nameof(baseUrl));

            resource = CombineUriParts(baseUrl, resource);
            return new Uri(resource, UriKind.Absolute);
        }

        internal static string CombineUriParts(params string[] uriParts)
        {
            // http://stackoverflow.com/a/6704287

            var uri = string.Empty;
            if (uriParts != null && uriParts.Any())
            {
                uriParts = uriParts.Where(part => !string.IsNullOrWhiteSpace(part)).ToArray();
                char[] trimChars = { '\\', '/' };
                uri = (uriParts[0] ?? string.Empty).TrimEnd(trimChars);
                for (var i = 1; i < uriParts.Length; i++)
                {
                    uri = $"{uri.TrimEnd(trimChars)}/{(uriParts[i] ?? string.Empty).TrimStart(trimChars)}";
                }
            }
            return uri;
        }
    }
}
