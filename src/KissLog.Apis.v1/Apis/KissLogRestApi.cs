using KissLog.Apis.v1.Models;
using KissLog.Apis.v1.Requests;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace KissLog.Apis.v1.Apis
{
    internal class KissLogRestApi : IKissLogApi
    {
        private readonly IApiClient _apiClient;
        public KissLogRestApi(string baseUrl)
        {
            _apiClient = new TryCatchApiClient(
                new ApiClient(baseUrl)
            );
        }

        public async Task<ApiResult<RequestLog>> CreateRequestLogAsync(CreateRequestLogRequest request)
        {
            var test = JsonConvert.SerializeObject(request, Formatting.Indented);

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
    }
}
