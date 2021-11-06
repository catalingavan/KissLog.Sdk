using KissLog.Http;
using System;

namespace KissLog.AspNet.Web
{
    internal static class HttpResponseFactory
    {
        public static HttpResponse Create(System.Web.HttpResponseBase httpResponse, long contentLength)
        {
            if (httpResponse == null)
                throw new ArgumentNullException(nameof(httpResponse));

            if (contentLength < 0)
                throw new ArgumentException(nameof(contentLength));

            var options = new HttpResponse.CreateOptions();
            options.StatusCode = httpResponse.StatusCode;
            options.Properties = new ResponseProperties(new ResponseProperties.CreateOptions
            {
                Headers = InternalHelpers.ToKeyValuePair(httpResponse.Headers),
                ContentLength = contentLength
            });

            return new HttpResponse(options);
        }
    }
}
