namespace KissLog.Internal
{
    internal static class NotifyOnMessageService
    {
        public static void Notify(LogMessage message, Logger logger)
        {
            foreach (LogListenerDecorator decorator in KissLogConfiguration.Listeners.Get())
            {
                ILogListener listener = decorator.Listener;

                if (decorator.ShouldSkipOnMessage(logger))
                    continue;

                if (listener.Parser != null && listener.Parser.ShouldLog(message, listener) == false)
                    continue;

                listener.OnMessage(message, logger);
            }
        }
    }
}
