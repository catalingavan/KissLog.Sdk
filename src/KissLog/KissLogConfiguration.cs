using KissLog.Web;
using System;
using System.Collections.Generic;

namespace KissLog
{
    public static class KissLogConfiguration
    {
        #region Obsolete >= 14-04-2019

        [Obsolete("Use Options.GetUser() handler", true)]
        public static Func<RequestProperties, string> GetLoggedInUserName = (RequestProperties request) => null;

        [Obsolete("Use Options.GetUser() handler", true)]
        public static Func<RequestProperties, string> GetLoggedInUserEmailAddress = (RequestProperties request) => null;

        [Obsolete("Use Options.GetUser() handler", true)]
        public static Func<RequestProperties, string> GetLoggedInUserAvatar = (RequestProperties request) => null;

        [Obsolete("Use Options.ShouldLogRequestInputStream() handler", true)]
        public static Func<WebRequestProperties, bool> ShouldLogRequestInputStream = (WebRequestProperties request) => false;

        [Obsolete("Use Options.ShouldLogResponseBody() handler", true)]
        public static Func<WebRequestProperties, bool> ShouldLogResponseBody = (WebRequestProperties request) => false;

        [Obsolete("Use Options.ShouldLogRequestCookie() handler", true)]
        public static Func<string, bool> ShouldLogCookie = (string cookieName) => false;

        [Obsolete("Use Options.AppendExceptionDetails() handler", true)]
        public static Func<Exception, string> AppendExceptionDetails = (Exception ex) => null;

        #endregion

        public static List<ILogListener> Listeners = new List<ILogListener>();

        public static Options Options { get; } = new Options();
    }
}
