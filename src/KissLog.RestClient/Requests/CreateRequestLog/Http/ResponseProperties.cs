using System.Collections.Generic;

namespace KissLog.RestClient.Requests.CreateRequestLog.Http
{
    public class ResponseProperties
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
