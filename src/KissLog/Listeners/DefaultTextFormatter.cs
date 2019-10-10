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

            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine(webRequestProperties.StartDateTime.ToString("o"));
            sb.AppendLine($"{httpMethod} {webRequestProperties.Url.PathAndQuery}");

            if(webRequestProperties.Response != null)
            {
                string httpStatusCodeText = webRequestProperties.Response.HttpStatusCode.ToString();
                int httpStatusCode = (int)webRequestProperties.Response.HttpStatusCode;

                sb.AppendLine($"{httpStatusCode} {httpStatusCodeText}");
            }

            return sb.ToString();
        }

        public string Format(LogMessage logMessage)
        {
            if (logMessage == null)
                return string.Empty;

            string logLevel = string.Format("{0,-20}", $"[{logMessage.LogLevel.ToString()}]");

            return $"{logLevel} {logMessage.Message}";
        }
    }
}
