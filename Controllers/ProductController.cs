using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ganges.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly List<Product> products = new List<Product>() {
            new Product()
            {
                Id = 1,
                Name = "Table",
                Description = "Glass",
                Seller = "George"
            },
            new Product()
            {
                Id = 2,
                Name = "Chair",
                Description = "Wooden",
                Seller = "Kevin"
            }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            return products;
        }

    }
}
