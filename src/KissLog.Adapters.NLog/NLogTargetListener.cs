using KissLog.Adapters.NLog;
using KissLog.FlushArgs;
using KissLog.Web;
using System;
using System.Text;

namespace KissLog.Listeners
{
    public class NLogTargetListener : ILogListener
    {
        private readonly ITextFormatter _textFormatter;

        public NLogTargetListener() : this(new NLogTextFormatter())
        {
        }

        public NLogTargetListener(ITextFormatter textFormatter)
        {
            _textFormatter = textFormatter;
        }

        private Lazy<bool> HasKissLogTarget = new Lazy<bool>(() =>
        {
            var target = NLog.LogManager.Configuration.FindTargetByName<KissLogTarget>("KissLog");
            if (target == null)
                return false;

            string kissLogTarget = "<target name=\"kisslog\" type=\"KissLog\" />";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{nameof(NLogTargetListener)} cannot be used when {kissLogTarget} is enabled, as this will create an infinite loop.");
            sb.AppendLine($"{kissLogTarget} means that NLog.Logger writes the logs to KissLog.ILogger");
            sb.AppendLine($"{nameof(NLogTargetListener)} means that KissLog.ILogger writes the logs to NLog.Logger");
            sb.AppendLine($"Only one of {nameof(NLogTargetListener)} or {kissLogTarget} can be used at the same time.");

            KissLog.Internal.InternalHelpers.Log(sb.ToString(), LogLevel.Error);

            return true;
        });

        public int MinimumResponseHttpStatusCode { get; set; } = 0;
        public LogLevel MinimumLogMessageLevel { get; set; } = LogLevel.Trace;
        public LogListenerParser Parser { get; set; } = new LogListenerParser();

        public void OnBeginRequest(HttpRequest httpRequest, ILogger logger)
        {
            if (HasKissLogTarget.Value == true)
                return;

            if (ShouldWriteBeginRequestEvent(httpRequest) == false)
                return;

            var nLogLogger = NLog.LogManager.GetLogger(logger.CategoryName);

            string message = _textFormatter.FormatBeginRequest(httpRequest);
            if (string.IsNullOrEmpty(message))
                return;

            nLogLogger.Info(message);
        }

        public void OnMessage(LogMessage message, ILogger logger)
        {
            if (HasKissLogTarget.Value == true)
                return;

            WriteLogMessage(message);
        }

        public void OnFlush(FlushLogArgs args, ILogger logger)
        {
            if (HasKissLogTarget.Value == true)
                return;

            if (ShouldWriteFlushEvent(args) == false)
                return;

            var nLogLogger = NLog.LogManager.GetLogger(logger.CategoryName);

            string message = _textFormatter.FormatEndRequest(args.WebProperties.Request, args.WebProperties.Response);
            if (string.IsNullOrEmpty(message))
                return;

            nLogLogger.Info(message);
        }

        private void WriteLogMessage(LogMessage message)
        {
            if (message == null)
                return;

            var nLogLogger = NLog.LogManager.GetLogger(message.CategoryName);

            string messageText = _textFormatter.FormatLogMessage(message);
            if (string.IsNullOrEmpty(messageText))
                messageText = message.Message;

            switch (message.LogLevel)
            {
                case LogLevel.Trace:
                    nLogLogger.Trace(messageText);
                    break;

                case LogLevel.Debug:
                    nLogLogger.Debug(messageText);
                    break;

                case LogLevel.Information:
                    nLogLogger.Info(messageText);
                    break;

                case LogLevel.Warning:
                    nLogLogger.Warn(messageText);
                    break;

                case LogLevel.Error:
                case LogLevel.Critical:
                    nLogLogger.Error(messageText);
                    break;

                default:
                    nLogLogger.Debug(messageText);
                    break;
            }
        }

        public Func<HttpRequest, bool> ShouldWriteBeginRequestEvent = (HttpRequest httpRequest) =>
        {
            return true;
        };

        public Func<FlushLogArgs, bool> ShouldWriteFlushEvent = (FlushLogArgs args) =>
        {
            return true;
        };
    }
}
