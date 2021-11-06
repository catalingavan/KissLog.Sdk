using KissLog.Http;
using System;

namespace KissLog.Tests.Common
{
    public class CustomLogListener : ILogListener
    {
        private readonly Action<HttpRequest> _onBeginRequest;
        private readonly Action<LogMessage> _onMessage;
        private readonly Action<FlushLogArgs> _onFlush;
        public CustomLogListener(
            Action<HttpRequest> onBeginRequest = null,
            Action<LogMessage> onMessage = null,
            Action<FlushLogArgs> onFlush = null)
        {
            _onBeginRequest = onBeginRequest;
            _onMessage = onMessage;
            _onFlush = onFlush;
        }

        public ILogListenerInterceptor Interceptor { get; set; }

        public void OnBeginRequest(HttpRequest httpRequest)
        {
            _onBeginRequest?.Invoke(httpRequest);
        }
        
        public void OnMessage(LogMessage message)
        {
            _onMessage?.Invoke(message);
        }
        
        public void OnFlush(FlushLogArgs args)
        {
            _onFlush?.Invoke(args);
        }
    }
}
