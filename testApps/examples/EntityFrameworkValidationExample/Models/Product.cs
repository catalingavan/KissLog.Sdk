using System;

namespace EntityFrameworkValidationExample.Models
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public Product()
        {
            Id = Guid.NewGuid();
        }
    }
}
