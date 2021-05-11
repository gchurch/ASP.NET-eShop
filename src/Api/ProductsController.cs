using ApplicationCore.Models;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <include file='ApiDoc.xml' path='docs/members[@name="ProductsController"]/GetAllProductsAsync/*'/>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProductsAsync()
        {
            IEnumerable<Product> products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        /// <include file='ApiDoc.xml' path='docs/members[@name="ProductsController"]/GetProductByIdAsync/*'/>
        [HttpGet("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProductByIdAsync(int productId)
        {
            bool doesProductIdExist = await _productService.DoesProductIdExist(productId);

            if (doesProductIdExist == true)
            {
                Product product = await _productService.GetProductByIdAsync(productId);
                return Ok(product);
            }
            else
            {
                return NotFound();
            }
        }

        /// <include file='ApiDoc.xml' path='docs/members[@name="ProductsController"]/AddProductAsync/*'/>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Product>> AddProductAsync([FromBody] Product product)
        {
            await _productService.AddProductAsync(product);
            return CreatedAtAction("GetProductById", new { productId = product.ProductId }, product);
        }

        /// <include file='ApiDoc.xml' path='docs/members[@name="ProductsController"]/DeleteProductByIdAsync/*'/>
        [HttpDelete("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteProductByIdAsync(int productId)
        {
            bool doesProductIdExist = await _productService.DoesProductIdExist(productId);

            if (doesProductIdExist == true)
            {
                await _productService.DeleteProductByIdAsync(productId);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        /// <include file='ApiDoc.xml' path='docs/members[@name="ProductsController"]/UpdateProductAsync/*'/>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> UpdateProductAsync([FromBody] Product product)
        {
            bool doesProductIdExist = await _productService.DoesProductIdExist(product.ProductId);

            if (doesProductIdExist == true)
            {
                await _productService.UpdateProductAsync(product);
                Product updatedProduct = await _productService.GetProductByIdAsync(product.ProductId);
                return Ok(updatedProduct);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
