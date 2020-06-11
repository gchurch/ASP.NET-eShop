using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ganges
{
    public class ProductService : IProductService
    {
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            //Retreiving the data like this should really be done in a service
            using (var context = new GangesDbContext())
            {
                var products = await context.Products.ToListAsync();
                return products;
            }
        }

        public async Task<Product> GetProductAsync(int id)
        {
            using (var context = new GangesDbContext())
            {
                // SingleOrDefault returns the Product with the specified ID, or returns null if it doesn't exist.
                var product = await context.Products.SingleOrDefaultAsync(x => x.Id == id);
                return product;
            }
        }

        public async Task<Product> BuyProduct(int id)
        {
            using (var context = new GangesDbContext())
            {
                // Find a product with the specified id.
                var product = await context.Products.SingleOrDefaultAsync(x => x.Id == id);

                if(product != null)
                {
                    // Reduce the quantity of the product by 1.
                    product.Quantity -= 1;

                    // Update the database.
                    await context.SaveChangesAsync();
                }

                return product;
            }
        }

    }
}
