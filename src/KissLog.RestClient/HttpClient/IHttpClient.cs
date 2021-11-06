using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace KissLog.RestClient.HttpClient
{
    internal interface IHttpClient
    {
        Task<ApiResult<T>> PostAsync<T>(Uri uri, HttpContent content);
        ApiResult<T> Post<T>(Uri uri, HttpContent content);
    }
}
