using KissLog.CloudListeners.Auth;
using KissLog.CloudListeners.KissLogRestApi;
using KissLog.CloudListeners.KissLogRestApi.Payload.AppendText;
using KissLog.Internal;
using KissLog.PeriodicListener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KissLog.CloudListeners.LiveTextLogsListener
{
    public class LiveTextLogsApiListener : PeriodicLogListener
    {
        public bool UseAsync { get; set; } = true;
        public string ApiUrl { get; set; } = Defaults.KissLogNetUrl;

        private readonly Application _application;

        public LiveTextLogsApiListener(Application application, PeriodicLogListenerOptions options) : base(options)
        {
            if (application == null)
                throw new ArgumentNullException(nameof(application));

            _application = application;
        }

        protected override Task ProcessBatchAsync(IEnumerable<string> lines)
        {
            if (lines == null || !lines.Any())
                return Task.FromResult(true);

            AppendTextRequest request = new AppendTextRequest
            {
                OrganizationId = _application.OrganizationId,
                ApplicationId = _application.ApplicationId,
                SdkName = InternalHelpers.SdkName,
                SdkVersion = InternalHelpers.SdkVersion,
                Lines = lines.ToList()
            };

            IKissLogRestApi kissLogRestApi = new KissLogRestApiV1Client(ApiUrl);

            if (UseAsync == true)
            {
                kissLogRestApi.AppendTextAsync(request).ConfigureAwait(false);
            }
            else
            {
                kissLogRestApi.AppendText(request);
            }

            return Task.FromResult(true);
        }
    }
}
