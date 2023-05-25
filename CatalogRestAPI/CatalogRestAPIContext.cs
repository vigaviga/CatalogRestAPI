using CatalogRestAPI.Models.Catalog;
using CatalogRestAPI.Models.Category;
using Microsoft.EntityFrameworkCore;

namespace CatalogRestAPI
{
    public class CatalogRestAPIContext : DbContext
    {
        public CatalogRestAPIContext(DbContextOptions<CatalogRestAPIContext> options) 
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Catalog> Catalogs { get; set; } = null!;
    }
}
