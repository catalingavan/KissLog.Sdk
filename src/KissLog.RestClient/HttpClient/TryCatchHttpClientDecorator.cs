using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace KissLog.RestClient.HttpClient
{
    internal class TryCatchHttpClientDecorator : IHttpClient
    {
        private readonly IHttpClient _decorated;
        public TryCatchHttpClientDecorator(IHttpClient decorated)
        {
            _decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
        }

        public async Task<ApiResult<T>> PostAsync<T>(Uri uri, HttpContent content)
        {
            ApiResult<T> result;

            try
            {
                result = await _decorated.PostAsync<T>(uri, content).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                InternalLogger.LogException(ex);

                result = new ApiResult<T>
                {
                    Exception = ApiException.Create(ex)
                };
            }

            return result;
        }

        public ApiResult<T> Post<T>(Uri uri, HttpContent content)
        {
            ApiResult<T> result;

            try
            {
                result = _decorated.Post<T>(uri, content);
            }
            catch (Exception ex)
            {
                InternalLogger.LogException(ex);

                result = new ApiResult<T>
                {
                    Exception = ApiException.Create(ex)
                };
            }

            return result;
        }
    }
}
