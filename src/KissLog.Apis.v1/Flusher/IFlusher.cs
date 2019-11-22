using KissLog.Apis.v1.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KissLog.Apis.v1.Flusher
{
    internal interface IFlusher
    {
        Task FlushAsync(CreateRequestLogRequest request, IList<LoggerFile> files = null);
        void Flush(CreateRequestLogRequest request, IList<LoggerFile> files = null);
    }
}
