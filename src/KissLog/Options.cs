using KissLog.FlushArgs;
using KissLog.Web;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace KissLog
{
    public class Options
    {
        private static readonly string[] UserNameClaims = { "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "name", "email" };
        private static readonly string[] EmailClaims = { "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", "email", "emailaddress" };
        private static readonly string[] AvatarClaims = { "avatar", "picture", "image" };

        internal JsonSerializerSettings JsonSerializerSettingsValue = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        };
        internal Func<RequestProperties, UserDetails> GetUserFn = (RequestProperties request) =>
        {
            string userName = request.Claims?.FirstOrDefault(p => UserNameClaims.Contains(p.Key.ToLower())).Value;
            string emailAddress = request.Claims?.FirstOrDefault(p => EmailClaims.Contains(p.Key.ToLower())).Value;
            string avatar = request.Claims?.FirstOrDefault(p => AvatarClaims.Contains(p.Key.ToLower())).Value;

            return new UserDetails
            {
                Name = userName,
                EmailAddress = emailAddress,
                Avatar = avatar
            };
        };

        internal Func<HttpRequest, bool> ShouldLogRequestInputStreamFn = (HttpRequest request) => true;
        internal Func<ILogListener, FlushLogArgs, bool> ShouldLogRequestInputStreamForListenerFn = (ILogListener listener, FlushLogArgs args) => true;

        internal Func<ILogListener, FlushLogArgs, string, bool> ShouldLogRequestHeaderKeyFn = (ILogListener listener, FlushLogArgs args, string name) => true;
        internal Func<ILogListener, FlushLogArgs, string, bool> ShouldLogRequestCookieKeyFn = (ILogListener listener, FlushLogArgs args, string name) => false;
        internal Func<ILogListener, FlushLogArgs, string, bool> ShouldLogRequestQueryStringKeyFn = (ILogListener listener, FlushLogArgs args, string name) => true;

        internal Func<HttpRequest, bool> ShouldLogRequestFormDataFn = (HttpRequest request) => true;
        internal Func<ILogListener, FlushLogArgs, string, bool> ShouldLogRequestFormDataKeyFn = (ILogListener listener, FlushLogArgs args, string name) => true;
        
        internal Func<ILogListener, FlushLogArgs, string, bool> ShouldLogRequestServerVariableKeyFn = (ILogListener listener, FlushLogArgs args, string name) => true;
        internal Func<ILogListener, FlushLogArgs, string, bool> ShouldLogRequestClaimKeyFn = (ILogListener listener, FlushLogArgs args, string name) => true;

        internal Func<ILogListener, FlushLogArgs, string, bool> ShouldLogResponseHeaderFn = (ILogListener listener, FlushLogArgs args, string name) => true;
        internal Func<ILogListener, FlushLogArgs, bool, bool> ShouldLogResponseBodyFn = (ILogListener listener, FlushLogArgs args, bool defaultValue) => defaultValue;

        internal Func<Exception, string> AppendExceptionDetailsFn = (Exception ex) => null;

        internal Func<ILogListener, FlushLogArgs, bool> ToggleListenerFn = (ILogListener listener, FlushLogArgs args) => true;

        public JsonSerializerSettings JsonSerializerSettings => JsonSerializerSettingsValue;

        public Options GetUser(Func<RequestProperties, UserDetails> handler)
        {
            GetUserFn = handler;
            return this;
        }

        public Options ShouldLogRequestHeader(Func<ILogListener, FlushLogArgs, string, bool> handler)
        {
            ShouldLogRequestHeaderKeyFn = handler;
            return this;
        }

        public Options ShouldLogRequestCookie(Func<ILogListener, FlushLogArgs, string, bool> handler)
        {
            ShouldLogRequestCookieKeyFn = handler;
            return this;
        }

        public Options ShouldLogRequestQueryString(Func<ILogListener, FlushLogArgs, string, bool> handler)
        {
            ShouldLogRequestQueryStringKeyFn = handler;
            return this;
        }

        public Options ShouldLogRequestFormData(Func<HttpRequest, bool> handler)
        {
            ShouldLogRequestFormDataFn = handler;
            return this;
        }

        public Options ShouldLogRequestFormData(Func<ILogListener, FlushLogArgs, string, bool> handler)
        {
            ShouldLogRequestFormDataKeyFn = handler;
            return this;
        }

        public Options ShouldLogRequestServerVariable(Func<ILogListener, FlushLogArgs, string, bool> handler)
        {
            ShouldLogRequestServerVariableKeyFn = handler;
            return this;
        }

        public Options ShouldLogRequestClaim(Func<ILogListener, FlushLogArgs, string, bool> handler)
        {
            ShouldLogRequestClaimKeyFn = handler;
            return this;
        }

        public Options ShouldLogRequestInputStream(Func<ILogListener, FlushLogArgs, bool> handler)
        {
            ShouldLogRequestInputStreamForListenerFn = handler;
            return this;
        }

        public Options ShouldLogRequestInputStream(Func<HttpRequest, bool> handler)
        {
            ShouldLogRequestInputStreamFn = handler;
            return this;
        }

        public Options ShouldLogResponseHeader(Func<ILogListener, FlushLogArgs, string, bool> handler)
        {
            ShouldLogResponseHeaderFn = handler;
            return this;
        }

        public Options ShouldLogResponseBody(Func<ILogListener, FlushLogArgs, bool, bool> handler)
        {
            ShouldLogResponseBodyFn = handler;
            return this;
        }

        public Options ToggleListener(Func<ILogListener, FlushLogArgs, bool> handler)
        {
            ToggleListenerFn = handler;
            return this;
        }

        public Options AppendExceptionDetails(Func<Exception, string> handler)
        {
            AppendExceptionDetailsFn = handler;
            return this;
        }
    }
}
