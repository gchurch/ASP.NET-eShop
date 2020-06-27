﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ganges
{
    public class ProductService : IProductService
    {
        private readonly GangesDbContext _context;

        public ProductService(GangesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            //Retreiving the data like this should really be done in a service
            var products = await _context.Products.ToListAsync();
            return products;
        }

        public async Task<Product> GetProductAsync(int id)
        {
            // SingleOrDefault returns the Product with the specified ID, or returns null if it doesn't exist.
            var product = await _context.Products.SingleOrDefaultAsync(x => x.Id == id);
            return product;
        }

        public async Task<Product> BuyProduct(int id)
        {
            // Find a product with the specified id.
            var product = await _context.Products.SingleOrDefaultAsync(x => x.Id == id);

            if(product != null)
            {
                // Reduce the quantity of the product by 1.
                product.Quantity -= 1;

                // Update the database.
                await _context.SaveChangesAsync();
            }

            return product;
        }

    }
}
