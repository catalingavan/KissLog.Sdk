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

        private readonly TextFormatter _textFormatter;
        private readonly string _logsDirectoryFullPath;

        public LocalTextFileListener(string logsDirectoryFullPath) :
            this(new TextFormatter(), logsDirectoryFullPath)
        {
        }

        public LocalTextFileListener(
            TextFormatter textFormatter,
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
                string value = _textFormatter.FormatBeginRequest(httpRequest);
                if (string.IsNullOrEmpty(value))
                    return;
                
                string filePath = GetFileName(_logsDirectoryFullPath);

                lock (Locker)
                {
                    using (StreamWriter sw = System.IO.File.AppendText(filePath))
                    {
                        sw.WriteLine(value);
                    }
                }
            }
        }

        public void OnMessage(LogMessage message, ILogger logger)
        {
            if (FlushTrigger == FlushTrigger.OnMessage)
            {
                string value = _textFormatter.FormatLogMessage(message);
                if (string.IsNullOrEmpty(value))
                    return;

                string filePath = GetFileName(_logsDirectoryFullPath);

                lock (Locker)
                {
                    using (StreamWriter sw = System.IO.File.AppendText(filePath))
                    {
                        sw.WriteLine(value);
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

                string beginRequest = _textFormatter.FormatBeginRequest(args.WebProperties.Request);
                string endRequest = _textFormatter.FormatEndRequest(args.WebProperties.Request, args.WebProperties.Response);

                lock (Locker)
                {
                    using (StreamWriter sw = System.IO.File.AppendText(filePath))
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
            else if(FlushTrigger == FlushTrigger.OnMessage)
            {
                string value = _textFormatter.FormatEndRequest(args.WebProperties.Request, args.WebProperties.Response);
                if (string.IsNullOrEmpty(value))
                    return;

                string filePath = GetFileName(_logsDirectoryFullPath);

                lock (Locker)
                {
                    using (StreamWriter sw = System.IO.File.AppendText(filePath))
                    {
                        sw.WriteLine(value);
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
