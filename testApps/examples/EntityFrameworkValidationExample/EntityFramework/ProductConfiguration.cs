using EntityFrameworkValidationExample.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFrameworkValidationExample.EntityFramework
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(256);
            builder.Property(p => p.Code).IsRequired().HasMaxLength(6);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(2000);

            builder.ToTable("Product");
        }
    }
}
