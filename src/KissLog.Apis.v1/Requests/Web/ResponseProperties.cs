using System.Collections.Generic;

namespace KissLog.Apis.v1.Requests.Web
{
    internal class ResponseProperties
    {
        public string HttpStatusCodeText { get; set; }

        public int HttpStatusCode { get; set; }

        public List<KeyValuePair<string, string>> Headers { get; set; }

        public long ContentLength { get; set; }

        public ResponseProperties()
        {
            Headers = new List<KeyValuePair<string, string>>();
        }
    }
}
