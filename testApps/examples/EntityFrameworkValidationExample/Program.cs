using EntityFrameworkValidationExample.EntityFramework;
using EntityFrameworkValidationExample.Models;
using KissLog;
using KissLog.Listeners.FileListener;
using Microsoft.EntityFrameworkCore;
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
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseSqlServer(connectionString);

            var dbContext = new AppDbContext(builder.Options);
            dbContext.Database.EnsureCreated();

            return dbContext;
        }

        private static void CreateProduct(AppDbContext dbContext)
        {
            var product = new Product();
            product.Name = "Product 1";
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

                    if (ex is Microsoft.EntityFrameworkCore.DbUpdateException dbUpdateException)
                    {
                        string entries = string.Join(Environment.NewLine, dbUpdateException.Entries.Select(p => p.ToString()));

                        sb.AppendLine("Affected entries:");
                        sb.Append(entries);
                    }

                    return sb.ToString();
                });

            KissLogConfiguration.Listeners
                .Add(new LocalTextFileListener("logs", FlushTrigger.OnFlush));
        }
    }
}
