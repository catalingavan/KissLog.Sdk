using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace KissLog.RestClient.HttpClient
{
    internal class DefaultHttpClient : IHttpClient
    {
        private readonly System.Net.Http.HttpClient _httpClient;
        public DefaultHttpClient(bool ignoreSslCertificate)
        {
            _httpClient = HttpClientFactory.Create(ignoreSslCertificate);
        }

        public ApiResult<T> Post<T>(Uri uri, HttpContent content)
        {
            using (content)
            {
                using (HttpResponseMessage response = _httpClient.PostAsync(uri, content).Result)
                {
                    return ReadResult<T>(response);
                }
            }
        }

        public async Task<ApiResult<T>> PostAsync<T>(Uri uri, HttpContent content)
        {
            using (content)
            {
                using (HttpResponseMessage response = await _httpClient.PostAsync(uri, content).ConfigureAwait(false))
                {
                    return ReadResult<T>(response);
                }
            }
        }

        private ApiResult<T> ReadResult<T>(HttpResponseMessage response)
        {
            string stringResponse = response.Content.ReadAsStringAsync().Result;

            ApiResult<T> result = new ApiResult<T>
            {
                ResponseContent = stringResponse,
                StatusCode = (int)response.StatusCode
            };

            if (response.IsSuccessStatusCode == false)
            {
                result.Exception = ApiException.Create(stringResponse);
            }
            else
            {
                if (!string.IsNullOrEmpty(stringResponse))
                {
                    result.Result = KissLogConfiguration.JsonSerializer.Deserialize<T>(stringResponse);
                }
            }

            return result;
        }
    }
}
