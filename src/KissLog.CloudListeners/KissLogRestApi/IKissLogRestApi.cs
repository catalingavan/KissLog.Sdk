using KissLog.CloudListeners.HttpApiClient;
using KissLog.CloudListeners.KissLogRestApi.Payload.AppendText;
using KissLog.CloudListeners.KissLogRestApi.Payload.CreateRequestLog;
using KissLog.CloudListeners.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KissLog.CloudListeners.KissLogRestApi
{
    internal interface IKissLogRestApi
    {
        Task<ApiResult<RequestLog>> CreateRequestLogAsync(CreateRequestLogRequest request, IList<File> files = null);
        ApiResult<RequestLog> CreateRequestLog(CreateRequestLogRequest request, IList<File> files = null);

        Task<ApiResult<bool>> AppendTextAsync(AppendTextRequest request);
        ApiResult<bool> AppendText(AppendTextRequest request);
    }
}
