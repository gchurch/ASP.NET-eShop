using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly Random rng = new Random();

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllProductsAsync();
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

        private String ChooseRandomImageUrl()
        {
            int randomNumber = rng.Next();
            if (randomNumber % 3 == 0)
            {
                return "book.png";
            }
            else if (randomNumber % 3 == 1)
            {
                return "lamp.png";
            }
            else if (randomNumber % 3 == 2)
            {
                return "duck.png";
            }
            return "";
        }

        public async Task AddProductAsync(Product product)
        {
            if (product != null)
            {
                // product.Id has to be 0 otherwise there will be an error. This is 
                // because you are not allowed to specify an ID value when adding a product
                // to the database. An ID value will automatically be given to the product.
                product.Id = 0;
                product.ImageUrl = ChooseRandomImageUrl();
                await _productRepository.AddProductAsync(product);
            }
        }
   
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

        private async Task CopyProductInfoAndUpdateProduct(Product productToModify, Product productToCopy)
        {
            if (productToModify != null && productToCopy != null)
            {
                productToCopy.Title = productToModify.Title;
                productToCopy.Description = productToModify.Description;
                productToCopy.Seller = productToModify.Seller;
                productToCopy.Price = productToModify.Price;
                productToCopy.Quantity = productToModify.Quantity;
                await _productRepository.UpdateProductAsync(productToCopy);
            }
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            if(product != null) {
                var existingProduct = await GetProductAsync(product.Id);
                await CopyProductInfoAndUpdateProduct(product, existingProduct);
                return existingProduct;
            }
            else
            {
                return product;
            }
        }
    }
}
