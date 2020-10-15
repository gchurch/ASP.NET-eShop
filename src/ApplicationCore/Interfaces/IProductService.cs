using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();

        Task<Product> GetProductAsync(int id);

        Task<Product> BuyProductAsync(int id);

        Task AddProductAsync(Product product);

        Task<bool> DeleteProductAsync(int id);

        Task<Product> UpdateProductAsync(Product newProduct);
    }
}