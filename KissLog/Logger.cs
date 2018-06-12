using KissLog.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace KissLog
{
    public class Logger : ILogger, IDisposable
    {
        private const string ExceptionLoggedKey = "KissLog-ExceptionLogged";

        public const string DefaultCategoryName = "Default";

        protected List<LogMessage> _messages = null;

        protected HttpStatusCode? _httpStatusCode;

        public string CategoryName { get; set; }

        private Dictionary<string, object> _customProperties = null;

        public void AddCustomProperty(string key, object value)
        {
            if(_customProperties == null)
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

        public object GetCustomProperty(string key)
        {
            if (_customProperties == null || !_customProperties.ContainsKey(key))
                return null;

            return _customProperties[key];
        }

        public IEnumerable<LogMessage> LogMessages => _messages;

        public HttpStatusCode? HttpStatusCode => _httpStatusCode;

        public WebRequestProperties WebRequestProperties { get; set; }

        public string ErrorMessage { get; set; }

        public static event LogMessageCreatedEventHandler OnMessage;

        public Logger() : this(DefaultCategoryName)
        {
        }

        public Logger(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
                categoryName = DefaultCategoryName;

            _messages = new List<LogMessage>();
            CategoryName = categoryName;

            WebRequestProperties = WebRequestPropertiesFactory.CreateDefault();
        }

        public void Log(LogLevel logLevel, string message, Action<LogMessage> action = null, string memberName = null, int lineNumber = 0, string memberType = null)
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

            action?.Invoke(logMessage);

            _messages.Add(logMessage);

            OnMessage?.Invoke(this, new LogMessageCreatedEventArgs { LogMessage = logMessage });
        }

        public void Log(LogLevel logLevel, Exception ex, Action<LogMessage> action = null, string memberName = null, int lineNumber = 0, string memberType = null)
        {
            if (ex == null)
                return;

            if(ex.Data.Contains(ExceptionLoggedKey))
                return;

            if(string.Compare(CategoryName, DefaultCategoryName, StringComparison.OrdinalIgnoreCase) == 0)
                ErrorMessage = ex.Message;

            string formatted = FormatMessage(ex);
            Log(logLevel, formatted, action, memberName, lineNumber, memberType);

            ex.Data[ExceptionLoggedKey] = true;
        }

        public void Log(LogLevel logLevel, object json, Action<LogMessage> action = null, string memberName = null, int lineNumber = 0, string memberType = null)
        {
            string formatted = FormatMessage(json);
            Log(logLevel, formatted, action, memberName, lineNumber, memberType);
        }

        public void Log(LogLevel logLevel, KissLog.Args args, Action<LogMessage> action = null, string memberName = null, int lineNumber = 0, string memberType = null)
        {
            if (args == null)
                return;

            StringBuilder sb = new StringBuilder();

            foreach(var arg in args.GetArgs())
            {
                string message = string.Empty;

                if(arg is string)
                {
                    message = (string)arg;
                }
                else if(arg is Exception)
                {
                    message = FormatMessage((Exception)arg);
                }
                else
                {
                    message = FormatMessage(arg);
                }

                sb.AppendLine(message);
            }

            Log(logLevel, sb.ToString(), action, memberName, lineNumber, memberType);
        }

        public void SetHttpStatusCode(HttpStatusCode httpStatusCode)
        {
            _httpStatusCode = httpStatusCode;
        }

        protected virtual string FormatMessage(Exception ex)
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

        protected virtual string FormatMessage(object json)
        {
            if (json == null)
                return "null";

            return JsonConvert.SerializeObject(json, Formatting.Indented);
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

        private void Reset()
        {
            _messages.Clear();
            _messages = new List<LogMessage>();
            _httpStatusCode = null;
        }

        private string NormalizeMemberType(string memberType)
        {
            if (string.IsNullOrEmpty(memberType))
                return memberType;

            if (memberType.Contains("\\"))
            {
                // this means that [System.Runtime.CompilerServices.CallerFilePath] passed the filePath as value

                string baseDirectory = string.Empty;

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

        public static void NotifyListeners(ILogger logger)
        {
            if(logger == null)
                return;

            NotifyListeners(new [] { logger });
        }
        public static void NotifyListeners(ILogger[] loggers)
        {
            if(loggers == null || !loggers.Any())
                return;

            if(KissLogConfiguration.Listeners == null || KissLogConfiguration.Listeners.Any() == false)
                return;

            ILogger defaultLogger = loggers.FirstOrDefault(p => p.CategoryName == DefaultCategoryName) ?? loggers.First();

            WebRequestProperties webRequestProperties = defaultLogger.WebRequestProperties;
            if (webRequestProperties == null)
            {
                webRequestProperties = WebRequestPropertiesFactory.CreateDefault();
            }

            if (defaultLogger.HttpStatusCode.HasValue && webRequestProperties != null)
                webRequestProperties.Response.HttpStatusCode = defaultLogger.HttpStatusCode.Value;

            if (!defaultLogger.HttpStatusCode.HasValue)
            {
                var lastMessage = defaultLogger.LogMessages.LastOrDefault();

                bool isLastMessageError = lastMessage != null && (lastMessage.LogLevel == LogLevel.Critical || lastMessage.LogLevel == LogLevel.Error);
                if (isLastMessageError && webRequestProperties.Response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    webRequestProperties.Response.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
                }
            }

            webRequestProperties.EndDateTime = DateTime.UtcNow;

            IEnumerable<LogMessagesGroup> messagesGroups = loggers.Select(p =>
                new LogMessagesGroup
                {
                    CategoryName = p.CategoryName,
                    Messages = p.LogMessages.ToList()
                })
                .ToList();

            FlushLogArgs args = new FlushLogArgs
            {
                ErrorMessage = defaultLogger.ErrorMessage,
                WebRequestProperties = webRequestProperties,
                MessagesGroups = messagesGroups
            };

            if (KissLogConfiguration.Listeners.Count == 1)
            {
                KissLogConfiguration.Listeners.First().OnFlush(args);
            }
            else
            {
                // we re-create the Args because each Listener can modify the args,
                // and we don't want to keep reference of the instance
                string argsJson = JsonConvert.SerializeObject(args);

                foreach (ILogListener listener in KissLogConfiguration.Listeners)
                {
                    FlushLogArgs theArgs = JsonConvert.DeserializeObject<FlushLogArgs>(argsJson);
                    listener.OnFlush(theArgs);
                }
            }
            
            foreach (ILogger logger in loggers)
            {
                if (logger is Logger myLogger)
                {
                    myLogger.Reset();
                }
            }
        }

        public void Dispose()
        {
            
        }
    }
}
