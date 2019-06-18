using System;
using System.Net;

namespace KissLog.Web
{
    internal static class WebRequestPropertiesFactory
    {
        public static WebRequestProperties CreateDefault()
        {
            return new WebRequestProperties
            {
                Url = new Uri("http://Application/RequestNotAvailable"),
                HttpMethod = "GET",
                MachineName = GetMachineName(),
                UserAgent = "Not available",
                Request = new RequestProperties(),
                Response = new ResponseProperties
                {
                    HttpStatusCode = HttpStatusCode.OK
                },
                StartDateTime = DateTime.UtcNow
            };
        }

        private static string GetMachineName()
        {
            string result = null;

            try
            {
                result = Environment.GetEnvironmentVariable("COMPUTERNAME") ??
                         Environment.GetEnvironmentVariable("HOSTNAME");
            }
            catch
            {
                
            }

            return result ?? "Not available";
        }
    }
}
