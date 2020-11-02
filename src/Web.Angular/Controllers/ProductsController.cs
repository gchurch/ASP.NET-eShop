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

        /// <include file='ApiDoc.xml' path='docs/members[@name="ProductsController"]/BuyProductAsync/*'/>
        [HttpPost("buy")]
        public async Task<ActionResult<int>> BuyProductByIdAsync([FromBody]int productId)
        {
            bool doesProductIdExist = await _productService.DoesProductIdExist(productId);

            if (doesProductIdExist == true)
            {
                await _productService.BuyProductByIdAsync(productId);
                Product product = await _productService.GetProductByIdAsync(productId);
                return Ok(product.Quantity);
            }
            else
            {
                return NotFound();
            }
        }

        /// <include file='ApiDoc.xml' path='docs/members[@name="ProductsController"]/AddProductAsync/*'/>
        [HttpPost]
        public async Task<ActionResult<Product>> AddProductAsync([FromBody] Product product)
        {
            await _productService.AddProductAsync(product);
            return CreatedAtAction("GetProductById", new { productId = product.Id }, product);
        }

        /// <include file='ApiDoc.xml' path='docs/members[@name="ProductsController"]/DeleteProductAsync/*'/>
        [HttpDelete("{productId}")]
        public async Task<ActionResult> DeleteProductByIdAsync(int productId)
        {
            bool doesProductIdExist = await _productService.DoesProductIdExist(productId);

            if(doesProductIdExist == true)
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
        public async Task<ActionResult<Product>> UpdateProductAsync([FromBody] Product product)
        {
            bool doesProductIdExist = await _productService.DoesProductIdExist(product.Id);

            if (doesProductIdExist == true)
            {
                await _productService.UpdateProductAsync(product);
                Product updatedProduct = await _productService.GetProductByIdAsync(product.Id);
                return Ok(updatedProduct);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
