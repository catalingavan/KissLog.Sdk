using KissLog.Apis.v1.Models;
using KissLog.Apis.v1.Requests;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KissLog.Apis.v1.Apis
{
    internal class KissLogRestApiV2 : IKissLogApi
    {
        private readonly IApiClient _apiClient;
        private readonly IKissLogApi _v1Api;
        public KissLogRestApiV2(string baseUrl)
        {
            _apiClient =
                new LogApiClient(
                    new TryCatchApiClient(
                        new ApiClient(baseUrl)
                    )
                );

            _v1Api = new KissLogRestApiV1(baseUrl);
        }

        public async Task<ApiResult<RequestLog>> CreateRequestLogAsync(CreateRequestLogRequest request, IList<File> files = null)
        {
            MultipartFormDataContent content = CreateCreateRequestLogContent(request, files);

            string url = "api/logs/v2.0/createRequestLog";
            return await _apiClient.PostAsync<RequestLog>(url, content).ConfigureAwait(false);
        }

        public ApiResult<RequestLog> CreateRequestLog(CreateRequestLogRequest request, IList<File> files = null)
        {
            MultipartFormDataContent content = CreateCreateRequestLogContent(request, files);

            string url = "api/logs/v2.0/createRequestLog";
            return _apiClient.Post<RequestLog>(url, content);
        }

        private MultipartFormDataContent CreateCreateRequestLogContent(CreateRequestLogRequest request, IList<File> files = null)
        {
            if (request == null)
                return null;

            MultipartFormDataContent form = new MultipartFormDataContent();

            if (request != null)
            {
                form.Add(CreateJsonHttpContent(request), "RequestLog");
            }

            if(files != null)
            {
                foreach (var file in files)
                {
                    if (!System.IO.File.Exists(file.FilePath))
                        continue;

                    form.Add(new ByteArrayContent(System.IO.File.ReadAllBytes(file.FilePath)), "Files", file.FullFileName);
                }
            }

            return form;
        }

        private HttpContent CreateJsonHttpContent(object content)
        {
            HttpContent httpContent = null;

            if (content != null)
            {
                var ms = new System.IO.MemoryStream();

                using (var sw = new System.IO.StreamWriter(ms, new UTF8Encoding(false), 1024, true))
                {
                    using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
                    {
                        var js = new JsonSerializer();
                        js.Serialize(jtw, content);
                        jtw.Flush();
                    }
                }

                ms.Seek(0, System.IO.SeekOrigin.Begin);
                httpContent = new StreamContent(ms);

                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return httpContent;
        }

        public async Task<ApiResult<bool>> UploadRequestLogFilesAsync(UploadFilesRequest request)
        {
            return await _v1Api.UploadRequestLogFilesAsync(request).ConfigureAwait(false);
        }

        public ApiResult<bool> UploadRequestLogFiles(UploadFilesRequest request)
        {
            return _v1Api.UploadRequestLogFiles(request);
        }
    }
}
