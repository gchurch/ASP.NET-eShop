using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Ganges.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpPost("buy")]
        // Here I am using the [FromBody] binding source attritube to tell the action method that the id parameter is coming
        // from the body of the request.
        public async Task<ActionResult> BuyProduct([FromBody]int id)
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

        [HttpPost]
        // TODO: Error handling
        public async Task<ActionResult> AddProductAsync([FromBody]Product product)
        {
            // product.Id has to be 0 otherwise there will be an error. This is 
            // because you are not allowed to specify an ID value. an ID value 
            // will automatically be given to the product.
            product.Id = 0;
            
            // Add the product to the database
            await _productService.AddProductAsync(product);
    
            // Return a 201 created status code. This also sends the created product
            // in the body of the response and sets the location of the created product 
            // in the response header.
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpDelete("{id}")]
        // TODO: Give better status code response(s). Add error handling.
        public async Task<ActionResult> DeleteProductAsync(int id)
        {
            await _productService.DeleteProductAsync(id);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProductAsync(int id, [FromBody]Product product)
        {
            product.Id = id;

            await _productService.UpdateProductAsync(product);

            return Ok();
        }
    }
}
