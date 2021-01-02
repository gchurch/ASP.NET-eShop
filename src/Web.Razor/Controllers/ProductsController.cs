using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Web.Razor.Authorization;

namespace Web.Razor.Controllers
{
    public class ProductsController : Controller
    {

        private readonly IProductService _productService;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<IdentityUser> _userManager;

        public ProductsController(
            IProductService productService,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager
        )
        {
            _productService = productService;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        // GET: /Products
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        // GET: /Products/Details/1
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            bool doesProductIdExist = await _productService.DoesProductIdExist(id);

            if (doesProductIdExist == true)
            {
                Product product = await _productService.GetProductByIdAsync(id);
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
            product.OwnerID = _userManager.GetUserId(User);

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                User,
                product,
                ProductOperations.Create
            );

            if(!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            await _productService.AddProductAsync(product);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Products/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            bool doesProductIdExist = await _productService.DoesProductIdExist(id);

            if (doesProductIdExist == true)
            {
                Product product = await _productService.GetProductByIdAsync(id);
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
            bool doesProductIdExist = await _productService.DoesProductIdExist(id);

            if(doesProductIdExist == true)
            {
                await _productService.DeleteProductByIdAsync(id);
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
            bool doesProductIdExist = await _productService.DoesProductIdExist(id);

            if(doesProductIdExist == true)
            {
                Product product = await _productService.GetProductByIdAsync(id);
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
            bool doesProductIdExist = await _productService.DoesProductIdExist(product.ProductId);

            if(doesProductIdExist == true)
            {
                Product productToUpdate = await _productService.GetProductByIdAsync(product.ProductId);
                await _productService.UpdateProductAsync(product);
                return RedirectToAction(nameof(Details), new { Id = productToUpdate.ProductId });
            }
            else {
                return NotFound();
            }
        }
    }
}