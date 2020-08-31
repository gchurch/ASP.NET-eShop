using Ganges.ApplicationCore.Entities;
using Ganges.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ganges.Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly GangesDbContext _context;

        public ProductRepository(GangesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            // Retreiving the data like this should really be done in a service
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductAsync(int id)
        {
            // SingleOrDefault returns the Product with the specified ID, or returns null if it doesn't exist.
            return await _context.Products.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Product> BuyProductAsync(int id)
        {
            // Find a product with the specified id.
            var product = await _context.Products.SingleOrDefaultAsync(x => x.Id == id);

            if (product != null)
            {
                // Reduce the quantity of the product by 1.
                product.Quantity -= 1;

                // Update the database.
                await _context.SaveChangesAsync();
            }

            return product;
        }

        // Add the product to the database and save the changes
        public async Task<int> AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var productExisted = false;

            var product = await _context.Products.SingleOrDefaultAsync(x => x.Id == id);

            if (product != null)
            {
                productExisted = true;
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }

            return productExisted;
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
