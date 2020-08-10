using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ganges.ApplicationCore.Entities;
using Ganges.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ganges.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService;

        // Adding the ProductService service using dependency injection.
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsAsync()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(products);
        }

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

        // Here I am using the [FromBody] binding source attribute to tell the action method that the id parameter is coming
        // from the body of the request.
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

        // TODO: Error handling
        [HttpPost]
        public async Task<ActionResult<Product>> AddProductAsync([FromBody]Product product)
        {
            if (product != null)
            {
                // Add the product to the database
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

        // If the product exists and is deleted then return a 200 OK status code.
        // If the product doesn't exist return a 404 not found status code.
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

        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProductAsync([FromBody]Product product)
        {
            // A null product is not acceptable
            if (product != null)
            {

                // If updatedProduct is null a product with the supplied id doesn't exist.
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
