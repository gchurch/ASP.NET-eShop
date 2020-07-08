using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ganges
{
    public interface IProductService
    {
        public Task<IEnumerable<Product>> GetProductsAsync();

        public Task<Product> GetProductAsync(int id);

        public Task<Product> BuyProductAsync(int id);

        public Task<int> AddProductAsync(Product product);

        public Task DeleteProductAsync(int id);

        public Task UpdateProductAsync(Product newProduct);
    }
}