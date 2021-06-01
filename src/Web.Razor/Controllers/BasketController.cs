using ApplicationCore.Interfaces;
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
        private readonly IProductService _productService;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<IdentityUser> _userManager;

        public BasketController(
            IProductService productService,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager
        )
        {
            _productService = productService;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}