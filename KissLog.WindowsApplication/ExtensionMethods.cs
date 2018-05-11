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

            if (logger.WebRequestProperties != null)
            {
                if (logger.WebRequestProperties.Response == null)
                    logger.WebRequestProperties.Response = new ResponseProperties();

                logger.WebRequestProperties.EndDateTime = DateTime.UtcNow;
                logger.WebRequestProperties.Response.HttpStatusCode = HttpStatusCode.OK;

                if (string.IsNullOrEmpty(logger.ErrorMessage) == false)
                {
                    logger.WebRequestProperties.Response.HttpStatusCode = HttpStatusCode.InternalServerError;
                }
            }

            Logger.NotifyListeners(logger);
        }
    }
}
