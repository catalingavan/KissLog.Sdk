using System;

namespace KissLog.Web.v2
{
    public class HttpResponse
    {
        public ResponseProperties Response { get; set; }

        public DateTime EndDateTime { get; set; }

        public double DurationInMilliseconds { get; set; }
    }
}
