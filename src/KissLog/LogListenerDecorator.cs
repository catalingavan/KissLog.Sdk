using System;
using System.Collections.Generic;

namespace KissLog
{
    internal class LogListenerDecorator
    {
        public ILogListener Listener { get; }
        public List<Guid> SkipHttpRequestIds { get; }

        public LogListenerDecorator(ILogListener listener)
        {
            Listener = listener ?? throw new ArgumentNullException(nameof(listener));
            SkipHttpRequestIds = new List<Guid>();
        }
    }
}
