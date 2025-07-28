using Api.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class ProductCatalogContext : DbContext
    {
        public ProductCatalogContext(DbContextOptions<ProductCatalogContext> options)
        : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = null!;
    }
}