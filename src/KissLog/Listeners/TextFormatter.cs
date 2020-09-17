using KissLog.Web;
using System;
using System.Text;

namespace KissLog.Listeners
{
    public class TextFormatter
    {
        public virtual string FormatBeginRequest(HttpRequest httpRequest)
        {
            if (httpRequest == null)
                return null;

            string httpMethod = (httpRequest.HttpMethod ?? string.Empty).ToUpper();
            string dateTime = FormatDateTime(httpRequest.StartDateTime);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine();

            if (!string.IsNullOrEmpty(dateTime))
                sb.AppendLine(dateTime);

            sb.Append($"{httpMethod} {httpRequest.Url.PathAndQuery}");

            return sb.ToString();
        }

        public virtual string FormatEndRequest(HttpRequest httpRequest, HttpResponse httpResponse)
        {
            if (httpRequest == null || httpResponse == null)
                return null;

            string httpStatusCodeText = httpResponse.HttpStatusCode.ToString();
            int httpStatusCode = (int)httpResponse.HttpStatusCode;
            string duration = string.Format("{0:0,0}", (httpResponse.EndDateTime - httpRequest.StartDateTime).TotalMilliseconds);

            StringBuilder sb = new StringBuilder();
            sb.Append($"{httpStatusCode} {httpStatusCodeText} Duration: {duration}ms");

            return sb.ToString();
        }

        public virtual string FormatLogMessage(LogMessage logMessage)
        {
            if (logMessage == null)
                return null;

            string logLevel = string.Format("{0,-20}", $"[{logMessage.LogLevel.ToString()}]");

            return $"{logLevel} {logMessage.Message}";
        }

        public virtual string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString("o");
        }
    }
}
