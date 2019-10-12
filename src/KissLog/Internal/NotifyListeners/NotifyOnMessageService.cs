using System.Linq;

namespace KissLog.Internal
{
    internal static class NotifyOnMessageService
    {
        public static void Notify(LogMessage message, Logger logger)
        {
            if (KissLogConfiguration.Listeners == null || KissLogConfiguration.Listeners.Any() == false)
                return;

            foreach (ILogListener listener in KissLogConfiguration.Listeners)
            {
                if (listener == null)
                    continue;

                if (listener.Parser != null && listener.Parser.ShouldLog(message, listener) == false)
                    continue;

                listener.OnMessage(message, logger);
            }
        }
    }
}
