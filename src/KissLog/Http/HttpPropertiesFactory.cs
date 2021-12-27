using System;

namespace KissLog.Http
{
    internal static class HttpPropertiesFactory
    {
        public static HttpProperties Create(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));

            return new HttpProperties(new HttpRequest(new HttpRequest.CreateOptions
            {
                HttpMethod = "GET",
                Url = UrlParser.GenerateUri(url),
                MachineName = InternalHelpers.GetMachineName()
            }));
        }
    }
}
