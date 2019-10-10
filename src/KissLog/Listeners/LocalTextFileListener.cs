using KissLog.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KissLog.Listeners
{
    public class LocalTextFileListener : ILogListener
    {
        private static readonly object Locker = new object();

        private readonly ITextFormatter _textFormatter;
        private readonly string _logsDirectoryFullPath;

        public LocalTextFileListener(string logsDirectoryFullPath) :
            this(new DefaultTextFormatter(), logsDirectoryFullPath)
        {
        }

        public LocalTextFileListener(
            ITextFormatter textFormatter,
            string logsDirectoryFullPath)
        {
            _textFormatter = textFormatter;
            _logsDirectoryFullPath = logsDirectoryFullPath;
        }

        public int MinimumResponseHttpStatusCode { get; set; } = 0;
        public LogLevel MinimumLogMessageLevel { get; set; } = LogLevel.Trace;
        public LogListenerParser Parser { get; set; } = new LogListenerParser();

        public void OnBeginRequest(WebRequestProperties webRequestProperties, ILogger logger)
        {
            
        }

        public void OnFlush(FlushLogArgs args, ILogger logger)
        {
            lock (Locker)
            {
                string filePath = GetFileName(_logsDirectoryFullPath);

                using (StreamWriter sw = System.IO.File.AppendText(filePath))
                {
                    Write(sw, args);
                }
            }
        }

        public void OnMessage(LogMessage message, ILogger logger)
        {
            lock (Locker)
            {
                string filePath = GetFileName(_logsDirectoryFullPath);

                using (StreamWriter sw = System.IO.File.AppendText(filePath))
                {
                    Write(sw, message);
                }
            }
        }

        private void Write(StreamWriter sw, FlushLogArgs args)
        {
            if (args.IsCreatedByHttpRequest == true)
            {
                sw.WriteLine(Format(args.WebRequestProperties));
            }

            IEnumerable<LogMessage> logMessages = args.MessagesGroups.SelectMany(p => p.Messages).OrderBy(p => p.DateTime).ToList();

            foreach (var logMessage in logMessages)
            {
                sw.WriteLine(Format(logMessage));
            }
        }

        private void Write(StreamWriter sw, LogMessage logMessage)
        {
            sw.WriteLine(Format(logMessage));
        }

        private string Format(WebRequestProperties webRequestProperties)
        {
            if (webRequestProperties == null)
                return string.Empty;

            return _textFormatter.Format(webRequestProperties);
        }

        private string Format(LogMessage logMessage)
        {
            if (logMessage == null)
                return string.Empty;

            return _textFormatter.Format(logMessage);
        }

        public Func<string, string> GetFileName = (string logsDirectoryPath) =>
        {
            if (Directory.Exists(logsDirectoryPath) == false)
                Directory.CreateDirectory(logsDirectoryPath);

            string fileName = $"{DateTime.UtcNow:yyyy-MM-dd}.log";
            return Path.Combine(logsDirectoryPath, fileName);
        };
    }
}
