using KissLog.Http;
using System;
using System.Linq;

namespace KissLog
{
    public class Options
    {
        internal HandlersContainer Handlers { get; }

        public Options()
        {
            Handlers = new HandlersContainer();
        }

        public Options AppendExceptionDetails(Func<Exception, string> handler)
        {
            if (handler == null)
                return this;

            Handlers.AppendExceptionDetails = handler;
            return this;
        }

        public Options ShouldLogRequestHeader(Func<OptionsArgs.LogListenerHeaderArgs, bool> handler)
        {
            if (handler == null)
                return this;

            Handlers.ShouldLogRequestHeaderForListener = handler;
            return this;
        }

        public Options ShouldLogRequestCookie(Func<OptionsArgs.LogListenerCookieArgs, bool> handler)
        {
            if (handler == null)
                return this;

            Handlers.ShouldLogRequestCookieForListener = handler;
            return this;
        }

        public Options ShouldLogFormData(Func<OptionsArgs.LogListenerFormDataArgs, bool> handler)
        {
            if (handler == null)
                return this;

            Handlers.ShouldLogFormDataForListener = handler;
            return this;
        }

        public Options ShouldLogServerVariable(Func<OptionsArgs.LogListenerServerVariableArgs, bool> handler)
        {
            if (handler == null)
                return this;

            Handlers.ShouldLogServerVariableForListener = handler;
            return this;
        }

        public Options ShouldLogClaim(Func<OptionsArgs.LogListenerClaimArgs, bool> handler)
        {
            if (handler == null)
                return this;

            Handlers.ShouldLogClaimForListener = handler;
            return this;
        }

        public Options ShouldLogInputStream(Func<OptionsArgs.LogListenerInputStreamArgs, bool> handler)
        {
            if (handler == null)
                return this;

            Handlers.ShouldLogInputStreamForListener = handler;
            return this;
        }

        public Options ShouldLogResponseHeader(Func<OptionsArgs.LogListenerHeaderArgs, bool> handler)
        {
            if (handler == null)
                return this;

            Handlers.ShouldLogResponseHeaderForListener = handler;
            return this;
        }

        public Options ShouldLogFormData(Func<HttpRequest, bool> handler)
        {
            if (handler == null)
                return this;

            Handlers.ShouldLogFormData = handler;
            return this;
        }

        public Options ShouldLogInputStream(Func<HttpRequest, bool> handler)
        {
            if (handler == null)
                return this;

            Handlers.ShouldLogInputStream = handler;
            return this;
        }

        public Options ShouldLogResponseBody(Func<HttpProperties, bool> handler)
        {
            if (handler == null)
                return this;

            Handlers.ShouldLogResponseBody = handler;
            return this;
        }

        internal class HandlersContainer
        {
            public Func<Exception, string> AppendExceptionDetails { get; set; }
            public Func<OptionsArgs.LogListenerHeaderArgs, bool> ShouldLogRequestHeaderForListener { get; set; }
            public Func<OptionsArgs.LogListenerCookieArgs, bool> ShouldLogRequestCookieForListener { get; set; }
            public Func<OptionsArgs.LogListenerFormDataArgs, bool> ShouldLogFormDataForListener { get; set; }
            public Func<OptionsArgs.LogListenerServerVariableArgs, bool> ShouldLogServerVariableForListener { get; set; }
            public Func<OptionsArgs.LogListenerClaimArgs, bool> ShouldLogClaimForListener { get; set; }
            public Func<OptionsArgs.LogListenerInputStreamArgs, bool> ShouldLogInputStreamForListener { get; set; }
            public Func<OptionsArgs.LogListenerHeaderArgs, bool> ShouldLogResponseHeaderForListener { get; set; }
            public Func<HttpRequest,  bool> ShouldLogFormData { get; set; }
            public Func<HttpRequest, bool> ShouldLogInputStream { get; set; }
            public Func<HttpProperties, bool> ShouldLogResponseBody { get; set; }

            public HandlersContainer()
            {
                AppendExceptionDetails = (Exception ex) => null;
                ShouldLogRequestHeaderForListener = (OptionsArgs.LogListenerHeaderArgs args) => true;
                ShouldLogRequestCookieForListener = (OptionsArgs.LogListenerCookieArgs args) => true;
                ShouldLogFormDataForListener = (OptionsArgs.LogListenerFormDataArgs args) => true;
                ShouldLogServerVariableForListener = (OptionsArgs.LogListenerServerVariableArgs args) => true;
                ShouldLogClaimForListener = (OptionsArgs.LogListenerClaimArgs args) => true;
                ShouldLogInputStreamForListener = (OptionsArgs.LogListenerInputStreamArgs args) => true;
                ShouldLogResponseHeaderForListener = (OptionsArgs.LogListenerHeaderArgs args) => true;
                ShouldLogFormData = (HttpRequest args) => true;
                ShouldLogInputStream = (HttpRequest args) => true;
                ShouldLogResponseBody = (HttpProperties args) =>
                {
                    string contentType = args.Response?.Properties?.Headers?.FirstOrDefault(p => string.Compare(p.Key, "Content-Type", StringComparison.OrdinalIgnoreCase) == 0).Value;
                    if (string.IsNullOrEmpty(contentType))
                        return false;

                    contentType = contentType.Trim().ToLowerInvariant();

                    return Constants.DefaultReadResponseBodyContentTypes.Any(p => contentType.Contains(p));
                };
            }
        }
    }
}
