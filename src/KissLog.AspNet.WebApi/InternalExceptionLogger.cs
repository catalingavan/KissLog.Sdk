using System;
using System.Web;

namespace KissLog.AspNet.WebApi
{
    internal static class InternalExceptionLogger
    {
        public static void LogException(Exception exception, HttpContextBase httpContext)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            var factory = new KissLog.AspNet.Web.LoggerFactory();
            Logger logger = factory.GetInstance(httpContext);

            logger.Error(exception);

            if (exception is HttpException)
            {
                HttpException httpException = (HttpException)exception;
                int statusCode = httpException.GetHttpCode();

                logger.SetStatusCode(statusCode);
            }
        }
    }
}
