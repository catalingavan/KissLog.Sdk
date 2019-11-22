using KissLog.Apis.v1.Apis;
using KissLog.Apis.v1.Models;
using KissLog.Apis.v1.Requests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KissLog.Apis.v1.Flusher
{
    internal class FlusherRestV2 : IFlusher
    {
        private readonly IKissLogApi _kissLogApi;
        public FlusherRestV2(string baseUrl)
        {
            _kissLogApi = new KissLogRestApiV2(baseUrl);
        }

        public async Task FlushAsync(CreateRequestLogRequest request, IList<LoggerFile> files = null)
        {
            IList<File> requestFiles = files == null ? null : files.Select(p => new File
            {
                FileName = p.FileName,
                Extension = p.Extension,
                FullFileName = p.FullFileName,
                FilePath = p.FilePath
            }).ToList();

            try
            {
                ApiResult<RequestLog> requestLog = await _kissLogApi.CreateRequestLogAsync(request, requestFiles).ConfigureAwait(false);
            }
            finally
            {
                DeleteFiles(files);
            }
        }

        public void Flush(CreateRequestLogRequest request, IList<LoggerFile> files = null)
        {
            IList<File> requestFiles = files == null ? null : files.Select(p => new File
            {
                FileName = p.FileName,
                Extension = p.Extension,
                FullFileName = p.FullFileName,
                FilePath = p.FilePath
            }).ToList();

            try
            {
                ApiResult<RequestLog> requestLog = _kissLogApi.CreateRequestLog(request, requestFiles);
            }
            finally
            {
                DeleteFiles(files);
            }
        }

        private static void DeleteFiles(IList<LoggerFile> files)
        {
            if (files == null || !files.Any())
                return;

            foreach (var item in files)
            {
                if (System.IO.File.Exists(item.FilePath))
                {
                    try
                    {
                        System.IO.File.Delete(item.FilePath);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }
    }
}
