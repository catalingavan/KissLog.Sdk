using KissLog.RestClient;
using KissLog.RestClient.Api;
using KissLog.RestClient.Models;
using KissLog.RestClient.Requests.CreateRequestLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KissLog.CloudListeners.RequestLogsListener
{
    internal static class Flusher
    {
        public static async Task FlushAsync(FlushOptions options, IPublicApi kisslogApi, FlushLogArgs flushArgs, CreateRequestLogRequest request)
        {
            IEnumerable<LoggedFile> files = CopyFiles(flushArgs);
            flushArgs.SetFiles(files);

            IEnumerable<File> requestFiles = files.Select(p => new File
            {
                FileName = p.FileName,
                FilePath = p.FilePath
            }).ToList();

            try
            {
                ApiResult<RequestLog> result = null;

                if (options.UseAsync)
                {
                    result = await kisslogApi.CreateRequestLogAsync(request, requestFiles).ConfigureAwait(false);
                }
                else
                {
                    result = kisslogApi.CreateRequestLog(request, requestFiles);
                }

                if (result.HasException && options.OnException != null)
                {
                    options.OnException.Invoke(new ExceptionArgs(flushArgs, result));
                }
            }
            finally
            {
                DeleteFiles(files);
            }
        }

        private static IEnumerable<LoggedFile> CopyFiles(FlushLogArgs args)
        {
            if (args.Files == null || !args.Files.Any())
                return new List<LoggedFile>();

            List<LoggedFile> result = new List<LoggedFile>();

            foreach (var file in args.Files)
            {
                if (!System.IO.File.Exists(file.FilePath))
                    continue;

                TemporaryFile tempFile = new TemporaryFile();
                System.IO.File.Copy(file.FilePath, tempFile.FileName, true);

                result.Add(new LoggedFile(file.FileName, tempFile.FileName, file.FileSize));
            }

            return result;
        }

        private static void DeleteFiles(IEnumerable<LoggedFile> files)
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
                    catch(Exception ex)
                    {
                        InternalLogger.LogException(ex);
                    }
                }
            }
        }
    }
}
