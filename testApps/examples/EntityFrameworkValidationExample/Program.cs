using EntityFrameworkValidationExample.EntityFramework;
using EntityFrameworkValidationExample.Models;
using KissLog;
using KissLog.Listeners.FileListener;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Text;

namespace EntityFrameworkValidationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            ConfigureKissLog();

            var logger = new Logger();

            try
            {
                logger.Trace("Creating DbContext");
                AppDbContext dbContext = CreateDbContext(configuration);

                logger.Trace("Creating product");
                CreateProduct(dbContext);
            }
            catch(Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex);
            }
            finally
            {
                Logger.NotifyListeners(logger);
            }

            Console.ReadKey();
        }

        private static AppDbContext CreateDbContext(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("AppDbContext");

            var dbContext = new AppDbContext(connectionString);
            dbContext.Database.CreateIfNotExists();

            return dbContext;
        }

        private static void CreateProduct(AppDbContext dbContext)
        {
            var product = new Product();
            product.Name = "Product 1";
            product.Code = Guid.NewGuid().ToString();
            product.Price = 10.5;

            dbContext.Products.Add(product);
            dbContext.SaveChanges();
        }

        private static void ConfigureKissLog()
        {
            KissLogConfiguration.Options
                .AppendExceptionDetails((Exception ex) =>
                {
                    StringBuilder sb = new StringBuilder();

                    if (ex is System.Data.Entity.Validation.DbEntityValidationException dbException)
                    {
                        sb.AppendLine("Validation exceptions:");

                        foreach (var error in dbException.EntityValidationErrors.SelectMany(p => p.ValidationErrors))
                        {
                            string message = string.Format("Field: {0}, Error: {1}", error.PropertyName, error.ErrorMessage);
                            sb.AppendLine(message);
                        }
                    }

                    return sb.ToString();
                });

            KissLogConfiguration.Listeners
                .Add(new LocalTextFileListener("logs", FlushTrigger.OnFlush));
        }
    }
}
