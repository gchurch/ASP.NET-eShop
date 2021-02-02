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

            if (doesProductIdExist)
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

            bool isUserAuthorized = await IsUserAuthorizedAsync(
                product,
                ProductOperations.Create
            );

            if(isUserAuthorized)
            {
                await _productService.AddProductAsync(product);
                return RedirectToAction(nameof(Details), new { id = product.ProductId });
            }
            else
            {
                return Forbid();
            }
        }

        // GET: /Products/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            bool doesProductIdExist = await _productService.DoesProductIdExist(id);

            if (doesProductIdExist)
            {
                Product product = await _productService.GetProductByIdAsync(id);

                bool isUserAuthorized = await IsUserAuthorizedAsync(
                    product,
                    ProductOperations.Delete
                );

                if(isUserAuthorized)
                {
                    return View(product);
                }
                else
                {
                    return Forbid();
                }
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

            if(doesProductIdExist)
            {
                Product product = await _productService.GetProductByIdAsync(id);
                
                bool isUserAuthorized = await IsUserAuthorizedAsync(
                    product,
                    ProductOperations.Delete
                );

                if(isUserAuthorized)
                {
                    await _productService.DeleteProductByIdAsync(id);
                    return RedirectToAction(nameof(Index));
                } 
                else
                {
                    return Forbid();
                }
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

            if(doesProductIdExist)
            {
                var product = await _productService.GetProductByIdAsync(id);

                bool isUserAuthorized = await IsUserAuthorizedAsync(
                    product,
                    ProductOperations.Update
                );

                if(isUserAuthorized)
                {
                    return View(product);
                }
                else
                {
                    return Forbid();
                }
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

            if(doesProductIdExist)
            {
                Product productToUpdate = await _productService.GetProductByIdAsync(product.ProductId);

                bool isUserAuthorized = await IsUserAuthorizedAsync(
                    productToUpdate,
                    ProductOperations.Update
                );

                if(isUserAuthorized)
                {
                    await _productService.UpdateProductAsync(product);
                    return RedirectToAction(nameof(Details), new { Id = product.ProductId });
                }
                else
                {
                    return Forbid();
                }
            }
            else {
                return NotFound();
            }
        }

        private async Task<bool> IsUserAuthorizedAsync(Product product, IAuthorizationRequirement operation)
        {
            AuthorizationResult isAuthorized = await _authorizationService.AuthorizeAsync(
                    User,
                    product,
                    operation
                );
            return isAuthorized.Succeeded;
        }
    }
}