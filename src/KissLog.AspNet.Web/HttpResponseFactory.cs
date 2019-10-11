using System;
using System.Net;
using System.Web;

namespace KissLog.AspNet.Web
{
    internal static class HttpResponseFactory
    {
        public static KissLog.Web.HttpResponse Create(HttpResponse response)
        {
            KissLog.Web.HttpResponse result = new KissLog.Web.HttpResponse();
            if (response == null)
                return result;

            result.HttpStatusCode = (HttpStatusCode)response.StatusCode;
            result.EndDateTime = DateTime.UtcNow;

            KissLog.Web.ResponseProperties properties = new KissLog.Web.ResponseProperties
            {
                Headers = DataParser.ToDictionary(response.Headers)
            };
            result.Properties = properties;

            return result;
        }
    }
}
