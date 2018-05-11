# KissLog.Net

KissLog represents a lightweight, adaptive and straightforward solution to integrate logging and error handling in .NET applications.

<br>

Some of the main features of KissLog are:

:small_blue_diamond: Centralized Logging, Diagnostics and Exceptions Handling

:small_blue_diamond: Automatically captures all the the requests, including unhandled exceptions.

:small_blue_diamond: It is easy to install and configure, even for existing - legacy applications.

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

```
public class ProductsService : IProductsService
{
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

<br>

**Registering Listeners**

A LogListener represents a class which persists the log messages to a storage location (Text File, Database, Cloud etc).

KissLog comes with built-in listeners to save the data on **Text Files** and on **Cloud**, but, if required, it is easy to create your own custom Listeners.

```
public class MvcApplication : System.Web.HttpApplication
{
    protected void Application_Start()
    {
        KissLogConfiguration.Listeners.AddRange(
        
            // writes on Text Files
            new LocalTextFileListener(),
            
            // writes on Databse, only if request was unsuccessfully 
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

Please check the [Wiki page](https://github.com/catalingavan/KissLog-net/wiki) for a complete documentation.
