using KissLog.AspNet.Web;
using System.Web.Http.Filters;

namespace KissLog.AspNet.WebApi
{
    public class KissLogWebApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        static KissLogWebApiExceptionFilterAttribute()
        {
            PackageInit.Init();
        }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            ILogger logger = Logger.Factory.Get();
            logger.Log(LogLevel.Error, actionExecutedContext.Exception);

            base.OnException(actionExecutedContext);
        }
    }
}
