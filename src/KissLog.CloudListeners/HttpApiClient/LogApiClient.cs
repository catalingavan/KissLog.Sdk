using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KissLog.CloudListeners.HttpApiClient
{
    internal class LogApiClient : IApiClient
    {
        private readonly IApiClient _decorated;
        public LogApiClient(IApiClient decorated)
        {
            _decorated = decorated;
        }

        public Uri BuildRequestUri(string resource)
        {
            return _decorated.BuildRequestUri(resource);
        }

        public ApiResult<T> Post<T>(string resource, HttpContent content)
        {
            Uri uri = BuildRequestUri(resource);
            LogBegin("post", uri);

            var result = _decorated.Post<T>(resource, content);

            LogComplete("post", uri, result);

            return result;
        }

        public ApiResult<T> PostAsJson<T>(string resource, object request)
        {
            Uri uri = BuildRequestUri(resource);
            LogBegin("post", uri);

            var result = _decorated.PostAsJson<T>(resource, request);

            LogComplete("post", uri, result);

            return result;
        }

        public async Task<ApiResult<T>> PostAsJsonAsync<T>(string resource, object request)
        {
            Uri uri = BuildRequestUri(resource);
            LogBegin("post", uri);

            var result = await _decorated.PostAsJsonAsync<T>(resource, request).ConfigureAwait(false);

            LogComplete("post", uri, result);

            return result;
        }

        public async Task<ApiResult<T>> PostAsync<T>(string resource, HttpContent content)
        {
            Uri uri = BuildRequestUri(resource);
            LogBegin("post", uri);

            var result = await _decorated.PostAsync<T>(resource, content).ConfigureAwait(false);

            LogComplete("post", uri, result);

            return result;
        }

        private void LogBegin(string httpMethod, Uri uri)
        {
            KissLog.Internal.InternalHelpers.Log($"{httpMethod.ToUpperInvariant()} {uri} begin", LogLevel.Trace);
        }

        private void LogComplete<T>(string httpMethod, Uri uri, ApiResult<T> result)
        {
            if (result == null)
            {
                KissLog.Internal.InternalHelpers.Log($"{httpMethod.ToUpperInvariant()} {uri} complete - no result", LogLevel.Trace);
                return;
            }

            if (result.HasException == false)
            {
                KissLog.Internal.InternalHelpers.Log($"{httpMethod.ToUpperInvariant()} {uri} OK", LogLevel.Trace);
                return;
            }

            StringBuilder sb = new StringBuilder();

            sb.Append($"{httpMethod.ToUpperInvariant()} {uri} {result.Exception.HttpStatusCode} ERROR");

            if (!string.IsNullOrEmpty(result.Exception.ErrorMessage))
            {
                sb.AppendLine();
                sb.Append(new string(' ', 5) + result.Exception.ErrorMessage);
            }

            if (!string.IsNullOrEmpty(result.Exception.Description))
            {
                sb.AppendLine();
                sb.Append(new string(' ', 5) + result.Exception.Description);
            }

            KissLog.Internal.InternalHelpers.Log(sb.ToString(), LogLevel.Error);
        }
    }
}
