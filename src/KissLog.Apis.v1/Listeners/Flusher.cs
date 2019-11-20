using KissLog.Apis.v1.Apis;
using KissLog.Apis.v1.Models;
using KissLog.Apis.v1.Requests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KissLog.Apis.v1.Listeners
{
    internal static class Flusher
    {
        #region v2

        public static async Task FlushAsync(IKissLogApiV2 kissLogApi, CreateRequestLogRequest request, IList<LoggerFile> files = null)
        {
            IList<File> requestFiles = null;

            if(files != null)
            {
                requestFiles = (files ?? new List<LoggerFile>()).Select(p => new File
                {
                    FileName = p.FileName,
                    Extension = p.Extension,
                    FullFileName = p.FullFileName,
                    FilePath = p.FilePath
                }).ToList();
            }

            try
            {
                ApiResult<RequestLog> requestLog = await kissLogApi.CreateRequestLogAsync(request, requestFiles).ConfigureAwait(false);
            }
            finally
            {
                DeleteFiles(files);
            }
        }

        public static void Flush(IKissLogApiV2 kissLogApi, CreateRequestLogRequest request, IList<LoggerFile> files = null)
        {
            IList<File> requestFiles = null;

            if (files != null)
            {
                requestFiles = (files ?? new List<LoggerFile>()).Select(p => new File
                {
                    FileName = p.FileName,
                    Extension = p.Extension,
                    FullFileName = p.FullFileName,
                    FilePath = p.FilePath
                }).ToList();
            }

            try
            {
                ApiResult<RequestLog> requestLog = kissLogApi.CreateRequestLog(request, requestFiles);
            }
            finally
            {
                DeleteFiles(files);
            }
        }

        #endregion

        #region v1

        public static async Task FlushAsync(IKissLogApi kissLogApi, CreateRequestLogRequest request, IList<LoggerFile> files = null)
        {
            ApiResult<RequestLog> requestLog = await kissLogApi.CreateRequestLogAsync(request).ConfigureAwait(false);

            if(files?.Any() == true)
            {
                string requestLogId = requestLog?.Result?.Id;
                await UploadFilesAsync(kissLogApi, requestLogId, request, files).ConfigureAwait(false);
            }
        }

        public static void Flush(IKissLogApi kissLogApi, CreateRequestLogRequest request, IList<LoggerFile> files = null)
        {
            ApiResult<RequestLog> requestLog = kissLogApi.CreateRequestLog(request);

            if (files?.Any() == true)
            {
                string requestLogId = requestLog?.Result?.Id;
                UploadFiles(kissLogApi, requestLogId, request, files);
            }
        }

        private static async Task UploadFilesAsync(IKissLogApi kissLogApi, string requestLogId, CreateRequestLogRequest request, IList<LoggerFile> files)
        {
            if (files == null || !files.Any())
                return;

            try
            {
                if (string.IsNullOrEmpty(requestLogId))
                    return;

                IList<File> requestFiles = files.Select(p => new File
                {
                    FileName = p.FileName,
                    Extension = p.Extension,
                    FullFileName = p.FullFileName,
                    FilePath = p.FilePath
                })
                .ToList();

                UploadFilesRequest uploadRequest = new UploadFilesRequest
                {
                    OrganizationId = request.OrganizationId,
                    ApplicationId = request.ApplicationId,
                    RequestLogId = requestLogId,
                    RequestLogClientId = request.ClientId,
                    HttpStatusCode = request.WebRequest?.Response?.HttpStatusCode ?? 500,
                    Files = requestFiles
                };

                await kissLogApi.UploadFilesAsync(uploadRequest).ConfigureAwait(false);
            }
            finally
            {
                DeleteFiles(files);
            }
        }

        private static void UploadFiles(IKissLogApi kissLogApi, string requestLogId, CreateRequestLogRequest request, IList<LoggerFile> files)
        {
            if (files == null || !files.Any())
                return;

            try
            {
                if (string.IsNullOrEmpty(requestLogId))
                    return;

                IList<File> requestFiles = files.Select(p => new File
                {
                    FileName = p.FileName,
                    Extension = p.Extension,
                    FullFileName = p.FullFileName,
                    FilePath = p.FilePath
                })
                .ToList();

                UploadFilesRequest uploadRequest = new UploadFilesRequest
                {
                    OrganizationId = request.OrganizationId,
                    ApplicationId = request.ApplicationId,
                    RequestLogId = requestLogId,
                    RequestLogClientId = request.ClientId,
                    HttpStatusCode = request.WebRequest?.Response?.HttpStatusCode ?? 500,
                    Files = requestFiles
                };

                kissLogApi.UploadFiles(uploadRequest);
            }
            finally
            {
                DeleteFiles(files);
            }
        }

        #endregion

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
