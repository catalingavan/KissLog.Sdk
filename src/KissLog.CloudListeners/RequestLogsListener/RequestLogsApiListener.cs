using KissLog.CloudListeners.Auth;
using KissLog.Http;
using KissLog.RestClient.Api;
using KissLog.RestClient.Requests.CreateRequestLog;
using System;
using System.Linq;

namespace KissLog.CloudListeners.RequestLogsListener
{
    public class RequestLogsApiListener : ILogListener
    {
        internal static Options Options { get; } = new Options();

        private readonly IPublicApi _kisslogApi;
        private readonly Application _application;

        public ILogListenerInterceptor Interceptor { get; set; }

        public bool UseAsync { get; set; } = true;
        public string ApiUrl { get; set; } = Constants.KissLogApiUrl;
        public bool IgnoreSslCertificate { get; set; } = false;
        public Action<ExceptionArgs> OnException { get; set; }
        public IObfuscationService ObfuscationService { get; set; } = new ObfuscationService();

        public RequestLogsApiListener(Application application)
        {
            _application = application ?? throw new ArgumentNullException(nameof(application));
        }
        internal RequestLogsApiListener(Application application, IPublicApi kisslogApi) : this(application)
        {
            _kisslogApi = kisslogApi;
        }

        public void OnBeginRequest(HttpRequest httpRequest)
        {
            
        }

        public void OnMessage(LogMessage message)
        {
            
        }

        public void OnFlush(FlushLogArgs args)
        {
            bool isValid = ValidateProperties();
            if (!isValid)
                return;

            InternalLogger.Log("RequestLogsApiListener: OnFlush begin", LogLevel.Trace);

            ObfuscateFlushLogArgsService obfuscateService = new ObfuscateFlushLogArgsService(ObfuscationService);
            obfuscateService.Obfuscate(args);

            CreateRequestLogRequest request = PayloadFactory.Create(args);
            request.OrganizationId = _application.OrganizationId;
            request.ApplicationId = _application.ApplicationId;
            request.Keywords = InternalHelpers.WrapInTryCatch(() => Options.Handlers.GenerateSearchKeywords(args));

            FlushOptions flushOptions = new FlushOptions
            {
                UseAsync = UseAsync,
                OnException = OnException
            };

            IPublicApi kisslogApi = _kisslogApi ?? new PublicRestApi(ApiUrl, IgnoreSslCertificate);

            Flusher.FlushAsync(flushOptions, kisslogApi, args, request).ConfigureAwait(false);

            InternalLogger.Log("RequestLogsApiListener: OnFlush complete", LogLevel.Trace);
        }

        internal bool ValidateProperties()
        {
            string organizationId = _application.OrganizationId;
            string applicationId = _application.ApplicationId;
            string apiUrl = ApiUrl;

            if (string.IsNullOrWhiteSpace(organizationId))
            {
                InternalLogger.Log("RequestLogsApiListener: Application.OrganizationId is null", LogLevel.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(applicationId))
            {
                InternalLogger.Log("RequestLogsApiListener: Application.applicationId is null", LogLevel.Error);
                return false;
            }

            Uri uri = null;

            if (!Uri.TryCreate(apiUrl, UriKind.Absolute, out uri))
            {
                InternalLogger.Log("RequestLogsApiListener: ApiUrl is null", LogLevel.Error);
                return false;
            }

            if (new[] { "http", "https" }.Any(p => string.Compare(p, uri.Scheme, true) == 0) == false)
            {
                InternalLogger.Log($"RequestLogsApiListener: ApiUrl is not a valid Uri: {uri}", LogLevel.Error);
                return false;
            }

            return true;
        }
    }
}
