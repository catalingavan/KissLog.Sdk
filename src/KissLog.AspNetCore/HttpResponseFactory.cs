using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Net;

namespace KissLog.AspNetCore
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

            KissLog.Web.ResponseProperties properties = new KissLog.Web.ResponseProperties();
            result.Properties = properties;

            foreach (string key in response.Headers.Keys)
            {
                StringValues values;
                response.Headers.TryGetValue(key, out values);

                string value = values.ToString();

                properties.Headers.Add(
                    new KeyValuePair<string, string>(key, value)
                );
            }

            return result;
        }
    }
}
