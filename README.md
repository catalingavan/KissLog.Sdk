[![KissLog.net](https://kisslog.net/cdn/KissLog/logos/kisslog-logo-64.png)](https://kisslog.net/)

[![Latest version](https://img.shields.io/nuget/v/KissLog.svg?style=flat-square&label=KissLog)](https://www.nuget.org/packages?q=kisslog) [![Downloads](https://img.shields.io/nuget/dt/KissLog.svg?style=flat-square&label=Downloads)](https://www.nuget.org/packages?q=kisslog)

KissLog is a lightweight and highly customizable logging and monitoring framework for .NET applications.

Some of the main features of KissLog are:

- Automatically captures and logs all the exceptions

- Logs all the HTTP properties

- Object oriented implementation

- Centralized logging using [kisslog.net](https://kisslog.net) or KissLog on-premises local server

Check the [documentation](https://github.com/KissLog-net/KissLog.Sdk/wiki) for a complete list of features.

![KissLog.net centralized logging](https://docs.kisslog.net/_images/centralized-logging.png)

## Framework support

- [.NET Core Web App](https://github.com/KissLog-net/KissLog.Sdk/wiki/.NET-Core-Web-App)
- [ASP.NET WebApi](https://github.com/KissLog-net/KissLog.Sdk/wiki/ASP.NET-WebApi)
- [ASP.NET MVC](https://github.com/KissLog-net/KissLog.Sdk/wiki/ASP.NET-MVC)
- [Console App (.NET Core)](https://github.com/KissLog-net/KissLog.Sdk/wiki/ConsoleApp-(.NET-Core))
- [Console App (.NET Framework)](https://github.com/KissLog-net/KissLog.Sdk/wiki/ConsoleApp-(.NET-Framework))

## Why KissLog?

KissLog implements three main components: logging functionality, exceptions tracking and application insights.

For web applications, KissLog automatically captures all the HTTP properties.

KissLog keeps the log events in memory and sends them to the registered listeners all at once. This can significantly reduce the load of the persistance logic (such as Disk I/O, database operations or network throughput).

It is unobtrusive, easy to set up and easy to configure.

## Basic usage

```csharp
using KissLog;

public class HomeController : Controller
{
    private readonly IKLogger _logger;
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
            KissLogConfiguration.Listeners
                .Add(new RequestLogsApiListener(new Application("d625d5c8-ef47-4cd5-bf2d-6b0a1fa7fda4", "39bb675d-5c13-4bd8-9b5a-1d368da020a2"))
                {
                    ApiUrl = "https://api.kisslog.net"
                })
                .Add(new LocalTextFileListener("logs", FlushTrigger.OnMessage))
                .Add(new MongoDbListener());
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
        .ShouldLogResponseBody((HttpProperties httpProperties) =>
        {
            int statusCode = httpProperties.Response.StatusCode;
            return statusCode >= 400;
        });

    KissLogConfiguration.Options
        .ShouldLogFormData((OptionsArgs.LogListenerFormDataArgs args) =>
        {
            if (args.FormDataName == "Password")
                return false;

            return true;
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
