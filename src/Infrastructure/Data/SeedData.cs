using Ganges.ApplicationCore.Entities;
using Ganges.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ganges.Infrastructure.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new GangesDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<GangesDbContext>>()))
            {
                // Look for any products.
                if (context.Products.Any())
                {
                    return;     // DB has been seeded
                }

                context.Products.AddRange(
                    new Product()
                    {
                        Title = "Toy",
                        Description = "Plastic",
                        Seller = "Michael",
                        Price = 50,
                        Quantity = 2,
                        ImageUrl = "toy.png"
                    },
                    new Product()
                    {
                        Title = "Book",
                        Description = "Hard back",
                        Seller = "Peter",
                        Price = 25,
                        Quantity = 4,
                        ImageUrl = "book.png"
                    },
                    new Product()
                    {
                        Title = "Lamp",
                        Description = "Bright",
                        Seller = "David",
                        Price = 75,
                        Quantity = 1,
                        ImageUrl = "lamp.png"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
