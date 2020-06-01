using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Ganges
{
    public class GangesDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=Ganges;Integrated Security=True");
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
