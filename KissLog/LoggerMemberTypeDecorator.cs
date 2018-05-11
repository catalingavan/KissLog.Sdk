using System;
using System.Collections.Generic;
using System.Net;
using KissLog.Web;

namespace KissLog
{
    public class LoggerMemberTypeDecorator : ILogger
    {
        private readonly ILogger _logger;
        private readonly string _memberType;
        public LoggerMemberTypeDecorator(
            ILogger logger,
            string memberType)
        {
            _logger = logger;
            _memberType = memberType;
        }

        public string CategoryName => _logger.CategoryName;
        public IEnumerable<LogMessage> LogMessages => _logger.LogMessages;
        public string ErrorMessage => _logger.ErrorMessage;
        public WebRequestProperties WebRequestProperties => _logger.WebRequestProperties;
        public HttpStatusCode? HttpStatusCode => _logger.HttpStatusCode;


#if NET40
        public void Log(LogLevel logLevel, string message, Action<LogMessage> action = null,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            _logger.Log(logLevel, message, action, memberName, lineNumber, _memberType);
        }

        public void Log(LogLevel logLevel, object json, Action<LogMessage> action = null,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            _logger.Log(logLevel, json, action, memberName, lineNumber, _memberType);
        }

        public void Log(LogLevel logLevel, Exception ex, Action<LogMessage> action = null,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            _logger.Log(logLevel, ex, action, memberName, lineNumber, _memberType);
        }

        public void Log(LogLevel logLevel, KissLog.Args args, Action<LogMessage> action = null,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            _logger.Log(logLevel, args, action, memberName, lineNumber, _memberType);
        }
#else
        public void Log(LogLevel logLevel, string message, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            string memberType = null)
        {
            _logger.Log(logLevel, message, action, memberName, lineNumber, _memberType);
        }

        public void Log(LogLevel logLevel, object json, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            string memberType = null)
        {
            _logger.Log(logLevel, json, action, memberName, lineNumber, _memberType);
        }

        public void Log(LogLevel logLevel, Exception ex, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            string memberType = null)
        {
            _logger.Log(logLevel, ex, action, memberName, lineNumber, _memberType);
        }

        public void Log(LogLevel logLevel, KissLog.Args args, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            string memberType = null)
        {
            _logger.Log(logLevel, args, action, memberName, lineNumber, _memberType);
        }
#endif

        public void SetHttpStatusCode(HttpStatusCode httpStatusCode)
        {
            _logger.SetHttpStatusCode(httpStatusCode);
        }
    }
}
