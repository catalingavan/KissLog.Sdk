namespace KissLog.CloudListeners.KissLogRestApi.Payload.CreateRequestLog.Web
{
    internal class WebRequestProperties
    {
        public Url Url { get; set; }

        public string UserAgent { get; set; }

        public string HttpMethod { get; set; }

        public string RemoteAddress { get; set; }

        public string HttpReferer { get; set; }

        public RequestProperties Request { get; set; }

        public ResponseProperties Response { get; set; }
    }
}
