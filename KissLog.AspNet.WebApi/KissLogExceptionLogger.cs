using KissLog.AspNet.Web;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace KissLog.AspNet.WebApi
{
    public class KissLogExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            ILogger logger = LoggerFactory.GetInstance();
            logger.Error(context.Exception);
        }

        public override Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            ILogger logger = LoggerFactory.GetInstance();
            logger.Error(context.Exception);

            return Task.FromResult(true);
        }

        public override bool ShouldLog(ExceptionLoggerContext context)
        {
            return true;
        }
    }
}
