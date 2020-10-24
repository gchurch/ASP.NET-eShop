using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();

        Task<Product> GetProductAsync(int id);

        Task BuyProductAsync(int id);

        Task AddProductAsync(Product product);

        Task DeleteProductAsync(int id);

        Task UpdateProductAsync(Product newProduct);
    }
}