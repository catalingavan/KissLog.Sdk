using KissLog.Web;
using System.Text;

namespace KissLog.Listeners
{
    internal class DefaultTextFormatter : ITextFormatter
    {
        public string Format(WebRequestProperties webRequestProperties)
        {
            if (webRequestProperties == null)
                return string.Empty;

            string httpMethod = (webRequestProperties.HttpMethod ?? string.Empty).ToUpper();
            string httpStatusCodeText = webRequestProperties.Response.HttpStatusCode.ToString();
            int httpStatusCode = (int)webRequestProperties.Response.HttpStatusCode;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine(webRequestProperties.StartDateTime.ToString("o"));
            sb.AppendLine($"{httpMethod} {webRequestProperties.Url.PathAndQuery}");
            sb.AppendLine($"{httpStatusCode} {httpStatusCodeText}");
            sb.AppendLine();

            return sb.ToString();
        }

        public string Format(LogMessage logMessage)
        {
            if (logMessage == null)
                return string.Empty;

            string logLevel = string.Format("{0,-20}", $"[{logMessage.LogLevel.ToString()}]");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{logLevel} {logMessage.Message}");

            return sb.ToString();
        }
    }
}
