using Ganges.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ganges.Infrastructure.Data
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
                    Title = "Toy",
                    Description = "Plastic",
                    Seller = "Michael",
                    Price = 50,
                    Quantity = 2,
                    ImageUrl = "toy.png"
                },
                new Product()
                {
                    Id = 2,
                    Title = "Book",
                    Description = "Hard back",
                    Seller = "Peter",
                    Price = 25,
                    Quantity = 4,
                    ImageUrl = "book.png"
                },
                new Product()
                {
                    Id = 3,
                    Title = "Lamp",
                    Description = "Bright",
                    Seller = "David",
                    Price = 75,
                    Quantity = 1,
                    ImageUrl = "lamp.png"
                }
            );
        }
    }
}
