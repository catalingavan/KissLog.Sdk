﻿using dotnetcore_3._1_ConsoleApp.Services;
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
using System.Diagnostics;
using System.IO;
using System.Text;

namespace dotnetcore_3._1_ConsoleApp
{
    internal class Program_simple
    {
        static void Main_simple(string[] args)
        {
            Logger.SetFactory(new KissLog.LoggerFactory(new Logger(url: "ConsoleApp/Main")));

            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var serviceProvider = ConfigureServices(configuration);

            ConfigureKissLog(configuration);

            ILogger logger = serviceProvider.GetService<ILogger<Program>>();
            logger.LogTrace("Trace log");
            logger.LogDebug("Debug log");
            logger.LogInformation("Information log");
            logger.LogWarning("Warning log");
            logger.LogError("Error log");
            logger.LogCritical("Critical log");
            logger.LogError(new DivideByZeroException(), "Divide by zero ex");

            IFooService fooService = serviceProvider.GetRequiredService<IFooService>();
            fooService.Foo();

            Console.WriteLine("Hello World!");

            var loggers = Logger.Factory.GetAll();
            Logger.NotifyListeners(loggers);
        }

        static IServiceProvider ConfigureServices(IConfiguration configuration)
        {
            var services = new ServiceCollection();

            services.AddLogging(logging =>
            {
                logging
                    .AddConfiguration(configuration.GetSection("Logging"))
                    .AddSimpleConsole()
                    .AddKissLog(options =>
                    {
                        options.Formatter = (FormatterArgs args) =>
                        {
                            if (args.Exception == null)
                                return args.DefaultValue;

                            string exceptionStr = new ExceptionFormatter().Format(args.Exception, args.Logger);
                            return string.Join(Environment.NewLine, new[] { args.DefaultValue, exceptionStr });
                        };
                    });
            });

            services.AddTransient<IFooService, FooService>();

            return services.BuildServiceProvider();
        }

        static void ConfigureKissLog(IConfiguration configuration)
        {
            KissLogConfiguration.Listeners
                .Add(new RequestLogsApiListener(new Application(configuration["LogBee.OrganizationId"], configuration["LogBee.ApplicationId"]))
                {
                    ApiUrl = configuration["LogBee.ApiUrl"],
                    UseAsync = false
                });

            KissLogConfiguration.Listeners
                .Add(new LocalTextFileListener(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs")));

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
        }
    }
}
