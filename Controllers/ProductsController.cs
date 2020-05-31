using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ganges.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly List<Product> products = new List<Product>() {
            new Product()
            {
                Id = 1,
                Title = "Table",
                Description = "Glass",
                Seller = "George",
                Price = 100,
                Quantity = 2,
                ImageUrl = "table.png"
            },
            new Product()
            {
                Id = 2,
                Title = "Chair",
                Description = "Wooden",
                Seller = "Kevin",
                Price = 50,
                Quantity = 5,
                ImageUrl = "chair.png"
            },
            new Product()
            {
                Id = 3,
                Title = "Computer",
                Description = "High performance",
                Seller = "James",
                Price = 800,
                Quantity = 1,
                ImageUrl = "computer.png"
            }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            return products;
        }

    }
}
