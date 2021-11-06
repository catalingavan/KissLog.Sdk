using KissLog.Http;
using System;
using System.IO;

namespace KissLog.Listeners.FileListener
{
    internal class MessageTextFileListener : ILogListener
    {
        private static readonly object Locker = new object();

        private readonly LocalTextFileListener _listener;
        private readonly ITextFormatter _textFormatter;
        public MessageTextFileListener(
            LocalTextFileListener listener,
            ITextFormatter textFormatter)
        {
            _listener = listener ?? throw new ArgumentNullException(nameof(listener));
            _textFormatter = textFormatter ?? throw new ArgumentNullException(nameof(textFormatter));
        }

        public ILogListenerInterceptor Interceptor { get; set; }

        public void OnBeginRequest(HttpRequest httpRequest)
        {
            string value = _textFormatter.FormatBeginRequest(httpRequest);
            if (string.IsNullOrEmpty(value))
                return;

            string filePath = _listener.GetFilePath();

            lock (Locker)
            {
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(value);
                }
            }
        }

        public void OnMessage(LogMessage message)
        {
            string value = _textFormatter.FormatLogMessage(message);
            if (string.IsNullOrEmpty(value))
                return;

            string filePath = _listener.GetFilePath();

            lock (Locker)
            {
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(value);
                }
            }
        }

        public void OnFlush(FlushLogArgs args)
        {
            string value = _textFormatter.FormatEndRequest(args.HttpProperties.Request, args.HttpProperties.Response);
            if (string.IsNullOrEmpty(value))
                return;

            string filePath = _listener.GetFilePath();

            lock (Locker)
            {
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(value);
                }
            }
        }
    }
}
