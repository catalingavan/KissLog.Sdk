using KissLog.Apis.v1.Models;
using KissLog.Apis.v1.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KissLog.Apis.v1.Apis
{
    internal interface IKissLogApi
    {
        Task<ApiResult<RequestLog>> CreateRequestLogAsync(CreateRequestLogRequest request);
        ApiResult<RequestLog> CreateRequestLog(CreateRequestLogRequest request);

        Task<ApiResult<bool>> UploadFilesAsync(UploadFilesRequest request);
        ApiResult<bool> UploadFiles(UploadFilesRequest request);

        Task<ApiResult<RequestLog>> CreateRequestLogV2Async(CreateRequestLogRequest request, IList<File> files = null);
    }
}
