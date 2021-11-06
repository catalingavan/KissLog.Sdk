using KissLog.RestClient.HttpClient;
using KissLog.RestClient.Models;
using KissLog.RestClient.Requests.CreateRequestLog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KissLog.RestClient.Api
{
    public class PublicRestApi : IPublicApi
    {
        private readonly IHttpClient _httpClient;
        private readonly string _baseUrl;
        public PublicRestApi(string baseUrl, bool ignoreSslCertificate = false)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentNullException(nameof(baseUrl));

            if (Uri.TryCreate(baseUrl, UriKind.Absolute, out _) == false)
                throw new ArgumentException($"{nameof(baseUrl)} is not a valid URL");

            _baseUrl = baseUrl;

            _httpClient = new LogHttpClientDecorator(
                new TryCatchHttpClientDecorator(new DefaultHttpClient(ignoreSslCertificate))
            );
        }

        public ApiResult<RequestLog> CreateRequestLog(CreateRequestLogRequest request, IEnumerable<File> files = null)
        {
            Uri url = Helpers.BuildUri(_baseUrl, "api/public/v1.0/createRequestLog");
            MultipartFormDataContent content = CreateMultipartFormDataContent(request, files);

            return _httpClient.Post<RequestLog>(url, content);
        }

        public async Task<ApiResult<RequestLog>> CreateRequestLogAsync(CreateRequestLogRequest request, IEnumerable<File> files = null)
        {
            Uri url = Helpers.BuildUri(_baseUrl, "api/public/v1.0/createRequestLog");
            MultipartFormDataContent content = CreateMultipartFormDataContent(request, files);

            return await _httpClient.PostAsync<RequestLog>(url, content).ConfigureAwait(false);
        }

        private MultipartFormDataContent CreateMultipartFormDataContent(CreateRequestLogRequest request, IEnumerable<File> files = null)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            MultipartFormDataContent form = new MultipartFormDataContent();

            if (request != null)
            {
                string json = KissLogConfiguration.JsonSerializer.Serialize(request);

                form.Add(new StringContent(json, Encoding.UTF8, "application/json"), "RequestLog");
            }

            if (files != null)
            {
                foreach (var file in files)
                {
                    if (!System.IO.File.Exists(file.FilePath))
                        continue;

                    form.Add(new ByteArrayContent(System.IO.File.ReadAllBytes(file.FilePath)), "Files", file.FileName);
                }
            }

            return form;
        }
    }
}
