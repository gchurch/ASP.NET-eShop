using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ganges
{
    public interface IProductService
    {
        public Task<IEnumerable<Product>> GetProductsAsync();

        public Task<Product> GetProductAsync(int id);

        public Task<Product> BuyProduct(int id);
    }
}