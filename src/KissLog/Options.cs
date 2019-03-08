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
        private static readonly string[] InputStreamContentTypes = { "text/plain", "application/json", "application/xml", "text/xml", "text/html" };
        private static readonly string[] ResponseBodyContentTypes = { "application/json" };

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
        internal Predicate<WebRequestProperties> ShouldLogRequestInputStreamFn = (WebRequestProperties request) =>
        {
            string contentType = request.Request.Headers.FirstOrDefault(p => string.Compare(p.Key, "Content-Type", StringComparison.OrdinalIgnoreCase) == 0).Value;
            if (string.IsNullOrEmpty(contentType))
                return false;

            contentType = contentType.ToLowerInvariant();
            return InputStreamContentTypes.Any(p => contentType.Contains(p.ToLowerInvariant()));
        };
        internal Predicate<WebRequestProperties> ShouldLogResponseBodyFn = (WebRequestProperties request) =>
        {
            string contentType = request.Response.Headers.FirstOrDefault(p => string.Compare(p.Key, "Content-Type", StringComparison.OrdinalIgnoreCase) == 0).Value;
            if (string.IsNullOrEmpty(contentType))
                return false;

            contentType = contentType.ToLowerInvariant();

            return ResponseBodyContentTypes.Any(p => contentType.Contains(p.ToLowerInvariant()));
        };
        internal Func<WebRequestProperties, string, bool> ShouldLogCookieFn = (WebRequestProperties request, string cookieName) => false;
        internal Func<Exception, string> AppendExceptionDetailsFn = (Exception ex) => null;
        internal Func<ILogListener, WebRequestProperties, bool> ToggleListenerFn = (ILogListener listener, WebRequestProperties request) => true;

        public JsonSerializerSettings JsonSerializerSettings => JsonSerializerSettingsValue;

        public Options GetUser(Func<RequestProperties, UserDetails> handler)
        {
            GetUserFn = handler;
            return this;
        }

        public Options ShouldLogRequestInputStream(Predicate<WebRequestProperties> handler)
        {
            ShouldLogRequestInputStreamFn = handler;
            return this;
        }

        public Options ShouldLogResponseBody(Predicate<WebRequestProperties> handler)
        {
            ShouldLogResponseBodyFn = handler;
            return this;
        }

        public Options ShouldLogCookie(Func<WebRequestProperties, string, bool> handler)
        {
            ShouldLogCookieFn = handler;
            return this;
        }

        public Options AppendExceptionDetails(Func<Exception, string> handler)
        {
            AppendExceptionDetailsFn = handler;
            return this;
        }

        public Options ToggleListener(Func<ILogListener, WebRequestProperties> handler)
        {
            return this;
        }
    }
}
