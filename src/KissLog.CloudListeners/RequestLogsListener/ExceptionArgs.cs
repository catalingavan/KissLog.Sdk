using KissLog.RestClient;
using System;

namespace KissLog.CloudListeners.RequestLogsListener
{
    public class ExceptionArgs
    {
        public FlushLogArgs FlushArgs { get; }
        public ApiResult ApiResult { get; }

        public ExceptionArgs(FlushLogArgs flushArgs, ApiResult apiResult)
        {
            FlushArgs = flushArgs ?? throw new ArgumentNullException(nameof(flushArgs));
            ApiResult = apiResult ?? throw new ArgumentNullException(nameof(apiResult));
        }
    }
}
