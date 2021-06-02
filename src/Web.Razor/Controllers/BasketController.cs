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
    [ApiController]
    [Route("api/[controller]")]
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
            return View();
        }

        [HttpGet("{ownerId}")]
        [AllowAnonymous]
        public ActionResult<Basket> GetBasket(string ownerId)
        {
            Console.WriteLine("Received request, getting basket");
            Basket basket = _basketService.GetBasket(ownerId);
            Console.WriteLine(basket);
            return Ok(basket);
        }

        [HttpGet("test")]
        [AllowAnonymous]
        public ActionResult Test()
        {
            return Ok();
        }

        [HttpGet("test2")]
        [AllowAnonymous]
        public ActionResult Test(int id)
        {
            return Ok(id);
        }

        [HttpPost("{ownerId}")]
        [AllowAnonymous]
        public ActionResult AddProductToBasket(string ownerId, [FromBody] BasketItem basketItem)
        {
            _basketService.AddProductToBasket(basketItem.ProductId, ownerId);
            return Ok();
        }
    }
}