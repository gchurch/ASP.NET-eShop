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
            var product = await _productRepository.GetProductAsync(id);

            if(product != null)
            {
                product.Quantity -= 1;
                await _productRepository.UpdateProductAsync(product);
            }

            return product;
        }

        public async Task AddProductAsync(Product product)
        {
            if (product != null)
            {
                // product.Id has to be 0 otherwise there will be an error. This is 
                // because you are not allowed to specify an ID value when adding a product
                // to the database. an ID value will automatically be given to the product.
                product.Id = 0;

                await _productRepository.AddProductAsync(product);
            }
        }

        /// <summary>
        /// Deletes a specified product.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>
        /// Returns true if the given product existed and has now been deleted.
        /// Returns false if the given product does not exist.
        /// </returns>
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetProductAsync(id);

            if(product != null)
            {
                await _productRepository.DeleteProductAsync(product);
                return true;
            }

            return false;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            if (product != null)
            {
                await _productRepository.UpdateProductAsync(product);
            }

            return product;
        }
    }
}
