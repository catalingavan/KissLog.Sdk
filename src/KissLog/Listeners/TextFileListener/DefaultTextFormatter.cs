using KissLog.FlushArgs;
using System.Text;

namespace KissLog.Listeners.TextFileListener
{
    internal class DefaultTextFormatter : ITextFormatter
    {
        public string FormatBeginRequest(BeginRequestArgs args)
        {
            if (args == null)
                return string.Empty;

            string httpMethod = (args.HttpMethod ?? string.Empty).ToUpper();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine(args.StartDateTime.ToString("o"));
            sb.Append($"{httpMethod} {args.Url.PathAndQuery}");

            return sb.ToString();
        }

        public string FormatEndRequest(EndRequestArgs args)
        {
            if (args == null)
                return string.Empty;

            string httpStatusCodeText = args.Response.HttpStatusCode.ToString();
            int httpStatusCode = (int)args.Response.HttpStatusCode;
            string durration = string.Format("{0:0,0}ms", args.DurationInMilliseconds);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{httpStatusCode} {httpStatusCodeText}");
            sb.Append($"{args.EndDateTime.ToString("o")} {durration}");

            return sb.ToString();
        }

        public string FormatLogMessage(LogMessage logMessage)
        {
            if (logMessage == null)
                return string.Empty;

            string logLevel = string.Format("{0,-20}", $"[{logMessage.LogLevel.ToString()}]");

            return $"{logLevel} {logMessage.Message}";
        }

        public string FormatFlush(FormatFlushArgs args)
        {
            string beginRequest = FormatBeginRequest(args.BeginRequest);
            string endRequest = FormatEndRequest(args.EndRequest);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(beginRequest);
            sb.AppendLine(endRequest);

            return sb.ToString();
        }
    }
}
