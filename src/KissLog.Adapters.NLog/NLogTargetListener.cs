using KissLog.Adapters.NLog;
using KissLog.FlushArgs;
using KissLog.Web;
using System;
using System.Text;

namespace KissLog.Listeners
{
    public class NLogTargetListener : ILogListener
    {
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
            
        }

        public void OnMessage(LogMessage message, ILogger logger)
        {
            if (HasKissLogTarget.Value == true)
                return;

            WriteLogMessage(message);
        }

        public void OnFlush(FlushLogArgs args, ILogger logger)
        {
            
        }

        private void WriteLogMessage(LogMessage message)
        {
            if (Parser.ShouldLog(message, this) == false)
                return;

            var nLogLogger = NLog.LogManager.GetLogger(message.CategoryName);

            switch (message.LogLevel)
            {
                case LogLevel.Trace:
                    nLogLogger.Trace(message.Message);
                    break;

                case LogLevel.Debug:
                    nLogLogger.Debug(message.Message);
                    break;

                case LogLevel.Information:
                    nLogLogger.Info(message.Message);
                    break;

                case LogLevel.Warning:
                    nLogLogger.Warn(message.Message);
                    break;

                case LogLevel.Error:
                case LogLevel.Critical:
                    nLogLogger.Error(message.Message);
                    break;

                default:
                    nLogLogger.Debug(message.Message);
                    break;
            }
        }

    }
}
