using System;

namespace KissLog.Web
{
    public class WebRequestProperties
    {
        public Uri Url { get; set; }

        public string UserAgent { get; set; }

        public string HttpMethod { get; set; }

        public string RemoteAddress { get; set; }

        public string HttpReferer { get; set; }

        public bool IsNewSession { get; set; }

        public string SessionId { get; set; }

        public string MachineName { get; set; }

        public bool IsAuthenticated { get; set; }

        public UserDetails User { get; set; }

        public RequestProperties Request { get; set; }

        public ResponseProperties Response { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }
    }
}
