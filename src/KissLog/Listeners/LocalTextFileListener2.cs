using System;

namespace KissLog.Listeners
{
    public class LocalTextFileListener2 : ILogListener
    {
        private readonly ITextFormatter _textFormatter;
        private readonly string _logsDirectoryFullPath;

        public int MinimumResponseHttpStatusCode { get; set; } = 0;
        public LogLevel MinimumLogMessageLevel { get; set; } = LogLevel.Trace;
        public LogListenerParser Parser { get; set; } = new LogListenerParser();
        public LocalTextFileFlushTrigger FlushTrigger { get; set; } = LocalTextFileFlushTrigger.NotifyListeners;

        public LocalTextFileListener2(
            ITextFormatter textFormatter,
            string logsDirectoryFullPath)
        {
            _textFormatter = textFormatter;
            _logsDirectoryFullPath = logsDirectoryFullPath;
        }

        public void OnFlush(FlushLogArgs args, ILogger logger)
        {
            if (FlushTrigger != LocalTextFileFlushTrigger.NotifyListeners)
                return;

            throw new NotImplementedException();
        }

        public void OnMessage(LogMessage message, ILogger logger)
        {
            if (FlushTrigger != LocalTextFileFlushTrigger.OnMessage)
                return;

            logger.

            throw new NotImplementedException();
        }
    }
}
