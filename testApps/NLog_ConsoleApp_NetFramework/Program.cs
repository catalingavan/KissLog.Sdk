using KissLog;
using KissLog.CloudListeners.Auth;
using KissLog.CloudListeners.RequestLogsListener;
using NLog;
using System;
using System.Configuration;

namespace NLog_ConsoleApp_NetFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            KissLog.Logger.SetFactory(new LoggerFactory(new KissLog.Logger(url: "NLog_ConsoleApp/Main")));

            ConfigureKissLog();

            ILogger logger = LogManager.GetCurrentClassLogger();

            logger.Trace("Trace log");
            logger.Debug("Debug log");
            logger.Info("Info log");
            logger.Warn("Warning log");
            logger.Error("Error log");
            logger.Fatal("Fatal log");

            var loggers = KissLog.Logger.Factory.GetAll();
            KissLog.Logger.NotifyListeners(loggers);
        }

        private static void ConfigureKissLog()
        {
            KissLogConfiguration.InternalLog = (message) =>
            {
                Console.WriteLine(message);
            };

            KissLogConfiguration.Listeners
                .Add(new RequestLogsApiListener(new Application(ConfigurationManager.AppSettings["LogBee.OrganizationId"], ConfigurationManager.AppSettings["LogBee.ApplicationId"]))
                {
                    ApiUrl = ConfigurationManager.AppSettings["LogBee.ApiUrl"],
                    UseAsync = false
                });
        }
    }
}
