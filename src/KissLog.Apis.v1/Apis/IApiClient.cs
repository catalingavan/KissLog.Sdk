using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace KissLog.Apis.v1.Apis
{
    internal interface IApiClient
    {
        Uri BuildRequestUri(string resource);

        Task<ApiResult<T>> PostAsJsonAsync<T>(string resource, object request);
        ApiResult<T> PostAsJson<T>(string resource, object request);

        Task<ApiResult<T>> PostAsync<T>(string resource, HttpContent content);
        ApiResult<T> Post<T>(string resource, HttpContent content);
    }
}
