using KissLog.CloudListeners.KissLogRestApi;
using KissLog.CloudListeners.Models;
using KissLog.CloudListeners.KissLogRestApi.Payload.CreateRequestLog;
using KissLog.CloudListeners.HttpApiClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KissLog.CloudListeners.RequestLogsListener
{
    internal static class Flusher
    {
        public static void Flush(IKissLogRestApi apiClient, CreateRequestLogRequest request, IList<LoggerFile> files = null)
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
                ApiResult<RequestLog> requestLog = apiClient.CreateRequestLog(request, requestFiles);
            }
            finally
            {
                DeleteFiles(files);
            }
        }

        public static async Task FlushAsync(IKissLogRestApi apiClient, CreateRequestLogRequest request, IList<LoggerFile> files = null)
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
                ApiResult<RequestLog> requestLog = await apiClient.CreateRequestLogAsync(request, requestFiles).ConfigureAwait(false);
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
