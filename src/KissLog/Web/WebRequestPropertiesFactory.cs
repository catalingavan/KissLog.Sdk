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
                MachineName = "Not available",
                UserAgent = "Not available",
                Request = new RequestProperties(),
                Response = new ResponseProperties
                {
                    HttpStatusCode = HttpStatusCode.OK
                },
                StartDateTime = DateTime.UtcNow,
                EndDateTime = DateTime.UtcNow.AddMilliseconds(100)
            };
        }
    }
}
