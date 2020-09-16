using KissLog.CloudListeners.HttpApiClient;
using KissLog.CloudListeners.KissLogRestApi.Payload.AppendText;
using KissLog.CloudListeners.KissLogRestApi.Payload.CreateRequestLog;
using KissLog.CloudListeners.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KissLog.CloudListeners.KissLogRestApi
{
    internal class KissLogRestApiV1Client : IKissLogRestApi
    {
        private readonly IApiClient _apiClient;
        public KissLogRestApiV1Client(string baseUrl)
        {
            _apiClient =
                new LogApiClient(
                    new TryCatchApiClient(
                        new ApiClient(baseUrl)
                    )
                );
        }

        public async Task<ApiResult<RequestLog>> CreateRequestLogAsync(CreateRequestLogRequest request, IList<File> files = null)
        {
            MultipartFormDataContent content = CreateCreateRequestLogContent(request, files);

            string url = "api/public/v1.0/createRequestLog";
            return await _apiClient.PostAsync<RequestLog>(url, content).ConfigureAwait(false);
        }

        public ApiResult<RequestLog> CreateRequestLog(CreateRequestLogRequest request, IList<File> files = null)
        {
            MultipartFormDataContent content = CreateCreateRequestLogContent(request, files);

            string url = "api/public/v1.0/createRequestLog";
            return _apiClient.Post<RequestLog>(url, content);
        }

        public async Task<ApiResult<bool>> AppendTextAsync(AppendTextRequest request)
        {
            string url = "api/public/v1.0/appendText";
            return await _apiClient.PostAsJsonAsync<bool>(url, request).ConfigureAwait(false);
        }

        public ApiResult<bool> AppendText(AppendTextRequest request)
        {
            string url = "api/public/v1.0/appendText";
            return _apiClient.PostAsJson<bool>(url, request);
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

            if (files != null)
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
