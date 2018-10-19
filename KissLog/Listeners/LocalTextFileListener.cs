using KissLog.Web;
using System;
using System.Collections.Generic;
using System.IO;

namespace KissLog.Listeners
{
    public class LocalTextFileListener : ILogListener
    {
        private static readonly object Locker = new object();

        private readonly ITextFormatter _textFormatter;
        private readonly string _logsDirectoryFullPath;

        public LocalTextFileListener(
            ITextFormatter textFormatter,
            string logsDirectoryFullPath)
        {
            _textFormatter = textFormatter;
            _logsDirectoryFullPath = logsDirectoryFullPath;
        }

        public virtual int MinimumResponseHttpStatusCode { get; set; } = 0;
        public virtual LogLevel MinimumLogMessageLevel { get; set; } = LogLevel.Trace;

        public virtual LogListenerParser Parser { get; set; } = new LogListenerParser();

        public void OnFlush(FlushLogArgs args)
        {
            if (Parser.ShouldLog(args, this) == false)
                return;

            lock (Locker)
            {
                string filePath = GetFileName(_logsDirectoryFullPath);

                using (StreamWriter sw = System.IO.File.AppendText(filePath))
                {
                    Write(sw, args);
                }
            }
        }

        private void Write(StreamWriter sw, FlushLogArgs args)
        {
            if (args.IsCreatedByHttpRequest == true)
            {
                sw.WriteLine(Format(args.WebRequestProperties));
            }

            IEnumerable<LogMessage> logMessages = InternalHelpers.MergeLogMessages(args.MessagesGroups);

            foreach (var logMessage in logMessages)
            {
                if (Parser.ShouldLog(logMessage, this))
                {
                    sw.WriteLine(Format(logMessage));
                }
            }
        }

        private string Format(WebRequestProperties webRequestProperties)
        {
            if (webRequestProperties == null)
                return string.Empty;

            return _textFormatter.Format(webRequestProperties);
        }

        private string Format(LogMessage logMessage)
        {
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
