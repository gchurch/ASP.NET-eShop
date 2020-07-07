using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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

        public async Task<Product> BuyProductAsync(int id)
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

        public async Task<int> AddProductAsync(Product product)
        {
            // Add the product to the database and save the changes
            await _context.Products.AddAsync(product);
            return await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            // This deletes the object with the given ID.
            Product product = new Product() { Id = id };
            _context.Products.Attach(product);
            _context.Products.Remove(product);

            await _context.SaveChangesAsync();
        }

    }
}
