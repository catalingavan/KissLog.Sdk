using KissLog.Web;
using System;

namespace KissLog.FlushArgs
{
    public class HttpResponseArgs
    {
        HttpRequestArgs Request { get; set; }

        public ResponseProperties Response { get; set; }

        public DateTime EndDateTime { get; set; }

        public double DurationInMilliseconds { get; set; }
    }
}
