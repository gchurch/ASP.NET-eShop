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
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<IdentityUser> _userManager;

        public BasketController(
            IBasketService basketService,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager
        )
        {
            _basketService = basketService;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            string userId = _userManager.GetUserId(User);
            Basket basket = _basketService.GetBasket(userId);
            return View(basket);
        }
    }
}