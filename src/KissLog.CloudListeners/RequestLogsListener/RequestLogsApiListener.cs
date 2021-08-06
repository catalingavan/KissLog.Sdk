using KissLog.CloudListeners.Auth;
using KissLog.CloudListeners.HttpApiClient;
using KissLog.CloudListeners.KissLogRestApi;
using KissLog.CloudListeners.KissLogRestApi.Payload.CreateRequestLog;
using KissLog.CloudListeners.Models;
using KissLog.FlushArgs;
using KissLog.Internal;
using KissLog.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KissLog.CloudListeners.RequestLogsListener
{
    public class RequestLogsApiListener : ILogListener
    {
        public ObfuscateArgsService ObfuscateService { get; } = new ObfuscateArgsService();
        public TruncateArgsService TruncateService { get; } = new TruncateArgsService();
        public GenerateKeywordsService GenerateKeywordsService { get; } = new GenerateKeywordsService();

        public bool UseAsync { get; set; } = true;
        public string ApiUrl { get; set; } = Defaults.KissLogNetUrl;

        public Func<FlushLogArgs, FlushProperties> UpdateFlushProperties = (flushArgs) => null;

        public int MinimumResponseHttpStatusCode { get; set; } = 0;
        public LogLevel MinimumLogMessageLevel { get; set; } = LogLevel.Trace;
        public LogListenerParser Parser { get; set; } = new LogListenerParser();

        private readonly Application _application;

        public RequestLogsApiListener(Application application)
        {
            if (application == null)
                throw new ArgumentNullException(nameof(application));

            _application = application;
        }

        public void OnBeginRequest(HttpRequest httpRequest, ILogger logger)
        {
            // Do nothing
            // RequestLogsApiListener saves the logs only at the end of the request
        }

        public void OnMessage(LogMessage message, ILogger logger)
        {
            // Do nothing
            // RequestLogsApiListener saves the logs only at the end of the request
        }

        public void OnFlush(FlushLogArgs args, ILogger logger)
        {
            FlushProperties flushProperties = GetAndValidateFlushProperties(args);
            if (flushProperties == null)
                return;

            InternalHelpers.Log("RequestLogsApiListener: OnFlush begin", LogLevel.Trace);

            ObfuscateService?.Obfuscate(args);
            TruncateService?.Truncate(args);

            CreateRequestLogRequest request = CreateRequestLogRequestFactory.Create(args);
            request.OrganizationId = flushProperties.Application.OrganizationId;
            request.ApplicationId = flushProperties.Application.ApplicationId;
            request.Keywords = GenerateKeywords(args);

            // we need to copy files, because we start a new Thread, and the existing files will be deleted before accessing them
            IList<LoggerFile> copy = CopyFiles(args.Files?.ToList());

            Action<ApiException> exceptionHandler = (ApiException ex) =>
            {
                ExceptionArgs exceptionArgs = new ExceptionArgs
                {
                    FlushArgs = args,
                    Payload = JsonConvert.SerializeObject(request),
                    Files = copy,
                    Exception = string.IsNullOrEmpty(ex.Description) ? ex.ErrorMessage : ex.Description,
                    HttpStatusCode = ex.HttpStatusCode,
                };

                ConfigurationOptions.ApplyOnRequestLogsApiListenerException(exceptionArgs);
            };

            IKissLogRestApi kissLogRestApi = new KissLogRestApiV1Client(flushProperties.ApiUrl);

            if (UseAsync == true)
            {
                Flusher.FlushAsync(kissLogRestApi, request, copy, exceptionHandler).ConfigureAwait(false);
            }
            else
            {
                Flusher.Flush(kissLogRestApi, request, copy, exceptionHandler);
            }

            InternalHelpers.Log("RequestLogsApiListener: OnFlush complete", LogLevel.Trace);
        }

        private List<LoggerFile> CopyFiles(IList<LoggerFile> source)
        {
            List<LoggerFile> files = new List<LoggerFile>();
            if (source == null || !source.Any())
                return files;

            foreach (var file in source)
            {
                if (!System.IO.File.Exists(file.FilePath))
                    continue;

                TemporaryFile tempFile = new TemporaryFile();
                System.IO.File.Copy(file.FilePath, tempFile.FileName, true);

                files.Add(new LoggerFile(tempFile.FileName, file.FullFileName));
            }

            return files;
        }

        private FlushProperties GetAndValidateFlushProperties(FlushLogArgs args)
        {
            FlushProperties result = null;

            if (UpdateFlushProperties != null)
            {
                result = UpdateFlushProperties(args);
            }

            if (result == null)
            {
                result = new FlushProperties
                {
                    Application = _application,
                    ApiUrl = ApiUrl
                };
            }

            string organizationId = result.Application?.OrganizationId;
            string applicationId = result.Application?.ApplicationId;
            string apiUrl = result.ApiUrl;

            if (string.IsNullOrEmpty(organizationId))
            {
                InternalHelpers.Log("RequestLogsApiListener: Application.OrganizationId is null", LogLevel.Error);
                return null;
            }

            if (string.IsNullOrEmpty(applicationId))
            {
                InternalHelpers.Log("RequestLogsApiListener: Application.applicationId is null", LogLevel.Error);
                return null;
            }

            if (string.IsNullOrEmpty(apiUrl))
            {
                InternalHelpers.Log("RequestLogsApiListener: ApiUrl is null", LogLevel.Error);
                return null;
            }

            if (!Uri.TryCreate(apiUrl, UriKind.Absolute, out _))
            {
                InternalHelpers.Log($"RequestLogsApiListener: ApiUrl \"{apiUrl}\" is not a valid Uri", LogLevel.Error);
                return null;
            }

            return result;
        }

        private IList<string> GenerateKeywords(FlushLogArgs args)
        {
            try
            {
                IList<string> defaultKeywords = GenerateKeywordsService.CreateKeywords(args) ?? new List<string>();

                IList<string> keywords = ConfigurationOptions.ApplyGenerateKeywords(args, defaultKeywords);

                return keywords ?? new List<string>();
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("GenerateKeywordsService error:");
                sb.AppendLine(ex.ToString());

                InternalHelpers.Log(sb.ToString(), LogLevel.Error);
            }

            return new List<string>();
        }
    }
}
