using KissLog.Apis.v1.Models;
using KissLog.Apis.v1.Requests;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KissLog.Apis.v1.Apis
{
    internal class KissLogRestApi : IKissLogApi
    {
        private readonly IApiClient _apiClient;
        public KissLogRestApi(string baseUrl)
        {
            _apiClient =
                new LogApiClient(
                    new TryCatchApiClient(
                        new ApiClient(baseUrl)
                    )
                );
        }

        public async Task<ApiResult<RequestLog>> CreateRequestLogAsync(CreateRequestLogRequest request)
        {
            string url = "api/logs/v1.0/createRequestLog";
            return await _apiClient.PostAsJsonAsync<RequestLog>(url, request).ConfigureAwait(false);
        }

        public ApiResult<RequestLog> CreateRequestLog(CreateRequestLogRequest request)
        {
            string url = "api/logs/v1.0/createRequestLog";
            return _apiClient.PostAsJson<RequestLog>(url, request);
        }

        public async Task<ApiResult<bool>> UploadFilesAsync(UploadFilesRequest request)
        {
            MultipartFormDataContent content = CreateUploadFilesContent(request);

            string url = "api/logs/v1.0/uploadRequestLogFiles";
            return await _apiClient.PostAsync<bool>(url, content).ConfigureAwait(false);
        }

        public ApiResult<bool> UploadFiles(UploadFilesRequest request)
        {
            MultipartFormDataContent content = CreateUploadFilesContent(request);

            string url = "api/logs/v1.0/uploadRequestLogFiles";
            return _apiClient.Post<bool>(url, content);
        }

        private MultipartFormDataContent CreateUploadFilesContent(UploadFilesRequest request)
        {
            MultipartFormDataContent form = new MultipartFormDataContent();

            if (request == null || request.Files == null || !request.Files.Any())
                return form;

            form.Add(new StringContent(request.OrganizationId), nameof(UploadFilesRequest.OrganizationId));
            form.Add(new StringContent(request.ApplicationId), nameof(UploadFilesRequest.ApplicationId));
            form.Add(new StringContent(request.RequestLogId), nameof(UploadFilesRequest.RequestLogId));
            form.Add(new StringContent(request.RequestLogClientId), nameof(UploadFilesRequest.RequestLogClientId));
            form.Add(new StringContent(request.HttpStatusCode.ToString()), nameof(UploadFilesRequest.HttpStatusCode));

            foreach (var file in request.Files)
            {
                if (!System.IO.File.Exists(file.FilePath))
                    continue;

                form.Add(new ByteArrayContent(System.IO.File.ReadAllBytes(file.FilePath)), nameof(UploadFilesRequest.Files), file.FullFileName);
            }

            return form;
        }

        public async Task<ApiResult<RequestLog>> CreateRequestLogV2Async(CreateRequestLogRequest request, IList<File> files = null)
        {
            MultipartFormDataContent content = CreateCreateRequestLogV2Content(request, files);

            string url = "api/logs/v1.0/createRequestLog/v2";
            return await _apiClient.PostAsync<RequestLog>(url, content).ConfigureAwait(false);
        }

        private MultipartFormDataContent CreateCreateRequestLogV2Content(CreateRequestLogRequest request, IList<File> files = null)
        {
            if (request == null)
                return null;

            MultipartFormDataContent form = new MultipartFormDataContent();

            request.SdkName = null;

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
    }
}
