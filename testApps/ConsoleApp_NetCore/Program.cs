using KissLog;
using KissLog.AspNetCore;
using KissLog.CloudListeners.Auth;
using KissLog.CloudListeners.RequestLogsListener;
using KissLog.Formatters;
using KissLog.Listeners.FileListener;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;

namespace ConsoleApp_NetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.SetFactory(new KissLog.LoggerFactory(new Logger(url: "ConsoleApp/Main")));

            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, configuration);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            ConfigureKissLog(configuration);

            ILogger logger = serviceProvider.GetService<ILogger<Program>>();

            logger.LogTrace("Trace log");
            logger.LogDebug("Debug log");
            logger.LogInformation("Information log");
            logger.LogWarning("Warning log");
            logger.LogError("Error log");
            logger.LogCritical("Critical log");

            logger.LogError(new DivideByZeroException(), "Divide by zero ex");

            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

            logger.LogAsFile($"Text content logged as file. Guid: {Guid.NewGuid()}", "file-01.txt");
            logger.LogFile(file, "appsettings.json");

            logger.AddCustomProperty("CorrelationId", Guid.NewGuid());
            logger.AddCustomProperty("boolean", true);
            logger.AddCustomProperty("date", DateTime.UtcNow);
            logger.AddCustomProperty("integer", 100);

            var loggers = Logger.Factory.GetAll();
            Logger.NotifyListeners(loggers);
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(logging =>
            {
                logging
                    .AddConfiguration(configuration.GetSection("Logging"))
                    .AddKissLog(options =>
                    {
                        options.Formatter = (FormatterArgs args) =>
                        {
                            string message = args.DefaultValue;

                            if (args.Exception == null)
                                return message;

                            string exceptionStr = new ExceptionFormatter().Format(args.Exception, args.Logger);

                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine(message);
                            sb.Append(exceptionStr);

                            return sb.ToString();
                        };
                    });
            });
        }

        private static void ConfigureKissLog(IConfiguration configuration)
        {
            KissLogConfiguration.InternalLog = (message) =>
            {
                Console.WriteLine(message);
            };

            KissLogConfiguration.Listeners
                .Add(new RequestLogsApiListener(new Application(configuration["KissLog.OrganizationId"], configuration["KissLog.ApplicationId"]))
                {
                    ApiUrl = configuration["KissLog.ApiUrl"],
                    UseAsync = false
                })
                .Add(new LocalTextFileListener("Logs\\onFlush", FlushTrigger.OnFlush))
                .Add(new LocalTextFileListener("Logs\\onMessage", FlushTrigger.OnMessage));
        }
    }
}
