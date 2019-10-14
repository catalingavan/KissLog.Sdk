using KissLog.Web;

namespace KissLog.FlushArgs
{
    public class BeginRequestArgs
    {
        public bool IsCreatedByHttpRequest { get; set; }
        public HttpRequest Request { get; set; }
    }
}
