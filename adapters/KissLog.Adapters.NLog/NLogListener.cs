using KissLog.Http;
using KissLog.Listeners;
using System;
using System.Text;

namespace KissLog.Adapters.NLog
{
    public class NLogListener : ILogListener
    {
        private readonly ITextFormatter _textFormatter;

        public NLogListener() : this(new TextFormatter())
        {

        }

        public NLogListener(ITextFormatter textFormatter)
        {
            _textFormatter = textFormatter ?? throw new ArgumentNullException(nameof(textFormatter));
        }

        public ILogListenerInterceptor Interceptor { get; set; }

        public void OnBeginRequest(HttpRequest httpRequest)
        {
            if (HasKissLogTarget.Value == true)
                return;

            string content = _textFormatter.FormatBeginRequest(httpRequest);

            var logger = global::NLog.LogManager.GetLogger(Constants.DefaultLoggerCategoryName);
            logger.Info(content);
        }

        public void OnFlush(FlushLogArgs args)
        {
            if (HasKissLogTarget.Value == true)
                return;

            string content = _textFormatter.FormatEndRequest(args.HttpProperties.Request, args.HttpProperties.Response);

            var logger = global::NLog.LogManager.GetLogger(Constants.DefaultLoggerCategoryName);
            logger.Info(content);
        }

        public void OnMessage(LogMessage message)
        {
            if (HasKissLogTarget.Value == true)
                return;

            string content = _textFormatter.FormatLogMessage(message);

            var logger = global::NLog.LogManager.GetLogger(Constants.DefaultLoggerCategoryName);

            switch(message.LogLevel)
            {
                case LogLevel.Trace:
                    logger.Trace(content);
                    break;

                case LogLevel.Debug:
                default:
                    logger.Debug(content);
                    break;

                case LogLevel.Information:
                    logger.Info(content);
                    break;

                case LogLevel.Warning:
                    logger.Warn(content);
                    break;

                case LogLevel.Error:
                    logger.Error(content);
                    break;

                case LogLevel.Critical:
                    logger.Fatal(content);
                    break;
            }
        }

        private Lazy<bool> HasKissLogTarget = new Lazy<bool>(() =>
        {
            var target = global::NLog.LogManager.Configuration.FindTargetByName<KissLogTarget>("KissLog");
            if (target == null)
                return false;

            string kissLogTarget = "<target name=\"kisslog\" type=\"KissLog\" />";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{nameof(NLogListener)} cannot be used when NLog.config has KissLog target, as this will create an infinite loop. ({kissLogTarget})");
            sb.AppendLine($"Only one of {nameof(NLogListener)} or KissLog target can be used at the same time.");

            InternalLogger.Log(sb.ToString(), LogLevel.Error);

            return true;
        });
    }
}
