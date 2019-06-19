using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;

namespace KissLog.Apis.v1.Apis
{
    internal static class ExtensionMethods
    {
        public static ApiException Create(this ApiException item, HttpResponseMessage response)
        {
            string stringResponse = response.Content.ReadAsStringAsync().Result;
            ApiException result = null;
            try
            {
                result = JsonConvert.DeserializeObject<ApiException>(stringResponse);
            }
            catch { }

            if (result == null)
            {
                result = new ApiException
                {
                    ErrorMessage = "HTTP exception",
                    Description = stringResponse,
                    HttpStatusCode = (int)HttpStatusCode.InternalServerError
                };
            }

            result.HttpStatusCode = (int)response.StatusCode;

            return result;
        }

        public static ApiException Create(this ApiException item, Exception ex)
        {
            ApiException result = new ApiException
            {
                HttpStatusCode = (int)HttpStatusCode.InternalServerError,
                ErrorMessage = ex.Message,
                Description = ex.ToString()
            };

            return result;
        }
    }
}
