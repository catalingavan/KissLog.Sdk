using KissLog.FlushArgs;
using KissLog.Web;
using System.Linq;

namespace KissLog.Internal
{
    internal static class NotifyOnBeginRequestService
    {
        public static void Notify(WebRequestProperties webRequestProperties, Logger logger)
        {
            if (KissLogConfiguration.Listeners == null || KissLogConfiguration.Listeners.Any() == false)
                return;

            BeginRequestArgs args = Factory.CreateBeginRequestArgs(webRequestProperties);

            foreach (ILogListener listener in KissLogConfiguration.Listeners)
            {
                if (listener == null)
                    continue;

                listener.OnBeginRequest(args, logger);
            }
        }
    }
}
