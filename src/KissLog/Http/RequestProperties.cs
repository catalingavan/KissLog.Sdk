using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog.Http
{
    public class RequestProperties
    {
        public IEnumerable<KeyValuePair<string, string>> Headers { get; }
        public IEnumerable<KeyValuePair<string, string>> Cookies { get; }
        public IEnumerable<KeyValuePair<string, string>> QueryString { get; }
        public IEnumerable<KeyValuePair<string, string>> FormData { get; }
        public IEnumerable<KeyValuePair<string, string>> ServerVariables { get; }
        public IEnumerable<KeyValuePair<string, string>> Claims { get; private set; }
        public string InputStream { get; }

        internal RequestProperties(CreateOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            Headers = options.Headers?.Where(p => !string.IsNullOrWhiteSpace(p.Key)).ToList() ?? new List<KeyValuePair<string, string>>();
            Cookies = options.Cookies?.Where(p => !string.IsNullOrWhiteSpace(p.Key)).ToList() ?? new List<KeyValuePair<string, string>>();
            QueryString = options.QueryString?.Where(p => !string.IsNullOrWhiteSpace(p.Key)).ToList() ?? new List<KeyValuePair<string, string>>();
            FormData = options.FormData?.Where(p => !string.IsNullOrWhiteSpace(p.Key)).ToList() ?? new List<KeyValuePair<string, string>>();
            ServerVariables = options.ServerVariables?.Where(p => !string.IsNullOrWhiteSpace(p.Key)).ToList() ?? new List<KeyValuePair<string, string>>();
            Claims = options.Claims?.Where(p => !string.IsNullOrWhiteSpace(p.Key)).ToList() ?? new List<KeyValuePair<string, string>>();
            InputStream = options.InputStream;
        }

        internal void SetClaims(IEnumerable<KeyValuePair<string, string>> claims)
        {
            if (claims == null)
                return;

            Claims = claims.ToList();
        }

        internal class CreateOptions
        {
            public IEnumerable<KeyValuePair<string, string>> Headers { get; set; }
            public IEnumerable<KeyValuePair<string, string>> Cookies { get; set; }
            public IEnumerable<KeyValuePair<string, string>> QueryString { get; set; }
            public IEnumerable<KeyValuePair<string, string>> FormData { get; set; }
            public IEnumerable<KeyValuePair<string, string>> ServerVariables { get; set; }
            public IEnumerable<KeyValuePair<string, string>> Claims { get; set; }
            public string InputStream { get; set; }
        }

        internal RequestProperties Clone()
        {
            return new RequestProperties(new CreateOptions
            {
                Headers = Headers,
                Cookies = Cookies,
                QueryString = QueryString,
                FormData = FormData,
                ServerVariables = ServerVariables,
                Claims = Claims,
                InputStream = InputStream
            });
        }
    }
}
