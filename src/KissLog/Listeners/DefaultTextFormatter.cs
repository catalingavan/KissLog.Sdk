using KissLog.Web;
using System.Text;

namespace KissLog.Listeners
{
    public class DefaultTextFormatter : ITextFormatter
    {
        public string FormatBeginRequest(HttpRequest httpRequest)
        {
            if (httpRequest == null)
                return string.Empty;

            string httpMethod = (httpRequest.HttpMethod ?? string.Empty).ToUpper();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine(httpRequest.StartDateTime.ToString("o"));
            sb.Append($"{httpMethod} {httpRequest.Url.PathAndQuery}");

            return sb.ToString();
        }

        public string FormatEndRequest(HttpRequest httpRequest, HttpResponse httpResponse)
        {
            if (httpResponse == null)
                return string.Empty;

            string httpStatusCodeText = httpResponse.HttpStatusCode.ToString();
            int httpStatusCode = (int)httpResponse.HttpStatusCode;
            string duration = string.Format("{0:0,0}", (httpResponse.EndDateTime - httpRequest.StartDateTime).TotalMilliseconds);

            StringBuilder sb = new StringBuilder();
            sb.Append($"{httpStatusCode} {httpStatusCodeText} Duration: {duration}ms");

            return sb.ToString();
        }

        public string FormatLogMessage(LogMessage logMessage)
        {
            if (logMessage == null)
                return string.Empty;

            string logLevel = string.Format("{0,-20}", $"[{logMessage.LogLevel.ToString()}]");

            return $"{logLevel} {logMessage.Message}";
        }

        public string FormatFlush(WebProperties webProperties)
        {
            string request = FormatBeginRequest(webProperties.Request);
            string response = FormatEndRequest(webProperties.Request, webProperties.Response);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(request);
            sb.Append(response);

            return sb.ToString();
        }
    }
}
