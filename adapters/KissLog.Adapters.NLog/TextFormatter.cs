using KissLog.Http;
using KissLog.Listeners;
using System;
using System.Net;

namespace KissLog.Adapters.NLog
{
    internal class TextFormatter : ITextFormatter
    {
        public string FormatBeginRequest(HttpRequest httpRequest)
        {
            if (httpRequest == null)
                throw new ArgumentNullException(nameof(httpRequest));

            return $"[{httpRequest.HttpMethod} {httpRequest.Url.PathAndQuery}]";
        }

        public string FormatEndRequest(HttpRequest httpRequest, HttpResponse httpResponse)
        {
            if (httpRequest == null)
                throw new ArgumentNullException(nameof(httpRequest));

            if (httpResponse == null)
                throw new ArgumentNullException(nameof(httpResponse));

            HttpStatusCode httpStatusCode = (HttpStatusCode)httpResponse.StatusCode;
            double duration = Math.Max(0, (httpResponse.EndDateTime - httpRequest.StartDateTime).TotalMilliseconds);

            return $"[{httpRequest.HttpMethod} {httpRequest.Url.PathAndQuery}] {httpResponse.StatusCode} {httpStatusCode} Duration: {duration:0,0}ms";
        }

        public string FormatLogMessage(LogMessage logMessage)
        {
            if (logMessage == null)
                throw new ArgumentNullException(nameof(logMessage));

            return logMessage.Message;
        }
    }
}
