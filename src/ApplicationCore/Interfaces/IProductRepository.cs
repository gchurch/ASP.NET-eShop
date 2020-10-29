using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IProductRepository
    {
         Task<IEnumerable<Product>> GetAllProductsAsync();

         Task<Product> GetProductByIdAsync(int id);

         Task AddProductAsync(Product product);

         Task DeleteProductByIdAsync(int id);

         Task UpdateProductAsync(Product product);
    }
}