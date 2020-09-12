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
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProductsAsync();
            return View(products);
        }

        // GET: /Products/Details/1
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Products/Create
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            await _productService.AddProductAsync(product);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Products/Delete/5
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

        // POST: /Products/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
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

        // POST: /Products/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            await _productService.UpdateProductAsync(product);
            return RedirectToAction(nameof(Index));
        }
    }
}