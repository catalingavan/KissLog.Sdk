using System;

namespace KissLog.Http
{
    public class HttpResponse
    {
        public int StatusCode { get; private set; }

        public DateTime EndDateTime { get; }

        public ResponseProperties Properties { get; private set; }

        internal HttpResponse(CreateOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            StatusCode = options.StatusCode;
            EndDateTime = options.EndDateTime;
            Properties = options.Properties ?? new ResponseProperties(new ResponseProperties.CreateOptions());
        }

        internal void SetStatusCode(int statusCode)
        {
            StatusCode = statusCode;
        }

        internal void SetProperties(ResponseProperties properties)
        {
            Properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        internal class CreateOptions
        {
            public int StatusCode { get; set; }
            public DateTime EndDateTime { get; set; }
            public ResponseProperties Properties { get; set; }

            public CreateOptions()
            {
                EndDateTime = DateTime.UtcNow;
            }
        }

        internal HttpResponse Clone()
        {
            return new HttpResponse(new CreateOptions
            {
                StatusCode = StatusCode,
                EndDateTime = EndDateTime,
                Properties = Properties.Clone()
            });
        }
    }
}
