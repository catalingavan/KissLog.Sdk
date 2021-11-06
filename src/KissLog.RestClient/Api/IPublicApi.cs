using KissLog.RestClient.Models;
using KissLog.RestClient.Requests.CreateRequestLog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KissLog.RestClient.Api
{
    public interface IPublicApi
    {
        Task<ApiResult<RequestLog>> CreateRequestLogAsync(CreateRequestLogRequest request, IEnumerable<File> files = null);
        ApiResult<RequestLog> CreateRequestLog(CreateRequestLogRequest request, IEnumerable<File> files = null);
    }
}
