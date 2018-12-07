using KissLog.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace KissLog
{
    public class Logger : ILogger
    {
        public static IKissLoggerFactory Factory { get; private set; } = new DefaultLoggerFactory();

        public const string DefaultCategoryName = "Default";

        private const string ExceptionLoggedKey = "KissLog-ExceptionLogged";

        public static event LogMessageCreatedEventHandler OnMessage;

        private WebRequestProperties _webRequestProperties = null;
        private List<LogMessage> _messages = null;
        private List<CapturedException> _capturedExceptions = null;
        private Dictionary<string, object> _customProperties = null;
        private HttpStatusCode? _httpStatusCode;

        internal LoggerFiles LoggerFiles = null;

        public WebRequestProperties WebRequestProperties => _webRequestProperties;
        internal IEnumerable<LogMessage> LogMessages => _messages;
        public IEnumerable<CapturedException> CapturedExceptions => _capturedExceptions;
        internal HttpStatusCode? HttpStatusCode => _httpStatusCode;

        public Logger() : this(DefaultCategoryName)
        {
        }

        public Logger(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                categoryName = DefaultCategoryName;

            CategoryName = categoryName;

            _webRequestProperties = WebRequestPropertiesFactory.CreateDefault();
            _messages = new List<LogMessage>();
            _capturedExceptions = new List<CapturedException>();

            LoggerFiles = new LoggerFiles(this);
        }

        public string CategoryName { get; set; }

        public void Log(LogLevel logLevel, string message, string memberName = null, int lineNumber = 0, string memberType = null)
        {
            memberType = NormalizeMemberType(memberType);

            if (string.Compare(memberName, "OnError", StringComparison.OrdinalIgnoreCase) == 0)
            {
                memberType = null;
                memberName = null;
                lineNumber = 0;
            }

            LogMessage logMessage = new LogMessage
            {
                CategoryName = CategoryName,
                Message = message,
                LogLevel = logLevel,
                DateTime = DateTime.UtcNow,

                MemberType = memberType,
                MemberName = memberName,
                LineNumber = lineNumber
            };

            _messages.Add(logMessage);

            OnMessage?.Invoke(this, new LogMessageCreatedEventArgs { LogMessage = logMessage });
        }

        public void Log(LogLevel logLevel, object json, string memberName = null, int lineNumber = 0, string memberType = null)
        {
            string formatted = FormatMessage(json);
            Log(logLevel, formatted, memberName, lineNumber, memberType);
        }

        public void Log(LogLevel logLevel, Exception ex, string memberName = null, int lineNumber = 0, string memberType = null)
        {
            if (ex == null)
                return;

            if (ex.Data.Contains(ExceptionLoggedKey))
                return;

            string formatted = FormatMessage(ex);
            Log(logLevel, formatted, memberName, lineNumber, memberType);

            ex.Data[ExceptionLoggedKey] = true;
        }

        public void Log(LogLevel logLevel, Args args, string memberName = null, int lineNumber = 0, string memberType = null)
        {
            if (args == null)
                return;

            StringBuilder sb = new StringBuilder();

            foreach (var arg in args.GetArgs())
            {
                string message = string.Empty;

                if (arg is string)
                {
                    message = (string)arg;
                }
                else if (arg is Exception)
                {
                    message = FormatMessage((Exception)arg);
                }
                else
                {
                    message = FormatMessage(arg);
                }

                sb.AppendLine(message);
            }

            Log(logLevel, sb.ToString(), memberName, lineNumber, memberType);
        }

        private string FormatMessage(object json)
        {
            if (json == null)
                return "null";

            return JsonConvert.SerializeObject(json, Formatting.Indented);
        }

        private string FormatMessage(Exception ex)
        {
            if (ex == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();

            FormatException(ex, sb, "Exception:", new List<string>());

            string exceptionDetails = KissLogConfiguration.AppendExceptionDetails(ex);
            if (string.IsNullOrEmpty(exceptionDetails) == false)
            {
                sb.AppendLine();
                sb.AppendLine(exceptionDetails);
            }

            return sb.ToString();
        }

        private void FormatException(Exception ex, StringBuilder sb, string errorType, List<string> exMessages)
        {
            string exString = ex.ToString();
            bool alreadyLogged = exMessages.Any(p => string.Compare(p, exString, StringComparison.Ordinal) == 0);

            if (alreadyLogged == false)
            {
                sb.AppendLine(errorType);
                sb.AppendLine(exString);

                exMessages.Add(exString);

                _capturedExceptions.Add(new CapturedException
                {
                    ExceptionType = ex.GetType().Name,
                    ExceptionMessage = ex.Message,
                    Exception = exString
                });
            }

            Exception innerException = ex.InnerException;
            while (innerException != null)
            {
                FormatException(innerException, sb, "Inner Exception:", exMessages);
                innerException = innerException.InnerException;
            }

            Exception baseException = ex.GetBaseException();
            if (baseException != null && baseException != ex)
            {
                FormatException(baseException, sb, "Base Exception:", exMessages);
            }
        }

        public static string NormalizeMemberType(string memberType)
        {
            if (string.IsNullOrEmpty(memberType))
                return memberType;

            if (memberType.Contains("\\"))
            {
                string baseDirectory = null;

                #if (NET40 || NET45)
                    baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                #endif

                #if (NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD2_0)
                    baseDirectory = AppContext.BaseDirectory;
                #endif

                if (!string.IsNullOrEmpty(baseDirectory))
                {
                    List<string> baseDirectoryPaths = baseDirectory.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    while (baseDirectoryPaths.ToList().Any())
                    {
                        string basePath = string.Join("\\", baseDirectoryPaths);
                        if (memberType.IndexOf(basePath, StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            string temp = memberType.ToLowerInvariant().Replace(basePath.ToLowerInvariant(), string.Empty);
                            memberType = memberType.Substring((memberType.Length - 1) - (temp.Length - 1));

                            break;
                        }

                        baseDirectoryPaths.RemoveAt(baseDirectoryPaths.Count - 1);
                    }
                }

                string[] paths = memberType.Split(new[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);

                if (paths.ElementAt(0).Contains(":"))
                {
                    // if first element contains : means its a file path, and we couldn't match
                    // the namespace

                    return string.Join("\\", paths);
                }

                string lastPath = paths.ElementAt(paths.Count() - 1);
                if (lastPath.IndexOf(".", StringComparison.Ordinal) > -1)
                {
                    lastPath = lastPath.Substring(0, lastPath.LastIndexOf(".", StringComparison.Ordinal));
                    paths[paths.Length - 1] = lastPath;
                }

                return string.Join(".", paths);
            }

            return memberType;
        }

        public void AddProperty(string key, object value)
        {
            if (_customProperties == null)
                _customProperties = new Dictionary<string, object>();

            if (_customProperties.ContainsKey(key))
            {
                _customProperties[key] = value;
            }
            else
            {
                _customProperties.Add(key, value);
            }
        }
        public object GetProperty(string key)
        {
            if (_customProperties == null || !_customProperties.ContainsKey(key))
                return null;

            return _customProperties[key];
        }

        public void SetWebRequestProperties(WebRequestProperties webRequestProperties)
        {
            _webRequestProperties = webRequestProperties;
        }

        public void SetHttpStatusCode(HttpStatusCode httpStatusCode)
        {
            _httpStatusCode = httpStatusCode;
        }

        internal void Reset()
        {
            _messages.Clear();
            _messages = new List<LogMessage>();

            _capturedExceptions.Clear();
            _capturedExceptions = new List<CapturedException>();

            _httpStatusCode = null;

            LoggerFiles.Dispose();
            LoggerFiles = new LoggerFiles(this);
        }

        public static void NotifyListeners(ILogger logger)
        {
            if (logger == null)
                return;

            NotifyListeners(new[] { logger });
        }

        public static void NotifyListeners(ILogger[] loggers)
        {
            KissLog.NotifyListeners.Notify(loggers);
        }
    }
}
