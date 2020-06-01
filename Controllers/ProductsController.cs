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

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            //Retreiving the data like this should really be done in a service
            using (var context = new GangesDbContext())
            {
                var products = context.Products.ToList();
                return products;
            }
        }

    }
}
