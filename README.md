# ![KissLog logo](https://kisslog.net/content/images/kissLogLogos/logo_32.png) KissLog.net

KissLog represents a powerful logging and monitoring solution for .NET applications.

Some of the main features of KissLog are:

&#128313; Centralized Logging, Diagnostics and Error Reporting

&#128313; Automatically captures all the exceptions

&#128313; Provides a lightweight, yet powerfull logging interface for developers

&#128313; Provides ready-to-use [KissLog.net](https://kisslog.net) cloud or on-premises integration

Please check the [Wiki page](https://github.com/catalingavan/KissLog-net/wiki) for a complete documentation.

<br>

**Quick guide**

* [Framework support](#Framework-support)
* [Logging interface](#Logging-interface)
* [Logging files](#Logging-files)
* [Error reporting](#Error-reporting)
* [Requests-tracking](#Requests-tracking)
* [Logs target](#Logs-target)
* [Focused for developers](#Focused-for-developers)

<br>

## Framework support

- [.NET Core](https://github.com/catalingavan/KissLog-net/wiki/Install-Net-Core)
- [AspNet WebApi](https://github.com/catalingavan/KissLog-net/wiki/Install-AspNet-WebApi)
- [AspNet MVC](https://github.com/catalingavan/KissLog-net/wiki/Install-AspNet-Mvc)

## Logging interface

```csharp
_logger.Info($"Recover password email sent for email {emailAddress}");

_logger.Debug(new { Id = 10, Price = 100.4M, Name = "Product 1" });
```

## Logging files

KissLog exposes methods which allows developers to save and log raw data as files.

```csharp
byte[] archive = File.ReadAllBytes(@"C:\Files\bootstrap.zip");
_logger.LogAsFile(archive, "Bootstrap.zip");

string path = @"C:\Files\Invoice-16-11-2017.pdf";
_logger.LogFile(path, "Invoice.pdf");
```

## Error reporting

KissLog captures all the unhandled exceptions.

## Requests tracking

KissLog monitors all the Http requests, regardless if they are successful or not.

## Logs target

KissLog comes with built-in log targets, saving the logs on:

- Local text files
- KissLog cloud / on-premises

Additionally, developers can create [custom output targets](https://github.com/catalingavan/KissLog-net/wiki/Custom-output) for saving the logs.

## Focused for developers

KissLog goal is to create an unobtrusive logging framework for .NET.

With this in mind, KissLog is built on the following principles:

* It is easy to install for existing, legacy applications

* It is lightweight, and it does not bring unnecessary dependencies

* Transparent configuration (we try to avoid confusing xml settings)

* It is highly customisable, being adaptive to application changes and specific scenarios