using System;
using System.Collections.Generic;
using System.Linq;
using KissLog.Web;

namespace KissLog
{
    public static class KissLogConfiguration
    {
        public static List<ILogListener> Listeners = new List<ILogListener>();

        public static string ObfuscatedValue = "***obfuscated***";

        public static List<string> UserNameClaims = new List<string> { "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "name", "email" };
        public static List<string> EmailAddressClaims = new List<string> { "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", "email", "emailaddress" };
        public static List<string> UserAvatarClaims = new List<string> { "avatar", "picture", "image" };

        public static List<string> ReadInputStreamContentTypes = new List<string> { "text/plain", "application/json", "application/xml", "text/xml", "text/html" };
        public static Func<WebRequestProperties, bool> ShouldReadInputStream = (WebRequestProperties request) =>
        {
            string contentType = request.Request.Headers.FirstOrDefault(p => string.Compare(p.Key, "Content-Type", StringComparison.OrdinalIgnoreCase) == 0).Value;
            if (string.IsNullOrEmpty(contentType))
                return false;

            contentType = contentType.ToLowerInvariant();

            return ReadInputStreamContentTypes.Any(p => contentType.Contains(p.ToLowerInvariant()));
        };

        public static Func<Exception, string> AppendExceptionDetails = (Exception ex) => null;
    }
}
