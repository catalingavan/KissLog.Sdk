using KissLog.Apis.v1.Apis;
using KissLog.Apis.v1.Models;
using KissLog.Apis.v1.Requests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KissLog.Apis.v1.Flusher
{
    internal class FlusherRestV1 : IFlusher
    {
        private readonly IKissLogApi _kissLogApi;
        public FlusherRestV1(string baseUrl)
        {
            _kissLogApi = new KissLogRestApiV1(baseUrl);
        }

        public async Task FlushAsync(CreateRequestLogRequest request, IList<LoggerFile> files = null)
        {
            try
            {
                ApiResult<RequestLog> requestLog = await _kissLogApi.CreateRequestLogAsync(request).ConfigureAwait(false);

                if(requestLog.HasException == false && files?.Any() == true)
                {
                    string requestLogId = requestLog?.Result?.Id;

                    await UploadRequestLogFilesAsync(requestLogId, request, files);
                }
            }
            finally
            {
                DeleteFiles(files);
            }
        }

        public void Flush(CreateRequestLogRequest request, IList<LoggerFile> files = null)
        {
            try
            {
                ApiResult<RequestLog> requestLog = _kissLogApi.CreateRequestLog(request);

                if (requestLog.HasException == false && files?.Any() == true)
                {
                    string requestLogId = requestLog?.Result?.Id;

                    UploadRequestLogFiles(requestLogId, request, files);
                }
            }
            finally
            {
                DeleteFiles(files);
            }
        }

        private async Task UploadRequestLogFilesAsync(string requestLogId, CreateRequestLogRequest request, IList<LoggerFile> files)
        {
            if (files == null || !files.Any())
                return;

            if (string.IsNullOrEmpty(requestLogId))
                return;

            IList<File> requestFiles = files.Select(p => new File
            {
                FileName = p.FileName,
                Extension = p.Extension,
                FullFileName = p.FullFileName,
                FilePath = p.FilePath
            }).ToList();

            UploadFilesRequest uploadRequest = new UploadFilesRequest
            {
                OrganizationId = request.OrganizationId,
                ApplicationId = request.ApplicationId,
                RequestLogId = requestLogId,
                RequestLogClientId = request.ClientId,
                HttpStatusCode = request.WebRequest?.Response?.HttpStatusCode ?? 500,
                Files = requestFiles
            };

            await _kissLogApi.UploadRequestLogFilesAsync(uploadRequest).ConfigureAwait(false);
        }

        private void UploadRequestLogFiles(string requestLogId, CreateRequestLogRequest request, IList<LoggerFile> files)
        {
            if (files == null || !files.Any())
                return;

            if (string.IsNullOrEmpty(requestLogId))
                return;

            IList<File> requestFiles = files.Select(p => new File
            {
                FileName = p.FileName,
                Extension = p.Extension,
                FullFileName = p.FullFileName,
                FilePath = p.FilePath
            }).ToList();

            UploadFilesRequest uploadRequest = new UploadFilesRequest
            {
                OrganizationId = request.OrganizationId,
                ApplicationId = request.ApplicationId,
                RequestLogId = requestLogId,
                RequestLogClientId = request.ClientId,
                HttpStatusCode = request.WebRequest?.Response?.HttpStatusCode ?? 500,
                Files = requestFiles
            };

            _kissLogApi.UploadRequestLogFiles(uploadRequest);
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
