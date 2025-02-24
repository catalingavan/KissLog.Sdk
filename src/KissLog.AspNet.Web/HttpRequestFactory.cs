using KissLog.Http;
using System;
using System.Collections.Generic;

namespace KissLog.AspNet.Web
{
    internal class HttpRequestFactory
    {
        public static HttpRequest Create(System.Web.HttpRequestBase httpRequest)
        {
            if (httpRequest == null)
                throw new ArgumentNullException(nameof(httpRequest));

            List<KeyValuePair<string, string>> requestHeaders = new List<KeyValuePair<string, string>>();
            if (httpRequest.Unvalidated != null)
                requestHeaders = InternalHelpers.ToKeyValuePair(httpRequest.Unvalidated.Headers);

            HttpRequest result = new HttpRequest(new HttpRequest.CreateOptions
            {
                Url = httpRequest.Url,
                HttpMethod = httpRequest.HttpMethod,
                UserAgent = httpRequest.UserAgent,
                HttpReferer = httpRequest.UrlReferrer?.ToString(),
                RemoteAddress = InternalHelpers.GetRemoteAddress(httpRequest, requestHeaders),
                MachineName = InternalHelpers.GetMachineName(httpRequest)
            });

            RequestProperties.CreateOptions propertiesOptions = new RequestProperties.CreateOptions();

            propertiesOptions.ServerVariables = InternalHelpers.ToKeyValuePair(httpRequest.ServerVariables);

            if(httpRequest.Unvalidated != null)
            {
                propertiesOptions.Headers = requestHeaders;
                propertiesOptions.Cookies = InternalHelpers.ToKeyValuePair(httpRequest.Unvalidated.Cookies);
                propertiesOptions.QueryString = InternalHelpers.ToKeyValuePair(httpRequest.Unvalidated.QueryString);

                if(KissLogConfiguration.Options.Handlers.ShouldLogFormData.Invoke(result) == true)
                {
                    propertiesOptions.FormData = InternalHelpers.ToKeyValuePair(httpRequest.Unvalidated.Form);
                }
            }

            if(KissLog.InternalHelpers.CanReadRequestInputStream(propertiesOptions.Headers))
            {
                if(KissLogConfiguration.Options.Handlers.ShouldLogInputStream.Invoke(result) == true)
                {
                    propertiesOptions.InputStream = KissLog.InternalHelpers.WrapInTryCatch(() =>
                    {
                        return InternalHelpers.ReadInputStream(httpRequest);
                    });
                }
            }

            result.SetProperties(new RequestProperties(propertiesOptions));

            return result;
        }
    }
}
