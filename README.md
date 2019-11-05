# ![KissLog logo](https://kisslog.net/cdn/KissLog/logos/32.png) KissLog.net

KissLog represents a powerful logging and monitoring solution for .NET applications.

Some of the main features of KissLog are:

- Automatically logs all the exceptions

- Monitors all the HTTP traffic

- Lightweight, powerful SDK

- Centralized logging using [KissLog.net](https://kisslog.net) cloud or on-premises integration

Check the [documentation](https://docs.kisslog.net) for a complete list of features.

![KissLog.net centralized logging](https://docs.kisslog.net/_images/centralized-logging.png)

## Framework support

- [.NET Core](https://docs.kisslog.net/docs/install-instructions/netcore.html)
- [ASP.NET WebApi](https://docs.kisslog.net/docs/install-instructions/aspnet-webapi.html)
- [ASP.NET MVC](https://docs.kisslog.net/docs/install-instructions/aspnet-mvc.html)
- [Windows / Console apps](https://docs.kisslog.net/docs/install-instructions/console-applications.html)

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

### Register listeners

KissLog saves the logs by using ILogListener listeners.

Listeners are registered at application startup using the `KissLogConfiguration.Listeners` container.

```csharp
protected void Application_Start()
{
    // KissLog.net cloud listener
    KissLogConfiguration.Listeners.Add(new KissLogApiListener(
        new KissLog.Apis.v1.Auth.Application("d625d5c8-ef47-4cd5-bf2d-6b0a1fa7fda4", "39bb675d-5c13-4bd8-9b5a-1d368da020a2")
    ));

    // NLog listener
    KissLogConfiguration.Listeners.Add(new NLogTargetListener());
}
```

### Configuration

`KissLogConfiguration.Options` provides a number of properties and runtime handlers used to customize the logs output.

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

[BSD license](LICENSE.md)
