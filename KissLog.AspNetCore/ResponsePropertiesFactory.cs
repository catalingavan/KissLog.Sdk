using KissLog.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace KissLog.AspNetCore
{
    internal static class ResponsePropertiesFactory
    {
        public static ResponseProperties Create(HttpResponse response)
        {
            ResponseProperties result = new ResponseProperties();

            if (response == null)
                return result;

            foreach (string key in response.Headers.Keys)
            {
                StringValues values;
                response.Headers.TryGetValue(key, out values);

                string value = values.ToString();

                result.Headers.Add(
                    new KeyValuePair<string, string>(key, value)
                );
            }

            return result;
        }
    }
}
