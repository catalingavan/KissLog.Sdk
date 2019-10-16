using KissLog.Internal;
using System.Collections.Generic;

namespace KissLog
{
    public class ListenersContainer
    {
        private readonly List<LogListenerDecorator> _listeners;
        public ListenersContainer()
        {
            _listeners = new List<LogListenerDecorator>();
        }

        public void Add(ILogListener listener)
        {
            if (listener == null)
                return;

            var decorator = new LogListenerDecorator(listener);
            _listeners.Add(decorator);
        }

        internal IList<LogListenerDecorator> Get()
        {
            List<LogListenerDecorator> result = new List<LogListenerDecorator>();

            foreach(LogListenerDecorator decorator in _listeners)
            {
                result.Add(decorator);
            }

            return result;
        }
    }
}
