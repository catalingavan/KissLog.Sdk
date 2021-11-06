using KissLog.Http;
using System;
using System.IO;

namespace KissLog.Listeners.FileListener
{
    public class LocalTextFileListener : ILogListener
    {
        private readonly ILogListener _textListener;
        private readonly string _logsDirectoryPath;

        public LocalTextFileListener(string logsDirectoryPath) : this(logsDirectoryPath, FlushTrigger.OnFlush)
        {

        }

        public LocalTextFileListener(
            string logsDirectoryPath,
            FlushTrigger flushTrigger) : this(logsDirectoryPath, flushTrigger, new TextFormatter())
        {

        }

        public LocalTextFileListener(
            string logsDirectoryPath,
            FlushTrigger flushTrigger,
            ITextFormatter textFormatter)
        {
            if (textFormatter == null)
                throw new ArgumentNullException(nameof(textFormatter));

            _logsDirectoryPath = NormalizeLogsDirectoryPath(logsDirectoryPath);

            if (flushTrigger == FlushTrigger.OnMessage)
            {
                _textListener = new MessageTextFileListener(this, textFormatter);
            }
            else
            {
                _textListener = new FlushTextFileListener(this, textFormatter);
            }
        }

        public ILogListenerInterceptor Interceptor { get; set; }

        public void OnBeginRequest(HttpRequest httpRequest)
        {
            _textListener.OnBeginRequest(httpRequest);
        }

        public void OnMessage(LogMessage message)
        {
            _textListener.OnMessage(message);
        }

        public void OnFlush(FlushLogArgs args)
        {
            _textListener.OnFlush(args);
        }

        public Func<string> GetFileName { get; set; } = () => $"{DateTime.UtcNow:yyyy-MM-dd}.log";

        internal string NormalizeLogsDirectoryPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            }

            path = path.Trim('/');
            path = path.Trim('\\');

            if (Path.IsPathRooted(path) == false)
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }

            return path;
        }

        internal string GetFilePath()
        {
            string fileName = GetFileName?.Invoke();
            if (string.IsNullOrWhiteSpace(fileName))
                fileName = $"{DateTime.UtcNow:yyyy-MM-dd}.log";

            if (!Directory.Exists(_logsDirectoryPath))
                Directory.CreateDirectory(_logsDirectoryPath);

            return Path.Combine(_logsDirectoryPath, fileName);
        }
    }
}
