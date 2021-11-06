using System;

namespace KissLog.Http
{
    public class HttpProperties
    {
        public HttpRequest Request { get; }
        public HttpResponse Response { get; private set; }

        internal HttpProperties(HttpRequest httpRequest)
        {
            Request = httpRequest ?? throw new ArgumentNullException(nameof(httpRequest));
        }

        internal void SetResponse(HttpResponse response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            Response = response;
        }

        internal HttpProperties Clone()
        {
            HttpRequest request = Request.Clone();
            HttpResponse response = Response == null ? null : Response.Clone();

            return new HttpProperties(request)
            {
                Response = response
            };
        }
    }
}
