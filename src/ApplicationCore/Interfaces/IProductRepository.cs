using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IProductRepository
    {
         Task<IEnumerable<Product>> GetAllProductsAsync();

         Task<Product> GetProductAsync(int id);

         Task AddProductAsync(Product product);

         Task DeleteProductAsync(Product product);

         Task UpdateProductAsync(Product product);
    }
}