using KissLog.Http;
using System;
using System.Linq;
using System.Net;
using System.Text;

namespace KissLog.Listeners
{
    public class TextFormatter : ITextFormatter
    {
        public string FormatBeginRequest(HttpRequest httpRequest)
        {
            if (httpRequest == null)
                throw new ArgumentNullException(nameof(httpRequest));

            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine();

            sb.Append($"{httpRequest.StartDateTime:o} [{httpRequest.HttpMethod} {httpRequest.Url.PathAndQuery}]");

            return sb.ToString();
        }

        public string FormatEndRequest(HttpRequest httpRequest, HttpResponse httpResponse)
        {
            if (httpRequest == null)
                throw new ArgumentNullException(nameof(httpRequest));

            if (httpResponse == null)
                throw new ArgumentNullException(nameof(httpResponse));

            HttpStatusCode httpStatusCode = (HttpStatusCode)httpResponse.StatusCode;
            double duration = Math.Max(0, (httpResponse.EndDateTime - httpRequest.StartDateTime).TotalMilliseconds);

            StringBuilder sb = new StringBuilder();
            sb.Append($"{httpResponse.EndDateTime:o} [{httpRequest.HttpMethod} {httpRequest.Url.PathAndQuery}] {httpResponse.StatusCode} {httpStatusCode} Duration: {duration:0,0}ms");

            return sb.ToString();
        }

        public string FormatLogMessage(LogMessage logMessage)
        {
            if (logMessage == null)
                throw new ArgumentNullException(nameof(logMessage));

            string timePart = logMessage.DateTime.ToString("o").Split(new[] { "T" }, StringSplitOptions.RemoveEmptyEntries).ElementAtOrDefault(1);

            return $"{timePart}, {logMessage.LogLevel,-20} {logMessage.Message}";
        }
    }
}
