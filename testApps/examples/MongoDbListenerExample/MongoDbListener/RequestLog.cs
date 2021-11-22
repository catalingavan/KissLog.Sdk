using System;
using System.Collections.Generic;

namespace MongoDbListenerExample.MongoDbListener
{
    public class RequestLog
    {
        public string Id { get; private set; }
        public DateTime DateTime { get; set; }
        public string UserAgent { get; set; }
        public string HttpMethod { get; set; }
        public string AbsoluteUri { get; set; }
        public double DurationInMilliseconds { get; set; }
        public IEnumerable<KeyValuePair<string, string>> RequestHeaders { get; set; }
        public int StatusCode { get; set; }
        public IEnumerable<KeyValuePair<string, string>> ResponseHeaders { get; set; }
        public IEnumerable<LogMessage> Messages { get; set; }

        public RequestLog()
        {
            Id = Guid.NewGuid().ToString();
            RequestHeaders = new List<KeyValuePair<string, string>>();
            ResponseHeaders = new List<KeyValuePair<string, string>>();
            Messages = new List<LogMessage>();
        }
    }
}
