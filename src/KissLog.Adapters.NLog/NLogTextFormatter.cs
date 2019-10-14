using KissLog.Listeners;
using KissLog.Web;
using System.Text;

namespace KissLog.Adapters.NLog
{
    internal class NLogTextFormatter : ITextFormatter
    {
        public string FormatBeginRequest(HttpRequest httpRequest)
        {
            if (httpRequest == null)
                return string.Empty;

            string httpMethod = (httpRequest.HttpMethod ?? string.Empty).ToUpper();

            return $"[{httpMethod} {httpRequest.Url.PathAndQuery}]";
        }

        public string FormatEndRequest(HttpRequest httpRequest, HttpResponse httpResponse)
        {
            if (httpRequest == null || httpResponse == null)
                return string.Empty;

            string httpMethod = (httpRequest.HttpMethod ?? string.Empty).ToUpper();

            string httpStatusCodeText = httpResponse.HttpStatusCode.ToString();
            int httpStatusCode = (int)httpResponse.HttpStatusCode;
            string duration = string.Format("{0:0,0}", (httpResponse.EndDateTime - httpRequest.StartDateTime).TotalMilliseconds);

            return $"[{httpStatusCode} {httpStatusCodeText}][{httpMethod} {httpRequest.Url.PathAndQuery}] Duration: {duration}ms";
        }

        public string FormatLogMessage(LogMessage logMessage)
        {
            if (logMessage == null)
                return string.Empty;

            return logMessage.Message;
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
