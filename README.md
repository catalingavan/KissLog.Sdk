[![KissLog.net](https://kisslog.net/cdn/KissLog/logos/kisslog-logo-64.png)](https://kisslog.net/)

[![Latest version](https://img.shields.io/nuget/v/KissLog.svg?style=flat-square&label=KissLog)](https://www.nuget.org/packages?q=kisslog) [![Downloads](https://img.shields.io/nuget/dt/KissLog.svg?style=flat-square&label=Downloads)](https://www.nuget.org/packages?q=kisslog)

KissLog represents a powerful logging and monitoring solution for .NET applications.

Some of the main features of KissLog are:

- Automatically captures and logs all the exceptions

- Monitors all the HTTP traffic

- Lightweight, powerful SDK

- Centralized logging using [kisslog.net](https://kisslog.net) or KissLog on-premises integration

Check the [documentation](https://docs.kisslog.net) for a complete list of features.

![KissLog.net centralized logging](https://docs.kisslog.net/_images/centralized-logging.png)

## Framework support

- [.NET Core 2.x](https://docs.kisslog.net/SDK/install-instructions/netcore20.html)
- [.NET Core 3.x](https://docs.kisslog.net/SDK/install-instructions/netcore30.html)
- [ASP.NET WebApi](https://docs.kisslog.net/SDK/install-instructions/aspnet-webapi.html)
- [ASP.NET MVC](https://docs.kisslog.net/SDK/install-instructions/aspnet-mvc.html)
- [Windows / Console apps](https://docs.kisslog.net/SDK/install-instructions/console-applications.html)

## Why KissLog?

KissLog is a logging framework which is focused primarily on web applications behaviour.

For each HTTP request, KissLog automatically captures all the available properties: User Agent, Request Headers, Form Data, Request Body, SessionId, Response Headers, Status Code, Response Body.

All the log messages are grouped per each HTTP request, making it easier to follow the execution details.

## Basic usage

```csharp
using KissLog;

public class HomeController : Controller
{
    private readonly ILogger _logger;
    public HomeController()
    {
        _logger = Logger.Factory.Get();
    }

    public IActionResult Index()
    {
        _logger.Debug("Hello World!");

        return View();
    }
}
```

## Setup

### Logs output

KissLog saves the logs to multiple output locations by using log listeners.

Log listeners are registered at application startup using the `KissLogConfiguration.Listeners` container.

Check the [documentation](https://docs.kisslog.net/SDK/logs-output/index.html) for more logs output examples.

```csharp
namespace MyApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        // [...]

        private void RegisterKissLogListeners()
        {
            // register KissLog.net cloud listener
            KissLogConfiguration.Listeners.Add(new RequestLogsApiListener(new Application("d625d5c8-ef47-4cd5-bf2d-6b0a1fa7fda4", "39bb675d-5c13-4bd8-9b5a-1d368da020a2"))
            {
                ApiUrl = "https://api.kisslog.net"
            });
			
            // register NLog listener
            KissLogConfiguration.Listeners.Add(new NLogTargetListener());
            
            // register MongoDB listener
            KissLogConfiguration.Listeners.Add(new MongoDbListener());
        }
    }
}
```

### Configuration

Logging behavior can be customized by using `KissLogConfiguration.Options` container.

Complete list of configuration options can be found on the [documentation](https://docs.kisslog.net/SDK/configuration/index.html) page.

```csharp
protected void Application_Start()
{
    KissLogConfiguration.Options
        .JsonSerializerSettings.Converters.Add(new StringEnumConverter());

    KissLogConfiguration.Options
        .ShouldLogResponseBody((listener, logArgs, defaultValue) =>
        {
            int responseStatusCode = (int)logArgs.WebProperties.Response.HttpStatusCode;
            return responseStatusCode >= 400;
        });

    KissLogConfiguration.Options
        .AppendExceptionDetails((Exception ex) =>
        {
            // log EntityFramework validation errors
            if (ex is DbEntityValidationException dbException)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("DbEntityValidationException:");

                foreach (var error in dbException.EntityValidationErrors.SelectMany(p => p.ValidationErrors))
                {
                    string message = string.Format("Field: {0}, Error: {1}", error.PropertyName, error.ErrorMessage);
                    sb.AppendLine(message);
                }

                return sb.ToString();
            }

            return null;
        });
}
```

## Samples

Check the [code samples](https://github.com/KissLog-net/KissLog.samples) for more examples of using KissLog.

## Feedback

Please use the [issues](https://github.com/KissLog-net/KissLog.Sdk/issues) section to report bugs, suggestions and general feedback.

## Contributing

All contributions are very welcomed: code, documentation, samples, bug reports, feature requests.

## License

[Apache-2.0](LICENSE.md)
