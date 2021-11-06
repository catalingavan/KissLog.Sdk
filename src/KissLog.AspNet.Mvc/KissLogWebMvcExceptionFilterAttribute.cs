using System;
using System.Web;
using System.Web.Mvc;

namespace KissLog.AspNet.Mvc
{
    public class KissLogWebMvcExceptionFilterAttribute : HandleErrorAttribute
    {
        static KissLogWebMvcExceptionFilterAttribute()
        {
            InternalHelpers.WrapInTryCatch(() =>
            {
                ModuleInitializer.Init();
            });
        }

        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null || filterContext.Exception == null)
                return;

            OnException(filterContext.Exception, filterContext.HttpContext);
        }

        internal void OnException(Exception exception, HttpContextBase httpContext)
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
