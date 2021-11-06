using System;

namespace KissLog.Http
{
    public class HttpRequest
    {
        internal Guid Id { get; }

        public DateTime StartDateTime { get; }
        public Uri Url { get; }
        public string HttpMethod { get; }
        public string UserAgent { get; }
        public string RemoteAddress { get;  }
        public string HttpReferer { get;  }
        public string SessionId { get; private set; }
        public bool IsNewSession { get; private set; }
        public bool IsAuthenticated { get; private set; }
        public string MachineName { get; }
        public RequestProperties Properties { get; private set; }

        internal HttpRequest(CreateOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            Id = Guid.NewGuid();

            string httpMethod = Constants.HttpMethodRegex.Replace(options.HttpMethod ?? string.Empty, string.Empty).Trim().ToUpperInvariant();

            if (string.IsNullOrWhiteSpace(httpMethod))
                throw new ArgumentNullException(nameof(httpMethod));

            Url = options.Url ?? throw new ArgumentNullException(nameof(options.Url));
            HttpMethod = httpMethod;
            UserAgent = options.UserAgent;
            RemoteAddress = options.RemoteAddress;
            HttpReferer = options.HttpReferer;
            SessionId = options.SessionId;
            IsNewSession = options.IsNewSession;
            IsAuthenticated = options.IsAuthenticated;
            MachineName = options.MachineName;
            Properties = options.Properties ?? new RequestProperties(new RequestProperties.CreateOptions());
            StartDateTime = options.StartDateTime;
        }

        internal void SetSession(string sessionId, bool isNewSession)
        {
            SessionId = sessionId;
            IsNewSession = isNewSession;
        }

        internal void SetIsAuthenticated(bool value)
        {
            IsAuthenticated = value;
        }

        internal void SetProperties(RequestProperties properties)
        {
            Properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        internal class CreateOptions
        {
            public Uri Url { get; set; }
            public DateTime StartDateTime { get; set; }
            public string HttpMethod { get; set; }
            public string UserAgent { get; set; }
            public string RemoteAddress { get; set; }
            public string HttpReferer { get; set; }
            public string SessionId { get; set; }
            public bool IsNewSession { get; set; }
            public bool IsAuthenticated { get; set; }
            public string MachineName { get; set; }
            public RequestProperties Properties { get; set; }

            public CreateOptions()
            {
                StartDateTime = DateTime.UtcNow;
            }
        }

        internal HttpRequest Clone()
        {
            return new HttpRequest(new CreateOptions
            {
                Url = Url,
                HttpMethod = HttpMethod,
                UserAgent = UserAgent,
                RemoteAddress = RemoteAddress,
                HttpReferer = HttpReferer,
                SessionId = SessionId,
                IsNewSession = IsNewSession,
                IsAuthenticated = IsAuthenticated,
                MachineName = MachineName,
                StartDateTime = StartDateTime,
                Properties = Properties.Clone()
            });
        }
    }
}
