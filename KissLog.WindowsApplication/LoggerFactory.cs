using System;
using System.Net;
using System.Net.Sockets;
using KissLog.Web;

namespace KissLog.WindowsApplication
{
    public static class LoggerFactory
    {
        private static string GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return string.Empty;
        }

        private static string GetOsVersion()
        {
            try
            {
                return Environment.OSVersion.ToString();
            }
            catch
            {
                return null;
            }
        }

        private static string GetMachineName()
        {
            try
            {
                return Environment.MachineName;
            }
            catch
            {
                return null;
            }
        }

        public static string DefaultBaseUrl { get; set; }
        public static string DefaultHttpMethod { get; set; } = "GET";

        public static Func<string, string> BuildUri = (string serviceName) =>
        {
            string baseUrl = DefaultBaseUrl;
            if (string.IsNullOrEmpty(baseUrl))
            {
                baseUrl = $"http://windows-application.com";
            }

            return $"{baseUrl}/{serviceName}";
        };

        public static ILogger CreateLogger(string serviceName)
        {
            if (string.IsNullOrEmpty(serviceName))
                serviceName = nameof(serviceName);

            var webRequestProperties = new WebRequestProperties
            {
                Url = new Uri(BuildUri(serviceName)),
                HttpMethod = DefaultHttpMethod,
                UserAgent = GetOsVersion(),
                MachineName = GetMachineName(),
                RemoteAddress = GetLocalIpAddress(),
                Request = new RequestProperties(),
                Response = new ResponseProperties(),
                StartDateTime = DateTime.UtcNow
            };

            Logger logger = new Logger();
            logger.SetWebRequestProperties(webRequestProperties);

            return logger;
        }
    }
}
