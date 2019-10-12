using System;
using System.Net;

namespace KissLog.Web
{
    internal static class WebPropertiesFactory
    {
        public static WebProperties CreateDefault()
        {
            return new WebProperties
            {
                Request = new HttpRequest
                {
                    Url = new Uri("http://Application/RequestNotAvailable"),
                    HttpMethod = "GET",
                    MachineName = GetMachineName(),
                    UserAgent = "Not available",
                    Properties = new RequestProperties(),
                    StartDateTime = DateTime.UtcNow
                },
                Response = new HttpResponse
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    EndDateTime = DateTime.UtcNow,
                    Properties = new ResponseProperties()
                }
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
