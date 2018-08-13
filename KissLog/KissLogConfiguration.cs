using KissLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog
{
    public static class KissLogConfiguration
    {
        private static readonly string[] UserNameClaims = new[] { "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "name", "email" };
        private static readonly string[] EmailClaims = new[] {"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress","email","emailaddress"};
        private static readonly string[] AvatarClaims = new[] {"avatar", "picture", "image"};
        private static readonly string[] InputStreamContentTypes = { "text/plain", "application/json", "application/xml", "text/xml", "text/html" };
        private static readonly string[] ResponseBodyContentTypes = { "application/json" };

        public static List<ILogListener> Listeners = new List<ILogListener>();

        public static string ObfuscatedValue = "***obfuscated***";

        public static Func<RequestProperties, string> GetLoggedInUserName = (RequestProperties request) =>
        {
            return request.Claims?.FirstOrDefault(p => UserNameClaims.Contains(p.Key.ToLower())).Value;
        };

        public static Func<RequestProperties, string> GetLoggedInUserEmailAddress = (RequestProperties request) =>
        {
            return request.Claims?.FirstOrDefault(p => EmailClaims.Contains(p.Key.ToLower())).Value;
        };

        public static Func<RequestProperties, string> GetLoggedInUserAvatar = (RequestProperties request) =>
        {
            return request.Claims?.FirstOrDefault(p => AvatarClaims.Contains(p.Key.ToLower())).Value;
        };

        public static Func<WebRequestProperties, bool> ShouldLogRequestInputStream = (WebRequestProperties request) =>
        {
            string contentType = request.Request.Headers.FirstOrDefault(p => string.Compare(p.Key, "Content-Type", StringComparison.OrdinalIgnoreCase) == 0).Value;
            if (string.IsNullOrEmpty(contentType))
                return false;

            contentType = contentType.ToLowerInvariant();

            return InputStreamContentTypes.Any(p => contentType.Contains(p.ToLowerInvariant()));
        };

        public static Func<WebRequestProperties, bool> ShouldLogResponseBody = (WebRequestProperties request) =>
        {
            string contentType = request.Response.Headers.FirstOrDefault(p => string.Compare(p.Key, "Content-Type", StringComparison.OrdinalIgnoreCase) == 0).Value;
            if (string.IsNullOrEmpty(contentType))
                return false;

            contentType = contentType.ToLowerInvariant();

            return ResponseBodyContentTypes.Any(p => contentType.Contains(p.ToLowerInvariant()));
        };

        public static Func<string, bool> ShouldLogCookie = (string cookieName) =>
        {
            return false;
        };

        public static Func<WebRequestProperties, IEnumerable<string>> AppendSearchKeywords = (WebRequestProperties request) => null;

        public static Func<Exception, string> AppendExceptionDetails = (Exception ex) => null;
    }
}
