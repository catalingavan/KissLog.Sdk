# KissLog.Net

KissLog represents a lightweight, adaptive and straightforward solution to integrate logging and error handling in .NET applications.

<br>

Some of the main features of KissLog are:

:small_blue_diamond: It is lightweight. The only required dependency is `Newtonsoft.Json`

:small_blue_diamond: It is easy to install and configure, even for existing - legacy applications.

:small_blue_diamond: Automatically captures all the the requests, including unhandled exceptions.

:small_blue_diamond: You have full control over data.

:small_blue_diamond: Provides ready-to-use [KissLog.net](https://kisslog.net) cloud or on-premises integration.

<br>

**Basic Usage**

| Level  | Usage |
| :--- | :--- |
| Trace  | `_logger.Trace("Database connection opened");`  |
| Debug  | `_logger.Debug("Two factor authentication started");`  |
| Information  | `_logger.Info($"Recover password email sent for email {emailAddress}");`  |
| Warning  | `_logger.Warn($"Cache entry for {key} was not found");`  |
| Error  | `_logger.Error($"User with Id = {userId} was not found");` <br> `_logger.Error(ex);`  |
| Critical  | `_logger.Critical("There is not enough space on the disk. Save failed.");`  |

<br>

Please check the [Wiki page](https://github.com/catalingavan/KissLog-net/wiki) for a complete documentation.
