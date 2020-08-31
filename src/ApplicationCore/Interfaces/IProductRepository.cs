using Ganges.ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ganges.ApplicationCore.Interfaces
{
    public interface IProductRepository
    {
         Task<IEnumerable<Product>> GetProductsAsync();

         Task<Product> GetProductAsync(int id);

         Task AddProductAsync(Product product);

         Task DeleteProductAsync(Product product);

         Task UpdateProductAsync(Product product);
    }
}