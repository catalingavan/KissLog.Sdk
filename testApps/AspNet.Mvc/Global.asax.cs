using KissLog;
using KissLog.AspNet.Mvc;
using KissLog.AspNet.Web;
using KissLog.CloudListeners.Auth;
using KissLog.CloudListeners.RequestLogsListener;
using KissLog.Listeners.FileListener;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace AspNet.Mvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            GlobalFilters.Filters.Add(new KissLogWebMvcExceptionFilterAttribute());

            ConfigureKissLog();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            if (exception != null)
            {
                var logger = Logger.Factory.Get();
                logger.Error(exception);

                if (logger.AutoFlush() == false)
                {
                    Logger.NotifyListeners(logger);
                }
            }
        }

        private void ConfigureKissLog()
        {
            // optional KissLog configuration
            KissLogConfiguration.Options
                .AppendExceptionDetails((Exception ex) =>
                {
                    StringBuilder sb = new StringBuilder();

                    if (ex is NullReferenceException nullRefException)
                    {
                        sb.AppendLine("Important: check for null references");
                    }

                    return sb.ToString();
                });

            // KissLog internal logs
            KissLogConfiguration.InternalLog = (message) =>
            {
                Debug.WriteLine(message);
            };

            // register listeners
            KissLogConfiguration.Listeners
                .Add(new RequestLogsApiListener(new Application(ConfigurationManager.AppSettings["LogBee.OrganizationId"], ConfigurationManager.AppSettings["LogBee.ApplicationId"]))
                {
                    ApiUrl = ConfigurationManager.AppSettings["LogBee.ApiUrl"],
                    Interceptor = new StatusCodeInterceptor
                    {
                        MinimumLogMessageLevel = LogLevel.Trace,
                        MinimumResponseHttpStatusCode = 200
                    }
                })
                .Add(new LocalTextFileListener(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs")));
        }

        public static KissLogHttpModule KissLogHttpModule = new KissLogHttpModule();

        public override void Init()
        {
            base.Init();

            KissLogHttpModule.Init(this);
        }
    }
}
