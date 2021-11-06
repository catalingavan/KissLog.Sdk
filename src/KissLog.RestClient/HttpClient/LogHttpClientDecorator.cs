using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KissLog.RestClient.HttpClient
{
    internal class LogHttpClientDecorator : IHttpClient
    {
        private readonly IHttpClient _decorated;
        public LogHttpClientDecorator(IHttpClient decorated)
        {
            _decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
        }

        public ApiResult<T> Post<T>(Uri uri, HttpContent content)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Log(HttpMethod.Post.Method, uri, content);

            ApiResult<T> result = _decorated.Post<T>(uri, content);

            Log(HttpMethod.Post.Method, uri, result, sw);

            return result;
        }

        public async Task<ApiResult<T>> PostAsync<T>(Uri uri, HttpContent content)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Log(HttpMethod.Post.Method, uri, content);

            ApiResult<T> result = await _decorated.PostAsync<T>(uri, content);

            Log(HttpMethod.Post.Method, uri, result, sw);

            return result;
        }

        private void Log(string httpMethod, Uri uri, HttpContent content)
        {
            InternalLogger.Log($"HTTP \"{httpMethod.ToUpperInvariant()} {uri}\" executing", LogLevel.Debug);

            if (content != null)
            {
                string contentAsString = content.ReadAsStringAsync().Result;

                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"HTTP \"{httpMethod.ToUpperInvariant()} {uri}\" Content:");
                sb.Append(contentAsString);

                InternalLogger.Log(sb.ToString(), LogLevel.Debug);
            }
        }

        private void Log<T>(string httpMethod, Uri uri, ApiResult<T> result, Stopwatch sw)
        {
            sw.Stop();

            InternalLogger.Log($"HTTP \"{httpMethod.ToUpperInvariant()} {uri}\" executed. Duration:{sw.ElapsedMilliseconds} StatusCode:{result.StatusCode}", LogLevel.Debug);

            if (result.ResponseContent != null)
            {
                InternalLogger.Log($"HTTP \"{httpMethod.ToUpperInvariant()} {uri}\" Response: {result.ResponseContent}", LogLevel.Debug);
            }
        }
    }
}
