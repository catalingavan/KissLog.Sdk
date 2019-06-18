using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace KissLog.Apis.v1.Apis
{
    internal class TryCatchApiClient : IApiClient
    {
        private readonly IApiClient _decorated;
        public TryCatchApiClient(IApiClient decorated)
        {
            _decorated = decorated;
        }

        public async Task<ApiResult<T>> PostAsJsonAsync<T>(string resource, object request)
        {
            ApiResult<T> result = new ApiResult<T>();

            try
            {
                result = await _decorated.PostAsJsonAsync<T>(resource, request);
            }
            catch (Exception ex)
            {
                result = new ApiResult<T>
                {
                    Exception = new ApiException().Create(ex)
                };
            }

            return result;
        }
        public ApiResult<T> PostAsJson<T>(string resource, object request)
        {
            ApiResult<T> result = new ApiResult<T>();

            try
            {
                result = _decorated.PostAsJson<T>(resource, request);
            }
            catch(Exception ex)
            {
                result = new ApiResult<T>
                {
                    Exception = new ApiException().Create(ex)
                };
            }

            return result;
        }

        public async Task<ApiResult<T>> PostAsync<T>(string resource, HttpContent content)
        {
            ApiResult<T> result = new ApiResult<T>();

            try
            {
                result = await _decorated.PostAsync<T>(resource, content);
            }
            catch (Exception ex)
            {
                result = new ApiResult<T>
                {
                    Exception = new ApiException().Create(ex)
                };
            }

            return result;
        }
        public ApiResult<T> Post<T>(string resource, HttpContent content)
        {
            ApiResult<T> result = new ApiResult<T>();

            try
            {
                result = _decorated.Post<T>(resource, content);
            }
            catch (Exception ex)
            {
                result = new ApiResult<T>
                {
                    Exception = new ApiException().Create(ex)
                };
            }

            return result;
        }
    }
}
