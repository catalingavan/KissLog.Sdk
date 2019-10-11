using System.Collections.Generic;

namespace KissLog.Web
{
    public class ResponseProperties
    {
        public List<KeyValuePair<string, string>> Headers { get; set; }

        public long ContentLength { get; set; }

        public ResponseProperties()
        {
            Headers = new List<KeyValuePair<string, string>>();
        }
    }
}
