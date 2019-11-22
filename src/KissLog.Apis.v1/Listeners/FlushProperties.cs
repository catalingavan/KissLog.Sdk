using KissLog.Apis.v1.Auth;

namespace KissLog.Apis.v1.Listeners
{
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
