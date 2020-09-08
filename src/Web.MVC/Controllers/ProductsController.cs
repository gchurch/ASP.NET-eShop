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

            if(product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
