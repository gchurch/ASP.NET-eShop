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

        [HttpGet("{id}")]
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
            var product = await _productService.BuyProduct(id);

            if (product == null)
            {
                // Respond withe a 404 status code.
                return NotFound();
            }
            else
            {
                // Respond with a 200 status code.
                return Ok();
            }
        }
    }
}
