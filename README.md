# ![KissLog logo](https://kisslog.net/cdn/KissLog/logos/32.png) KissLog.net

KissLog represents a powerful logging and monitoring solution for .NET applications.

Some of the main features of KissLog are:

&#128313; Automatically logs all the exceptions

&#128313; Monitors all the HTTP traffic

&#128313; Lightweight, powerful SDK

&#128313; Centralized logging using [KissLog.net](https://kisslog.net) cloud or on-premises integration

## Framework support

- [.NET Core](https://github.com/KissLog-net/KissLog.Sdk/wiki/Install-Net-Core)
- [ASP.NET WebApi](https://github.com/KissLog-net/KissLog.Sdk/wiki/Install-AspNet-WebApi)
- [ASP.NET MVC](https://github.com/KissLog-net/KissLog.Sdk/wiki/Install-AspNet-Mvc)

Check the [Wiki page](https://github.com/KissLog-net/KissLog.Sdk/wiki) for a complete documentation.

View the [Change log](https://github.com/KissLog-net/KissLog.Sdk/wiki/ChangeLog).

## Table of contents

- [Setup](#Setup)
  - [Register listeners](#register-listeners)
  - [Configuration](#configuration)
- [Usage](#usage)
- [Centralized logging](#centralized-logging)
- [Integration with other loggers](#integration-with-other-loggers)
- [Feedback](#feedback)
- [Contributing](#contributing)
- [License](#license)

---

## Setup

### Register listeners

Listeners are responsible for saving the log messages.

KissLog comes with built-in listeners, and it is easy to create [custom implementations](https://github.com/KissLog-net/KissLog.Sdk/wiki/Custom-output).

```csharp
public Startup(IConfiguration configuration)
{
    // KissLog.net listener
    KissLogConfiguration.Listeners.Add(
        new KissLogApiListener("KissLog_OrganizationId", "KissLog_ApplicationId", "Staging")
    );

    // custom MongoDb listener
    KissLogConfiguration.Listeners.Add(new MongoDbListener());
}
```

### Configuration

`Options` container provides a number of properties and runtime handlers used to customize the logs output.

```csharp
public Startup(IConfiguration configuration)
{
    KissLogConfiguration.Options
        .JsonSerializerSettings.Converters.Add(new StringEnumConverter());

    KissLogConfiguration.Options
        .ShouldLogRequestFormData((ILogListener listener, FlushLogArgs args, string key) =>
        {
            // do not log "CreditCard" parameter
            if (string.Compare(key, "CreditCard", true) == 0)
                return false;

            return true;
        });
}
```

Additional information about the captured exceptions can be logged by using the `AppendExceptionDetails(ex)` handler.

```csharp
public Startup(IConfiguration configuration)
{
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
                    sb.AppendLine($"Field: {error.PropertyName}, Error: {error.ErrorMessage}");
                }

                return sb.ToString();
            }

            return null;
        });
}
```

## Usage

`ILogger` instance is acquired by using the `Logger.Factory.Get()` factory method.

```csharp
public class Service
{
    private readonly ILogger _logger;
    public Service()
    {
        _logger = Logger.Factory.Get();
    }

    public void Foo(string productId, double price)
    {
        _logger.Debug($"Foo begin with args: {productId}, {price}");

        // executing Foo

        _logger.Debug("Foo completed");
    }
}
```

Additionally, you can log files, asynchronously.

```csharp
public class Service
{
    private readonly ILogger _logger;
    public Service()
    {
        _logger = Logger.Factory.Get();
    }

    public void Foo()
    {
        byte[] archive = File.ReadAllBytes(@"C:\Files\bootstrap.zip");
        _logger.LogAsFile(archive, "Bootstrap.zip");

        string path = @"C:\Files\Invoice-16-11-2017.pdf";
        _logger.LogFile(path, "Invoice.pdf");
    }
}
```

## Centralized logging

When using `KissLogApiListener` listener, the logs will be saved to KissLog.net cloud or on-premises application.

```csharp
public Startup(IConfiguration configuration)
{
    KissLogConfiguration.Listeners.Add(new KissLogApiListener(
        Configuration["KissLog.OrganizationId"],
        Configuration["KissLog.ApplicationId"],
        Configuration["Environment"]
    )
    {
        // URI to KissLog.net on-premises application
        ApiUrl = "http://my-kisslog.net"
    });
}
```

## Integration with other loggers

KissLog provides adapters used for saving **NLog** and **log4net** logs to [KissLog.net](https://kisslog.net).

- [NLog](https://github.com/KissLog-net/KissLog.samples/tree/master/src/NLog-integration)

```xml
<nlog>
  <extensions>
    <add assembly="KissLog.Adapters.NLog"/>
  </extensions>
  <targets>
    <target name="kisslog" type="KissLog" layout="${message}" />
  </targets>
  <rules>
    <logger name="*" minlevel="Trace" writeTo="kisslog" />
  </rules>
</nlog>
```

- [log4net](https://github.com/KissLog-net/KissLog.samples/tree/master/src/log4net-integration)

```xml
﻿<log4net>
  <root>
    <level value="ALL" />
    <appender-ref ref="KissLog" />
  </root>
  <appender name="KissLog" type="KissLog.Adapters.log4net.KissLogAppender, KissLog.Adapters.log4net">
    <layout type="log4net.Layout.SimpleLayout" />
  </appender>
</log4net>
```

## Feedback

Please use the [issues](https://github.com/KissLog-net/KissLog.Sdk/issues) section to report bugs, suggestions and general feedback.

## Contributing

All contributions are very welcomed: code, documentation, bug reports, feature requests.

## License

[BSD license](/LICENSE.md)
