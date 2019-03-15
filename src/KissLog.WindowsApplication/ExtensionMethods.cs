using KissLog.Web;
using System;
using System.Linq;
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
                WebRequestProperties webRequestProperties = theLogger.DataContainer.WebRequestProperties;

                if(webRequestProperties.Response == null)
                {
                    webRequestProperties.Response = new ResponseProperties
                    {
                        HttpStatusCode = HttpStatusCode.OK
                    };
                }

                if(theLogger.DataContainer.Exceptions.Any())
                {
                    webRequestProperties.Response.HttpStatusCode = HttpStatusCode.InternalServerError;
                }

                webRequestProperties.EndDateTime = DateTime.UtcNow;
            }

            Logger.NotifyListeners(logger);
        }
    }
}
