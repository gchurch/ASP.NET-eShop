using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ganges.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Ganges.ApplicationCore.Entities;

namespace Ganges.Web.MVC.Controllers
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
            var products = await _productService.GetProductsAsync();
            return View(products);
        }

        // GET: /Products/Details/1
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                return View(product);
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
            var product = await _productService.GetProductAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                return View(product);
            }
        }

        // POST: /Products/Delete
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var product = await _productService.GetProductAsync(id);

            if(product == null)
            {
                return NotFound();
            }
            else {
                await _productService.DeleteProductAsync(id);
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: /Products/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductAsync(id);

            if(product == null)
            {
                return NotFound();
            }
            else
            {
                return View(product);
            }
        }

        // POST: /Products/Edit
        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditPost(Product product)
        {
            var readProduct = await _productService.GetProductAsync(product.Id);

            if (readProduct == null)
            {
                return NotFound();
            }
            else
            {
                await _productService.UpdateProductAsync(product);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}