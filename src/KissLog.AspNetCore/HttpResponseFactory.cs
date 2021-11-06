using KissLog.Http;
using System;

namespace KissLog.AspNetCore
{
    internal static class HttpResponseFactory
    {
        public static HttpResponse Create(Microsoft.AspNetCore.Http.HttpResponse httpResponse, long contentLength)
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
