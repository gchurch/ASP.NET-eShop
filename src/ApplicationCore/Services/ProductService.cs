using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    /// <summary>
    /// The ProductService class allows you to interact with the products.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly Random rand = new Random();

        /// <summary>
        /// Initializes a new instance of the ProductService class.
        /// </summary>
        /// <param name="productRepository"></param>
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Retrieves all of the products from the product repository.
        /// </summary>
        /// <returns>A list of all the products.</returns>
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllProductsAsync();
        }

        /// <summary>
        /// Retrieves a specific product from the product repository.
        /// </summary>
        /// <param name="id">The ID of the desired product.</param>
        /// <returns>The desired product or null if a product with the provided ID doesn't exist.</returns>
        public async Task<Product> GetProductAsync(int id)
        {
            return await _productRepository.GetProductAsync(id);
        }

        /// <summary>
        /// Reduces the quantity of a specific product by 1.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The updated product or null if a product with the specified ID doesn't exist.</returns>
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

        /// <summary>
        /// Selects a random URL from the set of possible URLs.
        /// </summary>
        /// <returns>A string containing one of the possible URLs.</returns>
        private String ChooseRandomImageUrl()
        {
            int randomNumber = rand.Next();
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

        /// <summary>
        /// Adds a product to the product repository. The ID property is ignored.
        /// </summary>
        /// <param name="product">The product to be added to the product repository</param>
        /// <returns>The product that has been added to the product repository.</returns>
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

        /// <summary>
        /// Copies the properties from one product to another product. The ImageUrl property is not copied.
        /// The modified product is then updated in the product repository.
        /// </summary>
        /// <param name="productToModify">The product you want to copy to.</param>
        /// <param name="productToCopy">The product you want to copy from.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Update a product in the product repository.
        /// </summary>
        /// <param name="product">The product that to be updated.</param>
        /// <returns>The updated product.</returns>
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
