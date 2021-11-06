using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace KissLog.AspNet.WebApi
{
    public class KissLogExceptionLogger : ExceptionLogger
    {
        static KissLogExceptionLogger()
        {
            InternalHelpers.WrapInTryCatch(() =>
            {
                ModuleInitializer.Init();
            });
        }

        public override void Log(ExceptionLoggerContext context)
        {
            if (HttpContext.Current != null && context.Exception != null)
            {
                HttpContextBase httpContext = new HttpContextWrapper(HttpContext.Current);
                InternalExceptionLogger.LogException(context.Exception, httpContext);
            }

            base.Log(context);
        }

        public override Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            if (HttpContext.Current != null && context.Exception != null)
            {
                HttpContextBase httpContext = new HttpContextWrapper(HttpContext.Current);
                InternalExceptionLogger.LogException(context.Exception, httpContext);
            }

            return base.LogAsync(context, cancellationToken);
        }

        public override bool ShouldLog(ExceptionLoggerContext context)
        {
            return true;
        }
    }
}
