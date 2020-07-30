using Ganges.ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ganges.ApplicationCore.Interfaces
{
    public interface IProductRepository
    {
        public Task<IEnumerable<Product>> GetProductsAsync();

        public Task<Product> GetProductAsync(int id);

        public Task<Product> BuyProductAsync(int id);

        public Task<int> AddProductAsync(Product product);

        public Task<bool> DeleteProductAsync(int id);

        public Task<Product> UpdateProductAsync(int id, Product newProduct);
    }
}