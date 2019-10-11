using KissLog.Web;
using System.Linq;

namespace KissLog.Internal
{
    internal static class NotifyOnBeginRequestService
    {
        public static void Notify(HttpRequest httpRequest, Logger logger)
        {
            if (KissLogConfiguration.Listeners == null || KissLogConfiguration.Listeners.Any() == false)
                return;

            foreach (ILogListener listener in KissLogConfiguration.Listeners)
            {
                if (listener == null)
                    continue;

                listener.OnBeginRequest(httpRequest, logger);
            }
        }
    }
}
