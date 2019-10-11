using KissLog.FlushArgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KissLog.Listeners.TextFileListener
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

        public void OnBeginRequest(BeginRequestArgs args, ILogger logger)
        {
            if (FlushTrigger == FlushTrigger.OnMessage)
            {
                string filePath = GetFileName(_logsDirectoryFullPath);

                lock (Locker)
                {
                    using (StreamWriter sw = System.IO.File.AppendText(filePath))
                    {
                        sw.WriteLine(_textFormatter.FormatBeginRequest(args));
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
                        if (args.IsCreatedByHttpRequest == true)
                        {
                            sw.WriteLine(_textFormatter.FormatFlush(new FormatFlushArgs
                            {
                                BeginRequest = args.BeginRequestArgs,
                                EndRequest = args.EndRequestArgs
                            }));
                        }

                        foreach (var logMessage in logMessages)
                        {
                            sw.WriteLine(_textFormatter.FormatLogMessage(logMessage));
                        }
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
