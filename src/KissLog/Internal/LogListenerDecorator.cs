using KissLog.Web;
using System.Collections.Generic;

namespace KissLog.Internal
{
    internal class LogListenerDecorator
    {
        public LogListenerDecorator(ILogListener listener)
        {
            Listener = listener;
        }
        
        public ILogListener Listener { get; private set; }
        public List<string> SkipRequestIds = new List<string>();

        public void Reset()
        {
            SkipRequestIds.Clear();
            SkipRequestIds = new List<string>();
        }

        public bool ShouldSkipOnMessage(Logger logger)
        {
            string requestId = logger.DataContainer?.WebProperties?.Request?._KissLogRequestId;

            if (string.IsNullOrEmpty(requestId))
                return false;

            if (SkipRequestIds.Contains(requestId))
                return true;

            return false;
        }

        public bool ShouldSkipOnFlush(HttpRequest request)
        {
            string requestId = request?._KissLogRequestId;

            if (string.IsNullOrEmpty(requestId))
                return false;

            if (SkipRequestIds.Contains(requestId))
                return true;

            return false;
        }
    }
}
