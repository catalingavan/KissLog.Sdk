using EntityFrameworkValidationExample.Models;
using System.Data.Entity;

namespace EntityFrameworkValidationExample.EntityFramework
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(string nameOrConnectionString) :
            base(nameOrConnectionString)
        {

        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ProductConfiguration());
        }
    }
}
