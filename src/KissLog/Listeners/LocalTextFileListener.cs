using KissLog.FlushArgs;
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
        public FlushTrigger FlushTrigger { get; set; } = FlushTrigger.OnFlush;

        public void OnBeginRequest(HttpRequest httpRequest, ILogger logger)
        {
            if (FlushTrigger == FlushTrigger.OnMessage)
            {
                string filePath = GetFileName(_logsDirectoryFullPath);

                lock (Locker)
                {
                    using (StreamWriter sw = System.IO.File.AppendText(filePath))
                    {
                        sw.WriteLine(_textFormatter.FormatBeginRequest(httpRequest));
                    }
                }
            }
        }

        public void OnMessage(LogMessage message, ILogger logger)
        {
            if (FlushTrigger == FlushTrigger.OnMessage)
            {
                string filePath = GetFileName(_logsDirectoryFullPath);

                lock (Locker)
                {
                    using (StreamWriter sw = System.IO.File.AppendText(filePath))
                    {
                        sw.WriteLine(_textFormatter.FormatLogMessage(message));
                    }
                }
            }
        }

        public void OnFlush(FlushLogArgs args, ILogger logger)
        {
            if (FlushTrigger == FlushTrigger.OnFlush)
            {
                IEnumerable<LogMessage> logMessages = args.MessagesGroups.SelectMany(p => p.Messages).OrderBy(p => p.DateTime).ToList();
                string filePath = GetFileName(_logsDirectoryFullPath);

                lock (Locker)
                {
                    using (StreamWriter sw = System.IO.File.AppendText(filePath))
                    {
                        if (args.WebProperties != null)
                        {
                            sw.WriteLine(_textFormatter.FormatFlush(args.WebProperties));
                        }

                        foreach (var logMessage in logMessages)
                        {
                            sw.WriteLine(_textFormatter.FormatLogMessage(logMessage));
                        }
                    }
                }
            }
            else if(FlushTrigger == FlushTrigger.OnMessage)
            {
                string filePath = GetFileName(_logsDirectoryFullPath);

                lock (Locker)
                {
                    using (StreamWriter sw = System.IO.File.AppendText(filePath))
                    {
                        sw.WriteLine(_textFormatter.FormatEndRequest(args.WebProperties.Request, args.WebProperties.Response));
                    }
                }
            }
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
