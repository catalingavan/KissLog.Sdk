# KissLog.Net

KissLog represents a lightweight, adaptive and straightforward solution to integrate logging and error handling in .NET applications.

<br>

Some of the main features of KissLog are:

:small_blue_diamond: Centralized Logging, Diagnostics and Exceptions Handling

:small_blue_diamond: Automatically captures all the the requests, including unhandled exceptions.

:small_blue_diamond: It is easy to install and configure, even for existing - legacy applications.

:small_blue_diamond: Provides ready-to-use [KissLog.net](https://kisslog.net) cloud or on-premises integration.

<br>

Please check the [Wiki page](https://github.com/catalingavan/KissLog-net/wiki) for a complete documentation.

<br>

**[Basic Usage](https://github.com/catalingavan/KissLog-net/wiki/Basic-Usage)**

| Level  | Usage |
| :--- | :--- |
| Trace  | `_logger.Trace("Database connection opened");`  |
| Debug  | `_logger.Debug("Two factor authentication started");`  |
| Information  | `_logger.Info($"Recover password email sent for email {emailAddress}");`  |
| Warning  | `_logger.Warn($"Cache entry for {key} was not found");`  |
| Error  | `_logger.Error($"User with Id = {userId} was not found");` <br> `_logger.Error(ex);`  |
| Critical  | `_logger.Critical("There is not enough space on the disk. Save failed.");`  |

When a Log message is created, source file, method name and line number are automatically captured.

Example:

```C
MyApplication.Services.ProductsService.cs CreateProduct : 17
[Info] Product inserted in database

MyApplication.Services.ProductsService.cs CreateProduct : 21
[Info] Sending confirmation email to user demo@example.com

MyApplication.Services.ProductsService.cs CreateProduct : 25
[Info] Confirmation email successfully sent
```

<br>

**[Requests Tracking](https://github.com/catalingavan/KissLog-net/wiki/Requests-Tracking)**

Logging only Exceptions is not always a complete solution.

Inconsistent application behaviour can be determined by observing all the requests, in general.

KissLog captures all the information for a specific HttpRequest, regardless if it was successful or not.

For each requst, we capture all the relevant information required to debug an issue.

| General | Request | Response | Log Messages |
| :--- | :--- | :--- | :--- |
| UserAgent <br> HttpMethod <br> Uri <br> IP Address <br> MachineName <br> StartTime <br> EndTime <br>Duration | Headers <br> Cookies <br> QueryString <br> FormData <br> InputStream <br> ServerVariables <br> Claims | HttpStatusCode <br> Headers | _Log messages_ <br> _Unhandled Exceptions_ |

<br>

**[Frameworks support](https://github.com/catalingavan/KissLog-net/wiki/Install-Instructions)**

- AspNetCore
- AspNet Mvc
- AspNet WebApi
- Windows Services / Console Applications

<br>

**[Log Targets](https://github.com/catalingavan/KissLog-net/wiki/Listeners)**

You can have any number of Log targets for an application.

A LogListener represents a class which persists the log messages to a storage location (Text File, Database, Cloud etc).

KissLog comes with built-in listeners to save the data on **Text Files** and on **Cloud**, but, if required, it is easy to create your own custom Listeners.

```csharp
public class MvcApplication : System.Web.HttpApplication
{
    protected void Application_Start()
    {
        KissLogConfiguration.Listeners.AddRange(
        
            // writes on Text Files
            new LocalTextFileListener(),
            
            // writes on Databse, only if request was unsuccessful 
            new DatabaseListener
            {
                MinimumResponseHttpStatusCode = 400
            },
            
            // logs on cloud
            new KissLogApiListener("applicationId")
        );
    }
}
```

Text File

![Text Log output](https://preview.ibb.co/nw9cad/3.png)

Cloud

![Cloud output](https://preview.ibb.co/b7Styy/frame_2.png)

<br>

**[IoC Integration](https://github.com/catalingavan/KissLog-net/wiki/IoC)**

KissLog supports easily integration with most of the IoC frameworks available.

- Ninject

- Autofac

- Unity

<br>

**In code**

```csharp
using KissLog;

public class ProductsService : IProductsService
{
    private readonly ILogger _logger;
    private readonly IProductsRepository _productsRepository;
    public ProductsService(ILogger logger, IProductsRepository productsRepository)
    {
        _logger = logger;
        _productsRepository = productsRepository;
    }

    public void CreateProduct(Product product, User createdBy)
    {
        _productsRepository.Insert(product);
        
        _logger.Info("Product inserted in database");
    
        try
        {
            _logger.Info($"Sending confirmation email to user {createdBy.EmailAddress}");

            EmailNotifications.SendConfirmation(createdBy.EmailAddress, product);

            _logger.Info("Confirmation email successfully sent");
        }
        catch(Exception ex)
        {
            _logger.Error(new Args("Send confirmation email failed with exception", ex));
        }
    }
}
```
