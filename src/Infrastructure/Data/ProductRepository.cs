using Ganges.ApplicationCore.Entities;
using Ganges.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

// The repository pattern: https://deviq.com/repository-pattern/

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
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductAsync(int id)
        {
            // SingleOrDefault returns the Product with the specified ID, or returns null if it doesn't exist.
            return await _context.Products.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
