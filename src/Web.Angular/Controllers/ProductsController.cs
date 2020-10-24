using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Angular.Controllers
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
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProductsAsync()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        /// <include file='ApiDoc.xml' path='docs/members[@name="ProductsController"]/GetProductAsync/*'/>
        [HttpGet("{id}", Name="GetProduct")]
        public async Task<ActionResult<Product>> GetProductAsync(int id)
        {
            var product = await _productService.GetProductAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(product);
            }
        }

        /// <include file='ApiDoc.xml' path='docs/members[@name="ProductsController"]/BuyProductAsync/*'/>
        [HttpPost("buy")]
        public async Task<ActionResult<int>> BuyProductAsync([FromBody]int id)
        {
            var product = await _productService.GetProductAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                await _productService.BuyProductAsync(id);
                return Ok(product.Quantity);
            }
        }

        /// <include file='ApiDoc.xml' path='docs/members[@name="ProductsController"]/AddProductAsync/*'/>
        [HttpPost]
        public async Task<ActionResult<Product>> AddProductAsync([FromBody]Product product)
        {
            if (product == null)
            {
                return BadRequest("The product cannot be null.");
            }
            else
            {
                await _productService.AddProductAsync(product);
                return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
            }
        }

        /// <include file='ApiDoc.xml' path='docs/members[@name="ProductsController"]/DeleteProductAsync/*'/>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProductAsync(int id)
        {
            var product = await _productService.GetProductAsync(id);

            if(product == null)
            {
                return NotFound();
            }
            else
            {
                await _productService.DeleteProductAsync(id);
                return Ok();
            }
        }

        /// <include file='ApiDoc.xml' path='docs/members[@name="ProductsController"]/UpdateProductAsync/*'/>
        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProductAsync([FromBody]Product product)
        {
            if (product == null)
            {
                return BadRequest("The product cannot be null.");
            }
            else
            {
                var productToUpdate = await _productService.GetProductAsync(product.Id);

                if (productToUpdate != null)
                {
                    await _productService.UpdateProductAsync(product);
                    return Ok(productToUpdate);
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }
}
