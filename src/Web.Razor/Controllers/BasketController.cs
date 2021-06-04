using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Razor.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IProductService _productService;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<IdentityUser> _userManager;

        public BasketController(
            IBasketService basketService,
            IProductService productService,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager
        )
        {
            _basketService = basketService;
            _productService = productService;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        // GET: /Basket
        public IActionResult Index()
        {
            string userId = _userManager.GetUserId(User);
            Basket basket = _basketService.GetBasket(userId);
            return View(basket);
        }

        // GET: /Basket/Add/1
        [HttpGet]
        public async Task<ActionResult> Add(int id)
        {
            Console.WriteLine("Adding product with ID: " + id + " to basket");
            if (await _productService.DoesProductIdExist(id))
            {
                string userId = _userManager.GetUserId(User);
                _basketService.AddProductToBasket(id, userId);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        // GET: /Basket/Remove/1
        [HttpGet]
        public async Task<ActionResult> Remove(int id)
        {
            Console.WriteLine("Adding product with ID: " + id + " to basket");
            if (await _productService.DoesProductIdExist(id))
            {
                string userId = _userManager.GetUserId(User);
                _basketService.RemoveProductFromBasket(id, userId);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}