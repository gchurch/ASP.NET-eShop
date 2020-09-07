using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ganges.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.MVC.Controllers
{
    public class ProductsController : Controller
    {

        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProductsAsync();
            return View(products);
        }
    }
}
