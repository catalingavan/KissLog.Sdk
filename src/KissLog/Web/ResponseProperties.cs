using System.Collections.Generic;
using System.Net;

namespace KissLog.Web
{
    public class ResponseProperties
    {
        public HttpStatusCode HttpStatusCode { get; set; }

        public List<KeyValuePair<string, string>> Headers { get; set; }

        public long ContentLength { get; set; }

        public ResponseProperties()
        {
            Headers = new List<KeyValuePair<string, string>>();
        }
    }
}
