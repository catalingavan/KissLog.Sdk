using System.Collections.Generic;

namespace KissLog.Web
{
    public class RequestProperties
    {
        public List<KeyValuePair<string, string>> Headers { get; set; }
        public List<KeyValuePair<string, string>> Cookies { get; set; }
        public List<KeyValuePair<string, string>> QueryString { get; set; }
        public List<KeyValuePair<string, string>> FormData { get; set; }
        public List<KeyValuePair<string, string>> ServerVariables { get; set; }
        public List<KeyValuePair<string, string>> Claims { get; set; }
        public string InputStream { get; set; }

        public RequestProperties()
        {
            Headers = new List<KeyValuePair<string, string>>();
            Cookies = new List<KeyValuePair<string, string>>();
            QueryString = new List<KeyValuePair<string, string>>();
            FormData = new List<KeyValuePair<string, string>>();
            ServerVariables = new List<KeyValuePair<string, string>>();
            Claims = new List<KeyValuePair<string, string>>();
        }
    }
}
