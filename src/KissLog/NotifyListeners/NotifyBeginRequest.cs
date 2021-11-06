using KissLog.Http;
using System;
using System.Collections.Generic;

namespace KissLog.NotifyListeners
{
    internal static class NotifyBeginRequest
    {
        public static void Notify(HttpRequest httpRequest)
        {
            if (httpRequest == null)
                throw new ArgumentNullException(nameof(httpRequest));

            List<LogListenerDecorator> logListeners = KissLogConfiguration.Listeners.GetAll();

            foreach (LogListenerDecorator decorator in logListeners)
            {
                InternalHelpers.WrapInTryCatch(() =>
                {
                    Notify(httpRequest, decorator);
                });
            }
        }

        private static void Notify(HttpRequest httpRequest, LogListenerDecorator decorator)
        {
            ILogListener listener = decorator.Listener;

            if (listener.Interceptor != null && listener.Interceptor.ShouldLog(httpRequest, listener) == false)
            {
                decorator.SkipHttpRequestIds.Add(httpRequest.Id);
                return;
            }

            listener.OnBeginRequest(httpRequest);
        }
    }
}
