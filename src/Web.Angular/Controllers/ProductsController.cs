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
            IEnumerable<Product> products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        /// <include file='ApiDoc.xml' path='docs/members[@name="ProductsController"]/GetProductAsync/*'/>
        [HttpGet("{productId}")]
        public async Task<ActionResult<Product>> GetProductAsync(int productId)
        {
            bool doesProductExist = await _productService.DoesProductIdExist(productId);

            if (doesProductExist == true)
            {
                Product product = await _productService.GetProductAsync(productId);
                return Ok(product);
            }
            else
            {
                return NotFound();
            }
        }

        /// <include file='ApiDoc.xml' path='docs/members[@name="ProductsController"]/BuyProductAsync/*'/>
        [HttpPost("buy")]
        public async Task<ActionResult<int>> BuyProductAsync([FromBody]int productId)
        {
            bool doesProductExist = await _productService.DoesProductIdExist(productId);

            if (doesProductExist == true)
            {
                await _productService.BuyProductAsync(productId);
                Product product = await _productService.GetProductAsync(productId);
                return Ok(product.Quantity);
            }
            else
            {
                return NotFound();
            }
        }

        /// <include file='ApiDoc.xml' path='docs/members[@name="ProductsController"]/AddProductAsync/*'/>
        [HttpPost]
        public async Task<ActionResult<Product>> AddProductAsync([FromBody]Product product)
        {
            if (product != null)
            {
                await _productService.AddProductAsync(product);
                return CreatedAtAction(nameof(GetProductAsync), new { id = product.Id }, product);
            }
            else
            {
                return BadRequest("The product cannot be null.");
            }
        }

        /// <include file='ApiDoc.xml' path='docs/members[@name="ProductsController"]/DeleteProductAsync/*'/>
        [HttpDelete("{productId}")]
        public async Task<ActionResult> DeleteProductAsync(int productId)
        {
            bool doesProductExist = await _productService.DoesProductIdExist(productId);

            if(doesProductExist == true)
            {
                await _productService.DeleteProductAsync(productId);
                return Ok();
            }
            else
            {
                return NotFound();
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
                bool doesProductExist = await _productService.DoesProductIdExist(product.Id);

                if (doesProductExist == true)
                {
                    await _productService.UpdateProductAsync(product);
                    Product updatedProduct = await _productService.GetProductAsync(product.Id);
                    return Ok(updatedProduct);
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }
}
