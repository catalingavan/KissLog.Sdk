using System;

namespace KissLog.CloudListeners.HttpApiClient
{
    internal class ApiResult<T>
    {
        public T Result { get; set; }

        public ApiException Exception { get; set; }

        public bool HasException
        {
            get { return Exception != null; }
        }

        public void EnsureSuccess()
        {
            if (Exception != null)
            {
                throw new Exception(Exception.ErrorMessage);
            }
        }
    }
}
