using KissLog.Apis.v1.Apis;
using KissLog.Apis.v1.Auth;
using KissLog.Apis.v1.Factories;
using KissLog.FlushArgs;
using KissLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KissLog.Apis.v1.Listeners
{
    public class KissLogApiListener : ILogListener
    {
        public ObfuscateArgsService ObfuscateService { get; } = new ObfuscateArgsService();
        public TruncateArgsService TruncateService { get; } = new TruncateArgsService();

        public bool UseAsync { get; set; } = true;
        public string ApiUrl { get; set; } = "https://api.kisslog.net";

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

        public void OnBeginRequest(BeginRequestArgs args, ILogger logger)
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
            string organizationId = _application?.OrganizationId;
            string applicationId = _application?.ApplicationId;

            if (string.IsNullOrEmpty(organizationId) || string.IsNullOrEmpty(applicationId) || string.IsNullOrEmpty(ApiUrl))
                return;

            if (!Uri.TryCreate(ApiUrl, UriKind.Absolute, out _))
                return;

            ObfuscateService?.Obfuscate(args);
            TruncateService?.Truncate(args);

            Requests.CreateRequestLogRequest request = CreateRequestLogRequestFactory.Create(args);
            request.OrganizationId = organizationId;
            request.ApplicationId = applicationId;
            request.Keywords = Configuration.Configuration.Options.ApplyAddRequestKeywordstHeader(args);

            if(UseAsync == true)
            {
                // we need to copy files, because we start a new Thread, and the existing files will be deleted before accessing them
                IList<LoggerFile> copy = CopyFiles(args.Files?.ToList());

                Task.Factory.StartNew(async () =>
                {
                    IKissLogApi kissLogApi = new KissLogRestApi(ApiUrl);
                    await Flusher.FlushAsync(kissLogApi, request, copy);
                })
                .ConfigureAwait(false);
            }
            else
            {
                IList<LoggerFile> copy = CopyFiles(args.Files?.ToList());

                IKissLogApi kissLogApi = new KissLogRestApi(ApiUrl);
                Flusher.Flush(kissLogApi, request, copy);
            }
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
    }
}