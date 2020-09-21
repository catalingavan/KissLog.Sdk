using KissLog.Apis.v1.Auth;
using System;

namespace KissLog.Apis.v1.Listeners
{
    [Obsolete("Install KissLog.CloudListeners NuGet package and replace with FlushProperties(). https://docs.kisslog.net/SDK/change-log/index.html#kisslog-4-0-0", true)]
    public class FlushProperties
    {
        public Application Application { get; set; }
        public string ApiUrl { get; set; }
        public ApiVersion ApiVersion { get; set; }

        public FlushProperties()
        {
            ApiUrl = Defaults.ApiUrl;
            ApiVersion = Defaults.ApiVersion;
        }
    }
}
