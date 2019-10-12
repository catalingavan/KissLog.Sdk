using KissLog.Web;
using System.Collections.Generic;
using System.Net;

namespace KissLog.Internal
{
    public class LoggerDataContainer
    {
        private readonly ILogger _logger;
        private Dictionary<string, object> _customProperties = null;
        public HttpStatusCode? ExplicitHttpStatusCode { get; set; }
        public WebProperties WebProperties { get; set; }
        public List<CapturedException> Exceptions { get; private set; }
        public List<LogMessage> LogMessages { get; private set; }
        internal LoggerFiles LoggerFiles { get; private set; }

        public LoggerDataContainer(ILogger logger)
        {
            _logger = logger;

            WebProperties = WebPropertiesFactory.CreateDefault();
            Exceptions = new List<CapturedException>();
            LogMessages = new List<LogMessage>();

            LoggerFiles = new LoggerFiles(logger);
        }

        public void AddProperty(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

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
        public Dictionary<string, object> GetProperties()
        {
            return _customProperties ?? new Dictionary<string, object>();
        }

        public void Reset()
        {
            ExplicitHttpStatusCode = null;

            LogMessages.Clear();
            LogMessages = new List<LogMessage>();

            Exceptions.Clear();
            Exceptions = new List<CapturedException>();

            LoggerFiles.Dispose();
            LoggerFiles = new LoggerFiles(_logger);
        }
    }
}
