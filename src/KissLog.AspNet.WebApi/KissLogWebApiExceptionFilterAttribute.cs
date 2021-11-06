using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace KissLog.AspNet.WebApi
{
    public class KissLogWebApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        static KissLogWebApiExceptionFilterAttribute()
        {
            InternalHelpers.WrapInTryCatch(() =>
            {
                ModuleInitializer.Init();
            });
        }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (HttpContext.Current != null && actionExecutedContext.Exception != null)
            {
                HttpContextBase httpContext = new HttpContextWrapper(HttpContext.Current);
                InternalExceptionLogger.LogException(actionExecutedContext.Exception, httpContext);
            }

            base.OnException(actionExecutedContext);
        }

        public override Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            if (HttpContext.Current != null && actionExecutedContext.Exception != null)
            {
                HttpContextBase httpContext = new HttpContextWrapper(HttpContext.Current);
                InternalExceptionLogger.LogException(actionExecutedContext.Exception, httpContext);
            }

            return base.OnExceptionAsync(actionExecutedContext, cancellationToken);
        }
    }
}
