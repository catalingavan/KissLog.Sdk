using KissLog;
using KissLog.AspNet.Mvc;
using KissLog.AspNet.Web;
using KissLog.CloudListeners.Auth;
using KissLog.CloudListeners.RequestLogsListener;
using KissLog.Listeners.FileListener;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
                })
                .GenerateSearchKeywords((FlushLogArgs args) =>
                {
                    var service = new GenerateSearchKeywordsService();
                    IEnumerable<string> keywords = service.GenerateKeywords(args);

                    return keywords;
                });

            // KissLog internal logs
            KissLogConfiguration.InternalLog = (message) =>
            {
                Debug.WriteLine(message);
            };

            // register listeners
            KissLogConfiguration.Listeners
                .Add(new RequestLogsApiListener(new Application(ConfigurationManager.AppSettings["KissLog.OrganizationId"], ConfigurationManager.AppSettings["KissLog.ApplicationId"]))
                {
                    ApiUrl = ConfigurationManager.AppSettings["KissLog.ApiUrl"],
                    Interceptor = new StatusCodeInterceptor
                    {
                        MinimumLogMessageLevel = LogLevel.Trace,
                        MinimumResponseHttpStatusCode = 200
                    },
                    OnException = (ExceptionArgs args) =>
                    {
                        var listener = new LocalTextFileListener("logs", FlushTrigger.OnFlush);
                        listener.OnFlush(args.FlushArgs);
                    }
                })
                .Add(new LocalTextFileListener("Logs_onFlush", FlushTrigger.OnFlush))
                .Add(new LocalTextFileListener("Logs_onMessage", FlushTrigger.OnMessage));
        }

        public static KissLogHttpModule KissLogHttpModule = new KissLogHttpModule();

        public override void Init()
        {
            base.Init();

            KissLogHttpModule.Init(this);
        }
    }
}
