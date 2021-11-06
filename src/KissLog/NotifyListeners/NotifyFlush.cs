using KissLog.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog.NotifyListeners
{
    internal static class NotifyFlush
    {
        public static void Notify(Logger[] loggers)
        {
            if (loggers == null)
                throw new ArgumentNullException();

            if (!loggers.Any())
                return;

            FlushLogArgs args = FlushLogArgsFactory.Create(loggers);

            Guid? httpRequestId = args.HttpProperties == null ? (Guid?)null : args.HttpProperties.Request.Id;

            List<LogListenerDecorator> logListeners = KissLogConfiguration.Listeners.GetAll();

            foreach (LogListenerDecorator decorator in logListeners)
            {
                InternalHelpers.WrapInTryCatch(() =>
                { 
                    Notify(args, decorator, httpRequestId);
                });
            }

            foreach(Logger logger in loggers)
            {
                logger.Reset();
            }
        }

        private static void Notify(FlushLogArgs args, LogListenerDecorator decorator, Guid? httpRequestId)
        {
            if (httpRequestId != null && decorator.SkipHttpRequestIds.Contains(httpRequestId.Value))
                return;

            ILogListener listener = decorator.Listener;

            if (listener.Interceptor != null && listener.Interceptor.ShouldLog(args, listener) == false)
                return;

            FlushLogArgs argsForListener = CreateArgsForListener(args, listener);

            listener.OnFlush(argsForListener);
        }

        internal static FlushLogArgs CreateArgsForListener(FlushLogArgs flushLogArgs, ILogListener listener)
        {
            if (flushLogArgs == null)
                throw new ArgumentNullException(nameof(flushLogArgs));

            if (listener == null)
                throw new ArgumentNullException(nameof(listener));

            List<KeyValuePair<string, string>> requestHeaders = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> requestCookies = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> requestFormData = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> requestServerVariables = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> requestClaims = new List<KeyValuePair<string, string>>();
            string inputStream = null;

            List<KeyValuePair<string, string>> responseHeaders = new List<KeyValuePair<string, string>>();

            foreach (var item in flushLogArgs.HttpProperties.Request.Properties.Headers)
            {
                var args = new OptionsArgs.LogListenerHeaderArgs(listener, flushLogArgs.HttpProperties, item.Key, item.Value);
                bool shouldLog = KissLogConfiguration.Options.Handlers.ShouldLogRequestHeaderForListener.Invoke(args);

                if (shouldLog == true)
                    requestHeaders.Add(item);
            }

            foreach (var item in flushLogArgs.HttpProperties.Request.Properties.Cookies)
            {
                var args = new OptionsArgs.LogListenerCookieArgs(listener, flushLogArgs.HttpProperties, item.Key, item.Value);
                bool shouldLog = KissLogConfiguration.Options.Handlers.ShouldLogRequestCookieForListener.Invoke(args);

                if (shouldLog == true)
                    requestCookies.Add(item);
            }

            foreach (var item in flushLogArgs.HttpProperties.Request.Properties.FormData)
            {
                var args = new OptionsArgs.LogListenerFormDataArgs(listener, flushLogArgs.HttpProperties, item.Key, item.Value);
                bool shouldLog = KissLogConfiguration.Options.Handlers.ShouldLogFormDataForListener.Invoke(args);

                if (shouldLog == true)
                    requestFormData.Add(item);
            }

            foreach (var item in flushLogArgs.HttpProperties.Request.Properties.ServerVariables)
            {
                var args = new OptionsArgs.LogListenerServerVariableArgs(listener, flushLogArgs.HttpProperties, item.Key, item.Value);
                bool shouldLog = KissLogConfiguration.Options.Handlers.ShouldLogServerVariableForListener.Invoke(args);

                if (shouldLog == true)
                    requestServerVariables.Add(item);
            }

            foreach (var item in flushLogArgs.HttpProperties.Request.Properties.Claims)
            {
                var args = new OptionsArgs.LogListenerClaimArgs(listener, flushLogArgs.HttpProperties, item.Key, item.Value);
                bool shouldLog = KissLogConfiguration.Options.Handlers.ShouldLogClaimForListener.Invoke(args);

                if (shouldLog == true)
                    requestClaims.Add(item);
            }

            if(KissLogConfiguration.Options.Handlers.ShouldLogInputStreamForListener(new OptionsArgs.LogListenerInputStreamArgs(listener, flushLogArgs.HttpProperties)))
            {
                inputStream = flushLogArgs.HttpProperties.Request.Properties.InputStream;
            }

            foreach (var item in flushLogArgs.HttpProperties.Response.Properties.Headers)
            {
                var args = new OptionsArgs.LogListenerHeaderArgs(listener, flushLogArgs.HttpProperties, item.Key, item.Value);
                bool shouldLog = KissLogConfiguration.Options.Handlers.ShouldLogResponseHeaderForListener.Invoke(args);

                if (shouldLog == true)
                    responseHeaders.Add(item);
            }

            List<LogMessagesGroup> messagesGroups = new List<LogMessagesGroup>();
            foreach(var group in flushLogArgs.MessagesGroups)
            {
                List<LogMessage> messages = group.Messages.ToList();
                if(listener.Interceptor != null)
                {
                    messages = messages.Where(p => listener.Interceptor.ShouldLog(p, listener) == true).ToList();
                }

                messagesGroups.Add(new LogMessagesGroup(group.CategoryName, messages));
            }

            FlushLogArgs result = flushLogArgs.Clone();

            result.SetMessagesGroups(messagesGroups);

            result.HttpProperties.Request.SetProperties(new RequestProperties(new RequestProperties.CreateOptions
            {
                Claims = requestClaims,
                Cookies = requestCookies,
                FormData = requestFormData,
                Headers = requestHeaders,
                QueryString = flushLogArgs.HttpProperties.Request.Properties.QueryString.ToList(),
                ServerVariables = requestServerVariables,
                InputStream = inputStream
            }));

            result.HttpProperties.Response.SetProperties(new ResponseProperties(new ResponseProperties.CreateOptions
            {
                Headers = responseHeaders,
                ContentLength = flushLogArgs.HttpProperties.Response.Properties.ContentLength
            }));

            return result;
        }
    }
}
