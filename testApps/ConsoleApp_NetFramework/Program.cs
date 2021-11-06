using KissLog;
using KissLog.CloudListeners.Auth;
using KissLog.CloudListeners.RequestLogsListener;
using KissLog.Listeners.FileListener;
using System;
using System.Configuration;
using System.IO;

namespace ConsoleApp_NetFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.SetFactory(new LoggerFactory(new Logger(url: "ConsoleApp/Main")));

            ConfigureKissLog();

            IKLogger logger = Logger.Factory.Get();

            logger.Trace("Trace log");
            logger.Debug("Debug log");
            logger.Info("Information log");
            logger.Warn("Warning log");
            logger.Error("Error log");
            logger.Critical("Critical log");

            logger.Error(new DivideByZeroException());

            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App.config");

            logger.LogAsFile($"Text content logged as file. Guid: {Guid.NewGuid()}", "file-01.txt");
            logger.LogFile(file, "appsettings.json");

            logger.AddCustomProperty("CorrelationId", Guid.NewGuid());
            logger.AddCustomProperty("boolean", true);
            logger.AddCustomProperty("date", DateTime.UtcNow);
            logger.AddCustomProperty("integer", 100);

            var loggers = Logger.Factory.GetAll();
            Logger.NotifyListeners(loggers);
        }

        private static void ConfigureKissLog()
        {
            KissLogConfiguration.InternalLog = (message) =>
            {
                Console.WriteLine(message);
            };

            KissLogConfiguration.Listeners
                .Add(new RequestLogsApiListener(new Application(ConfigurationManager.AppSettings["KissLog.OrganizationId"], ConfigurationManager.AppSettings["KissLog.ApplicationId"]))
                {
                    ApiUrl = ConfigurationManager.AppSettings["KissLog.ApiUrl"],
                    UseAsync = false
                })
                .Add(new LocalTextFileListener("Logs\\onFlush", FlushTrigger.OnFlush))
                .Add(new LocalTextFileListener("Logs\\onMessage", FlushTrigger.OnMessage));
        }
    }
}
