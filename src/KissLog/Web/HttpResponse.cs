using System;
using System.Net;

namespace KissLog.Web
{
    public class HttpResponse
    {
        public HttpStatusCode HttpStatusCode { get; set; }

        public DateTime EndDateTime { get; set; }

        public ResponseProperties Properties { get; set; }

        public HttpResponse()
        {
            Properties = new ResponseProperties();
        }
    }
}
