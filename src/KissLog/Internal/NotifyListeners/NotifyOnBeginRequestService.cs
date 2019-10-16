using KissLog.FlushArgs;
using KissLog.Web;

namespace KissLog.Internal
{
    internal static class NotifyOnBeginRequestService
    {
        public static void Notify(HttpRequest httpRequest, Logger logger)
        {
            foreach (LogListenerDecorator decorator in KissLogConfiguration.Listeners.Get())
            {
                ILogListener listener = decorator.Listener;

                BeginRequestArgs args = new BeginRequestArgs
                {
                    IsCreatedByHttpRequest = logger.IsCreatedByHttpRequest(),
                    Request = httpRequest
                };

                if (ShouldUseListener(listener, args) == false)
                {
                    // make the listener skip all the events for the current request
                    decorator.SkipRequestIds.Add(args.Request._KissLogRequestId);

                    continue;
                }

                listener.OnBeginRequest(httpRequest, logger);
            }
        }

        private static bool ShouldUseListener(ILogListener listener, BeginRequestArgs args)
        {
            LogListenerParser parser = listener.Parser;
            if (parser == null)
                return true;

            return parser.ShouldLog(args, listener);
        }
    }
}
