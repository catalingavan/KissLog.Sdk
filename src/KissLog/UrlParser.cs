using System;
using System.Text.RegularExpressions;

namespace KissLog
{
    internal static class UrlParser
    {
        public static Uri GenerateUri(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return new Uri(Constants.DefaultBaseUrl, UriKind.Absolute);

            url = url.Trim().Trim('/');

            url = Constants.UrlRegex.Replace(url, "-");

            url = Regex.Replace(url, @"\-+", "-").Trim('-');

            string scheme = "http";
            string host = "application";
            string pathAndQuery = url;

            if (Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
            {
                scheme = uri.Scheme;
                host = uri.Host;
                pathAndQuery = uri.PathAndQuery;
            }

            pathAndQuery = Regex.Replace(pathAndQuery, @"/+", @"/").Trim('/');

            if(pathAndQuery.Contains("?"))
            {
                string path = pathAndQuery.Substring(0, pathAndQuery.IndexOf("?")).Trim('/');
                string query = pathAndQuery.Substring(pathAndQuery.IndexOf("?") + 1).Trim('/');

                pathAndQuery = $"{path}?{query}";
            }

            url = $"{scheme}://{host}/";
            if(!string.IsNullOrEmpty(pathAndQuery))
            {
                url = $"{url}{pathAndQuery}";
            }

            return new Uri(url, UriKind.Absolute);
        }
    }
}
