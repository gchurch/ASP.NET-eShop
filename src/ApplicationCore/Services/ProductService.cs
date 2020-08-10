using Ganges.ApplicationCore.Entities;
using Ganges.ApplicationCore.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ganges.ApplicationCore.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _productRepository.GetProductsAsync();
        }

        public async Task<Product> GetProductAsync(int id)
        {
            return await _productRepository.GetProductAsync(id);
        }

        public async Task<Product> BuyProductAsync(int id)
        {
            return await _productRepository.BuyProductAsync(id);
        }

        public async Task<int> AddProductAsync(Product product)
        {
            // product.Id has to be 0 otherwise there will be an error. This is 
            // because you are not allowed to specify an ID value when adding a product
            // to the database. an ID value will automatically be given to the product.
            product.Id = 0;

            return await _productRepository.AddProductAsync(product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            return await _productRepository.DeleteProductAsync(id);
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            return await _productRepository.UpdateProductAsync(product);
        }
    }
}
