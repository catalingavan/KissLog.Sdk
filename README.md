# KissLog-net
KissLog is a lightweight, customizable and powerful Logging tool for .NET

Some of the main features of KissLog are:

:small_blue_diamond: It is lightweight. The only required dependency is `Newtonsoft.Json`

:small_blue_diamond: It is easy to install and configure, even for existing - legacy applications.

:small_blue_diamond: Automatically captures all the unhandled exceptions.

:small_blue_diamond: Tracks all the requests made by an application - successful or unsuccessful.

:small_blue_diamond: You have full control over data.

:small_blue_diamond: Provides ready-to-use [KissLog.net](https://kisslog.net) cloud or on-premises integration.


Basic Usage

| Level  | Usage |
| :--- | :--- |
| Trace  | `_logger.Trace("Database connection opened");`  |
| Debug  | `_logger.Debug("Two factor authentication started");`  |
| Information  | `_logger.Info($"Recover password email sent for email {emailAddress}");`  |
| Warning  | `_logger.Warn($"Cache entry for {key} was not found");`  |
| Error  | `_logger.Error($"User with Id = {userId} was not found");` <br> `_logger.Error(ex);`  |
| Critical  | `_logger.Critical("There is not enough space on the disk. Save failed.");`  |