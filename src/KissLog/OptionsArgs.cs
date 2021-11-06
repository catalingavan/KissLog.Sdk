using KissLog.Http;
using System;

namespace KissLog
{
    public class OptionsArgs
    {
        public class LogListenerHeaderArgs
        {
            public ILogListener Listener { get; }
            public HttpProperties HttpProperties { get; }
            public string HeaderName { get; }
            public string HeaderValue { get; }

            public LogListenerHeaderArgs(ILogListener listener, HttpProperties httpProperties, string headerName, string headerValue)
            {
                if (string.IsNullOrWhiteSpace(headerName))
                    throw new ArgumentNullException(nameof(headerName));
                
                if(httpProperties == null)
                    throw new ArgumentNullException(nameof(httpProperties));

                if (httpProperties.Response == null)
                    throw new ArgumentNullException(nameof(httpProperties.Response));

                Listener = listener ?? throw new ArgumentNullException(nameof(listener));
                HttpProperties = httpProperties;
                HeaderName = headerName;
                HeaderValue = headerValue;
            }
        }

        public class LogListenerCookieArgs
        {
            public ILogListener Listener { get; }
            public HttpProperties HttpProperties { get; }
            public string CookieName { get; }
            public string CookieValue { get; }

            public LogListenerCookieArgs(ILogListener listener, HttpProperties httpProperties, string cookieName, string cookieValue)
            {
                if (string.IsNullOrWhiteSpace(cookieName))
                    throw new ArgumentNullException(nameof(cookieName));

                if (httpProperties == null)
                    throw new ArgumentNullException(nameof(httpProperties));

                if (httpProperties.Response == null)
                    throw new ArgumentNullException(nameof(httpProperties.Response));

                Listener = listener ?? throw new ArgumentNullException(nameof(listener));
                HttpProperties = httpProperties;
                CookieName = cookieName;
                CookieValue = cookieValue;
            }
        }

        public class LogListenerFormDataArgs
        {
            public ILogListener Listener { get; }
            public HttpProperties HttpProperties { get; }
            public string FormDataName { get; }
            public string FormDataValue { get; }

            public LogListenerFormDataArgs(ILogListener listener, HttpProperties httpProperties, string formDataName, string formDataValue)
            {
                if (string.IsNullOrWhiteSpace(formDataName))
                    throw new ArgumentNullException(nameof(formDataName));

                if (httpProperties == null)
                    throw new ArgumentNullException(nameof(httpProperties));

                if (httpProperties.Response == null)
                    throw new ArgumentNullException(nameof(httpProperties.Response));

                Listener = listener ?? throw new ArgumentNullException(nameof(listener));
                HttpProperties = httpProperties;
                FormDataName = formDataName;
                FormDataValue = formDataValue;
            }
        }

        public class LogListenerServerVariableArgs
        {
            public ILogListener Listener { get; }
            public HttpProperties HttpProperties { get; }
            public string ServerVariableName { get; }
            public string ServerVariableValue { get; }

            public LogListenerServerVariableArgs(ILogListener listener, HttpProperties httpProperties, string serverVariableName, string serverVariableValue)
            {
                if (string.IsNullOrWhiteSpace(serverVariableName))
                    throw new ArgumentNullException(nameof(serverVariableName));

                if (httpProperties == null)
                    throw new ArgumentNullException(nameof(httpProperties));

                if (httpProperties.Response == null)
                    throw new ArgumentNullException(nameof(httpProperties.Response));

                Listener = listener ?? throw new ArgumentNullException(nameof(listener));
                HttpProperties = httpProperties;
                ServerVariableName = serverVariableName;
                ServerVariableValue = serverVariableValue;
            }
        }

        public class LogListenerClaimArgs
        {
            public ILogListener Listener { get; }
            public HttpProperties HttpProperties { get; }
            public string ClaimType { get; }
            public string ClaimValue { get; }

            public LogListenerClaimArgs(ILogListener listener, HttpProperties httpProperties, string claimType, string claimValue)
            {
                if (string.IsNullOrWhiteSpace(claimType))
                    throw new ArgumentNullException(nameof(claimType));

                if (httpProperties == null)
                    throw new ArgumentNullException(nameof(httpProperties));

                if (httpProperties.Response == null)
                    throw new ArgumentNullException(nameof(httpProperties.Response));

                Listener = listener ?? throw new ArgumentNullException(nameof(listener));
                HttpProperties = httpProperties;
                ClaimType = claimType;
                ClaimValue = claimValue;
            }
        }

        public class LogListenerInputStreamArgs
        {
            public ILogListener Listener { get; }
            public HttpProperties HttpProperties { get; }

            public LogListenerInputStreamArgs(ILogListener listener, HttpProperties httpProperties)
            {
                if (httpProperties == null)
                    throw new ArgumentNullException(nameof(httpProperties));

                if (httpProperties.Response == null)
                    throw new ArgumentNullException(nameof(httpProperties.Response));

                Listener = listener ?? throw new ArgumentNullException(nameof(listener));
                HttpProperties = httpProperties ?? throw new ArgumentNullException(nameof(httpProperties));
            }
        }
    }
}
