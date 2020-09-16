using KissLog.CloudListeners.Auth;

namespace KissLog.CloudListeners.RequestLogsListener
{
    public class FlushProperties
    {
        public Application Application { get; set; }
        public string ApiUrl { get; set; }

        public FlushProperties()
        {
            ApiUrl = Defaults.KissLogNetUrl;
        }
    }
}
