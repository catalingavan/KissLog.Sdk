using EntityFrameworkValidationExample.Models;
using System.Data.Entity.ModelConfiguration;

namespace EntityFrameworkValidationExample.EntityFramework
{
    internal class ProductConfiguration : EntityTypeConfiguration<Product>
    {
        public ProductConfiguration()
        {
            HasKey(p => p.Id);

            Property(p => p.Name).IsRequired().HasMaxLength(256);
            Property(p => p.Code).IsRequired().HasMaxLength(6);
            Property(p => p.Description).IsRequired().HasMaxLength(2000);

            ToTable("Product");
        }
    }
}
