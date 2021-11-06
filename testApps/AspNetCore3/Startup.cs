using KissLog;
using KissLog.AspNetCore;
using KissLog.CloudListeners.Auth;
using KissLog.CloudListeners.RequestLogsListener;
using KissLog.Formatters;
using KissLog.Listeners.FileListener;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AspNetCore3
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddSession();

            services.AddLogging(logging =>
            {
                logging.AddKissLog(options =>
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

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseKissLogMiddleware(options => ConfigureKissLog(options));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void ConfigureKissLog(IOptionsBuilder options)
        {
            // optional KissLog configuration
            options.Options
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
            options.InternalLog = (message) =>
            {
                Debug.WriteLine(message);
            };

            // register listeners
            KissLogConfiguration.Listeners
                .Add(new RequestLogsApiListener(new Application(Configuration["KissLog.OrganizationId"], Configuration["KissLog.ApplicationId"]))
                {
                    ApiUrl = Configuration["KissLog.ApiUrl"],
                    Interceptor = new StatusCodeInterceptor
                    {
                        MinimumLogMessageLevel = LogLevel.Trace,
                        MinimumResponseHttpStatusCode = 200
                    }
                })
                .Add(new LocalTextFileListener("Logs\\onFlush", FlushTrigger.OnFlush))
                .Add(new LocalTextFileListener("Logs\\onMessage", FlushTrigger.OnMessage));
        }
    }
}
