using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();

        Task<Product> GetProductByIdAsync(int id);

        Task AddProductAsync(Product product);

        Task DeleteProductByIdAsync(int id);

        Task UpdateProductAsync(Product newProduct);

        Task<bool> DoesProductIdExist(int productId);
    }
}