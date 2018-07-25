using KissLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog
{
    public static class KissLogConfiguration
    {
        private static readonly string[] UserNameClaims = new[] {"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", "email", "emailaddress"};
        private static readonly string[] EmailClaims = new[] {"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress","email","emailaddress"};
        private static readonly string[] AvatarClaims = new[] {"avatar", "picture", "image"};

        public static List<ILogListener> Listeners = new List<ILogListener>();

        public static string ObfuscatedValue = "***obfuscated***";

        public static Func<IEnumerable<KeyValuePair<string, string>>, string> GetLoggedInUserName = (IEnumerable<KeyValuePair<string, string>> claims) =>
        {
            return claims.FirstOrDefault(p => UserNameClaims.Contains(p.Key.ToLower())).Value;
        };

        public static Func<IEnumerable<KeyValuePair<string, string>>, string> GetLoggedInUserEmailAddress = (IEnumerable<KeyValuePair<string, string>> claims) =>
        {
            return claims.FirstOrDefault(p => EmailClaims.Contains(p.Key.ToLower())).Value;
        };

        public static Func<IEnumerable<KeyValuePair<string, string>>, string> GetLoggedInUserAvatar = (IEnumerable<KeyValuePair<string, string>> claims) =>
        {
            return claims.FirstOrDefault(p => AvatarClaims.Contains(p.Key.ToLower())).Value;
        };

        public static Func<WebRequestProperties, bool> ShouldReadInputStream = (WebRequestProperties request) =>
        {
            string[] contentTypes = {"text/plain", "application/json", "application/xml", "text/xml", "text/html"};

            string contentType = request.Request.Headers.FirstOrDefault(p => string.Compare(p.Key, "Content-Type", StringComparison.OrdinalIgnoreCase) == 0).Value;
            if (string.IsNullOrEmpty(contentType))
                return false;

            contentType = contentType.ToLowerInvariant();

            return contentTypes.Any(p => contentType.Contains(p.ToLowerInvariant()));
        };

        public static Func<string, bool> ShouldReadCookie = (string cookieName) =>
        {
            return false;
        };

        public static Func<Exception, string> AppendExceptionDetails = (Exception ex) => null;
    }
}
