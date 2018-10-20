using KissLog.Web;
using System;
using System.Net;

namespace KissLog.WindowsApplication
{
    public static class ExtensionMethods
    {
        public static void NotifyListeners(this ILogger logger)
        {
            if (logger == null)
                return;

            if(logger is Logger theLogger)
            {
                WebRequestProperties webRequestProperties = theLogger.WebRequestProperties;

                if(webRequestProperties.Response == null)
                {
                    webRequestProperties.Response = new ResponseProperties
                    {
                        HttpStatusCode = HttpStatusCode.OK
                    };
                }

                if(!string.IsNullOrEmpty(theLogger.ErrorMessage))
                {
                    webRequestProperties.Response.HttpStatusCode = HttpStatusCode.InternalServerError;
                }

                webRequestProperties.EndDateTime = DateTime.UtcNow;

                theLogger.SetWebRequestProperties(webRequestProperties);
            }

            Logger.NotifyListeners(logger);
        }
    }
}
