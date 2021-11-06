using KissLog.Http;
using System;
using System.Collections.Generic;

namespace KissLog.LoggerData
{
    public class LoggerDataContainer : IDisposable
    {
        internal bool _disposed = false;

        private List<LogMessage> _messages;
        private List<Exception> _exceptions;

        internal DateTime DateTimeCreated { get; }
        public HttpProperties HttpProperties { get; private set; }
        public IEnumerable<LogMessage> LogMessages => _messages;
        public IEnumerable<Exception> Exceptions => _exceptions;
        internal FilesContainer FilesContainer { get; }
        internal LoggerProperties LoggerProperties { get; }

        internal LoggerDataContainer(Logger logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _messages = new List<LogMessage>();
            _exceptions = new List<Exception>();
            
            DateTimeCreated = DateTime.UtcNow;
            FilesContainer = new FilesContainer(logger);
            LoggerProperties = new LoggerProperties();
        }

        internal void SetHttpProperties(HttpProperties httpProperties)
        {
            HttpProperties = httpProperties ?? throw new ArgumentNullException(nameof(httpProperties));
        }

        internal void Add(LogMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            _messages.Add(message);
        }

        internal void Add(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            _exceptions.Add(exception);
        }

        public void Dispose()
        {
            FilesContainer.Dispose();

            _disposed = true;
        }
    }
}
