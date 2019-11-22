using KissLog.Apis.v1.Auth;
using KissLog.Apis.v1.Factories;
using KissLog.Apis.v1.Flusher;
using KissLog.FlushArgs;
using KissLog.Internal;
using KissLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog.Apis.v1.Listeners
{
    public class KissLogApiListener : ILogListener
    {
        public ObfuscateArgsService ObfuscateService { get; } = new ObfuscateArgsService();
        public TruncateArgsService TruncateService { get; } = new TruncateArgsService();

        public bool UseAsync { get; set; } = true;
        public string ApiUrl { get; set; } = Defaults.ApiUrl;
        public ApiVersion ApiVersion { get; set; } = Defaults.ApiVersion;

        public Func<FlushLogArgs, FlushProperties> UpdateFlushProperties = (flushArgs) => null;

        private readonly Application _application;

        #region Obsolete >= 18-05-2019

        [Obsolete("This constructor is obsolete. Please use KissLogApiListener(Application application) constructor.")]
        public KissLogApiListener(
            string organizationId,
            string applicationId,
            string environment) : this(new Application(organizationId, applicationId))
        { }

        #endregion

        public KissLogApiListener(Application application)
        {
            _application = application;
        }

        public int MinimumResponseHttpStatusCode { get; set; } = 0;
        public LogLevel MinimumLogMessageLevel { get; set; } = LogLevel.Trace;

        public virtual LogListenerParser Parser { get; set; } = new LogListenerParser();

        public void OnBeginRequest(HttpRequest httpRequest, ILogger logger)
        {
            // Do nothing
            // KissLogApiListeners saves the logs only at the end of the request
        }

        public void OnMessage(LogMessage message, ILogger logger)
        {
            // Do nothing
            // KissLogApiListeners saves the logs only at the end of the request
        }

        public void OnFlush(FlushLogArgs args, ILogger logger)
        {
            FlushProperties flushProperties = GetAndValidateFlushProperties(args);
            if (flushProperties == null)
                return;

            InternalHelpers.Log("KissLogApiListener: OnFlush begin", LogLevel.Trace);

            ObfuscateService?.Obfuscate(args);
            TruncateService?.Truncate(args);

            Requests.CreateRequestLogRequest request = CreateRequestLogRequestFactory.Create(args);
            request.OrganizationId = flushProperties.Application.OrganizationId;
            request.ApplicationId = flushProperties.Application.ApplicationId;
            request.Keywords = Configuration.Configuration.Options.ApplyAddRequestKeywordstHeader(args);

            // we need to copy files, because we start a new Thread, and the existing files will be deleted before accessing them
            IList<LoggerFile> copy = CopyFiles(args.Files?.ToList());

            IFlusher flusher = CreateFlusher(flushProperties);

            if (UseAsync == true)
            {
                flusher.FlushAsync(request, copy).ConfigureAwait(false);
            }
            else
            {
                flusher.Flush(request, copy);
            }

            InternalHelpers.Log("KissLogApiListener: OnFlush complete", LogLevel.Trace);
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

            if(UpdateFlushProperties != null)
            {
                result = UpdateFlushProperties(args);
            }

            if(result == null)
            {
                result = new FlushProperties
                {
                    Application = _application,
                    ApiUrl = ApiUrl,
                    ApiVersion = ApiVersion
                };
            }

            string organizationId = result.Application?.OrganizationId;
            string applicationId = result.Application?.ApplicationId;
            string apiUrl = result.ApiUrl;

            if (string.IsNullOrEmpty(organizationId))
            {
                InternalHelpers.Log("KissLogApiListener: Application.OrganizationId is null", LogLevel.Error);
                return null;
            }

            if (string.IsNullOrEmpty(applicationId))
            {
                InternalHelpers.Log("KissLogApiListener: Application.applicationId is null", LogLevel.Error);
                return null;
            }

            if (string.IsNullOrEmpty(apiUrl))
            {
                InternalHelpers.Log("KissLogApiListener: ApiUrl is null", LogLevel.Error);
                return null;
            }

            if (!Uri.TryCreate(apiUrl, UriKind.Absolute, out _))
            {
                InternalHelpers.Log($"KissLogApiListener: ApiUrl \"{apiUrl}\" is not a valid Uri", LogLevel.Error);
                return null;
            }

            return result;
        }
        
        private IFlusher CreateFlusher(FlushProperties flushProperties)
        {
            IFlusher flusher = null;
            string apiUrl = flushProperties.ApiUrl;

            switch (flushProperties.ApiVersion)
            {
                case ApiVersion.v1:
                    flusher = new FlusherRestV1(apiUrl);
                    break;

                default:
                    flusher = new FlusherRestV2(apiUrl);
                    break;
            }

            return flusher;
        }
    }
}