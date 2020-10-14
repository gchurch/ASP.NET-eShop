using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Angular.Controllers
{
    /// <summary>
    /// The controller class implementing the API to interact with the products.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService;

        /// <summary>
        /// Creates a new instance of the ProductsController class.
        /// </summary>
        /// <param name="productService"></param>
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get all of the products.
        /// </summary>
        /// <returns>A list of all of the products.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProductsAsync()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        /// <summary>
        /// Get a specific product.
        /// </summary>
        /// <param name="id">The ID of the product</param>
        /// <returns>The requested product.</returns>
        [HttpGet("{id}", Name="GetProduct")]
        public async Task<ActionResult<Product>> GetProductAsync(int id)
        {

            var product = await _productService.GetProductAsync(id);

            // If the product is not found then return a 404 status code.
            if (product == null)
            {
                return NotFound();
            }
            // If the product is found then return a 200 status code along with the product.
            else
            {
                return Ok(product);
            }
        }

        /// <summary>
        /// Reduce the quantity of a specific product by 1.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The requested product with its quantity reduced by 1.</returns>
        [HttpPost("buy")]
        public async Task<ActionResult<int>> BuyProduct([FromBody]int id)
        {
            var product = await _productService.BuyProductAsync(id);

            if (product == null)
            {
                // Respond withe a 404 status code.
                return NotFound();
            }
            else
            {
                // Respond with a 200 status code.
                return Ok(product.Quantity);
            }
        }

        /// <summary>
        /// Add a product to the list of products.
        /// </summary>
        /// <remarks>The ID supplied in the product will be ignored.</remarks>
        /// <param name="product">The product that you want to add.</param>
        /// <returns>The created product.</returns>
        [HttpPost]
        public async Task<ActionResult<Product>> AddProductAsync([FromBody]Product product)
        {
            if (product != null)
            {
                await _productService.AddProductAsync(product);

                // Return a 201 created status code. This also sends the created product
                // in the body of the response and sets the location of the created product 
                // in the response header.
                return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
            }
            else
            {
                return BadRequest("The product cannot be null.");
            }
        }

        /// <summary>
        /// Delete a specific product.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>Ok or NotFound.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProductAsync(int id)
        {
            bool productExisted = await _productService.DeleteProductAsync(id);

            if(productExisted)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Update an already existing product.
        /// </summary>
        /// <remarks>The ID of the product argument should be the ID of the product you want to update.</remarks>
        /// <param name="product">What you want to update the product to.</param>
        /// <returns>The updated product.</returns>
        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProductAsync([FromBody]Product product)
        {
            if (product != null)
            {

                // If updatedProduct is null, a product with the supplied id doesn't exist.
                Product updatedProduct = await _productService.UpdateProductAsync(product);

                if (updatedProduct != null)
                {
                    return Ok(updatedProduct);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest("The product cannot be null.");
            }
        }
    }
}
