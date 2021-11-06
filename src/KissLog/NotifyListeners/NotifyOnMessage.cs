using System;
using System.Collections.Generic;

namespace KissLog.NotifyListeners
{
    internal static class NotifyOnMessage
    {
        public static void Notify(LogMessage message, Guid? httpRequestId = null)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            List<LogListenerDecorator> logListeners = KissLogConfiguration.Listeners.GetAll();

            foreach(LogListenerDecorator decorator in logListeners)
            {
                InternalHelpers.WrapInTryCatch(() =>
                {
                    Notify(message, decorator, httpRequestId);
                });
            }
        }

        private static void Notify(LogMessage message, LogListenerDecorator decorator, Guid? httpRequestId = null)
        {
            if(httpRequestId != null && decorator.SkipHttpRequestIds.Contains(httpRequestId.Value))
                return;

            ILogListener listener = decorator.Listener;

            if (listener.Interceptor != null && listener.Interceptor.ShouldLog(message, listener) == false)
                return;

            listener.OnMessage(message);
        }
    }
}
