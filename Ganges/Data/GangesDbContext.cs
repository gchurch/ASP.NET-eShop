using Ganges.Models;
using Microsoft.EntityFrameworkCore;

namespace Ganges.Data
{
    public class GangesDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public GangesDbContext(DbContextOptions<GangesDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Adding some initial data to the database
            modelBuilder.Entity<Product>().HasData(
                new Product()
                {
                    Id = 1,
                    Title = "Table",
                    Description = "Glass",
                    Seller = "George",
                    Price = 100,
                    Quantity = 2,
                    ImageUrl = "table.png"
                },
                new Product()
                {
                    Id = 2,
                    Title = "Chair",
                    Description = "Wooden",
                    Seller = "Kevin",
                    Price = 50,
                    Quantity = 5,
                    ImageUrl = "chair.png"
                },
                new Product()
                {
                    Id = 3,
                    Title = "Computer",
                    Description = "High performance",
                    Seller = "James",
                    Price = 800,
                    Quantity = 1,
                    ImageUrl = "computer.png"
                }
            );
        }
    }
}
