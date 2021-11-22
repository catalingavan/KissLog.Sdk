using System;

namespace MongoDbListenerExample.MongoDbListener
{
    public class LogMessage
    {
        public string Category { get; set; }
        public DateTime DateTime { get; set; }
        public string LogLevel { get; set; }
        public string Message { get; set; }
    }
}
