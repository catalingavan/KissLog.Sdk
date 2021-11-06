using KissLog.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KissLog.Listeners.FileListener
{
    internal class FlushTextFileListener : ILogListener
    {
        private static readonly object Locker = new object();

        private readonly LocalTextFileListener _listener;
        private readonly ITextFormatter _textFormatter;
        public FlushTextFileListener(
            LocalTextFileListener listener,
            ITextFormatter textFormatter)
        {
            _listener = listener ?? throw new ArgumentNullException(nameof(listener));
            _textFormatter = textFormatter ?? throw new ArgumentNullException(nameof(textFormatter));
        }

        public ILogListenerInterceptor Interceptor { get; set; }

        public void OnBeginRequest(HttpRequest httpRequest)
        {
            
        }

        public void OnMessage(LogMessage message)
        {

        }

        public void OnFlush(FlushLogArgs args)
        {
            IEnumerable<LogMessage> logMessages = args.MessagesGroups.SelectMany(p => p.Messages).OrderBy(p => p.DateTime).ToList();
            string filePath = _listener.GetFilePath();

            string beginRequest = _textFormatter.FormatBeginRequest(args.HttpProperties.Request);
            string endRequest = _textFormatter.FormatEndRequest(args.HttpProperties.Request, args.HttpProperties.Response);

            lock (Locker)
            {
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    if (!string.IsNullOrEmpty(beginRequest))
                        sw.WriteLine(beginRequest);

                    if (!string.IsNullOrEmpty(endRequest))
                        sw.WriteLine(endRequest);

                    foreach (var logMessage in logMessages)
                    {
                        string value = _textFormatter.FormatLogMessage(logMessage);

                        if (!string.IsNullOrEmpty(value))
                            sw.WriteLine(value);
                    }
                }
            }
        }
    }
}
