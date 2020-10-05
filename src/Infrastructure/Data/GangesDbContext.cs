using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class GangesDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public GangesDbContext(DbContextOptions<GangesDbContext> options)
            : base(options)
        {
        }
    }
}