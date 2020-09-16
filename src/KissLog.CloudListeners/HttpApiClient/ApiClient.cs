using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KissLog.CloudListeners.HttpApiClient
{
    internal class ApiClient : IApiClient
    {
        private static HttpClient httpClient = new HttpClient();

        private readonly string _baseUrl;
        public ApiClient(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public async Task<ApiResult<T>> PostAsJsonAsync<T>(string resource, object request)
        {
            Uri uri = BuildRequestUri(resource);

            using (HttpContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"))
            {
                using (HttpResponseMessage response = await httpClient.PostAsync(uri, content).ConfigureAwait(false))
                {
                    return ReadResult<T>(response);
                }
            }
        }
        public ApiResult<T> PostAsJson<T>(string resource, object request)
        {
            Uri uri = BuildRequestUri(resource);

            using (HttpContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"))
            {
                using (HttpResponseMessage response = httpClient.PostAsync(uri, content).Result)
                {
                    return ReadResult<T>(response);
                }
            }
        }

        public async Task<ApiResult<T>> PostAsync<T>(string resource, HttpContent content)
        {
            Uri uri = BuildRequestUri(resource);

            using (content)
            {
                using (HttpResponseMessage response = await httpClient.PostAsync(uri, content).ConfigureAwait(false))
                {
                    return ReadResult<T>(response);
                }
            }
        }
        public ApiResult<T> Post<T>(string resource, HttpContent content)
        {
            Uri uri = BuildRequestUri(resource);

            using (content)
            {
                using (HttpResponseMessage response = httpClient.PostAsync(uri, content).Result)
                {
                    return ReadResult<T>(response);
                }
            }
        }

        public Uri BuildRequestUri(string resource)
        {
            resource = CombineUriParts(_baseUrl, resource);
            return new Uri(resource, UriKind.RelativeOrAbsolute);
        }

        // http://stackoverflow.com/a/6704287
        string CombineUriParts(params string[] uriParts)
        {
            var uri = string.Empty;
            if (uriParts != null && uriParts.Any())
            {
                uriParts = uriParts.Where(part => !string.IsNullOrWhiteSpace(part)).ToArray();
                char[] trimChars = { '\\', '/' };
                uri = (uriParts[0] ?? string.Empty).TrimEnd(trimChars);
                for (var i = 1; i < uriParts.Length; i++)
                {
                    uri = $"{uri.TrimEnd(trimChars)}/{(uriParts[i] ?? string.Empty).TrimStart(trimChars)}";
                }
            }
            return uri;
        }

        private ApiResult<T> ReadResult<T>(HttpResponseMessage response)
        {
            ApiResult<T> result = new ApiResult<T>();

            if (response.IsSuccessStatusCode == false)
            {
                result.Exception = (null as ApiException).Create(response);
            }
            else
            {
                string stringResponse = response.Content.ReadAsStringAsync().Result;
                if(!string.IsNullOrEmpty(stringResponse))
                {
                    result.Result = JsonConvert.DeserializeObject<T>(stringResponse);
                }
            }

            return result;
        }
    }
}
