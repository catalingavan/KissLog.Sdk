using System.Collections.Generic;
using System.Linq;

namespace KissLog
{
    public class LogListenersContainer
    {
        private readonly List<LogListenerDecorator> _listeners;
        internal LogListenersContainer()
        {
            _listeners = new List<LogListenerDecorator>();
        }

        public LogListenersContainer Add(ILogListener listener)
        {
            if (listener == null)
                return this;

            _listeners.Add(new LogListenerDecorator(listener));

            return this;
        }

        internal List<LogListenerDecorator> GetAll()
        {
            return _listeners.Select(p => p).ToList();
        }
    }
}
