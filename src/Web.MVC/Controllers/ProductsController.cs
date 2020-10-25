using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Entities;

namespace Web.MVC.Controllers
{
    public class ProductsController : Controller
    {

        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: /Products
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        // GET: /Products/Details/1
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            bool doesProductExist = await _productService.DoesProductIdExist(id);

            if (doesProductExist == true)
            {
                Product product = await _productService.GetProductAsync(id);
                return View(product);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: /Products/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Products/Create
        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreatePost(Product product)
        {
            await _productService.AddProductAsync(product);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Products/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            bool doesProductExist = await _productService.DoesProductIdExist(id);

            if (doesProductExist == true)
            {
                Product product = await _productService.GetProductAsync(id);
                return View(product);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: /Products/Delete
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int id)
        {
            bool doesProductExist = await _productService.DoesProductIdExist(id);

            if(doesProductExist == true)
            {
                await _productService.DeleteProductAsync(id);
                return RedirectToAction(nameof(Index));
            }
            else {
                return NotFound();
            }
        }

        // GET: /Products/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            bool doesProductExist = await _productService.DoesProductIdExist(id);

            if(doesProductExist == true)
            {
                Product product = await _productService.GetProductAsync(id);
                return View(product);
            }
            else
            {
                 return NotFound();
            }
        }

        // POST: /Products/Edit
        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditPost(Product product)
        {
            bool doesProductExist = await _productService.DoesProductIdExist(product.Id);

            if(doesProductExist == true)
            {
                Product productToUpdate = await _productService.GetProductAsync(product.Id);
                await _productService.UpdateProductAsync(product);
                return RedirectToAction(nameof(Details), new { Id = productToUpdate.Id });
            }
            else {
                return NotFound();
            }
        }
    }
}