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
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            //Retreiving the data like this should really be done in a service
            using (var context = new GangesDbContext())
            {
                var products = context.Products.ToList();
                return products;
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            using (var context = new GangesDbContext())
            {
                // SingleOrDefault returns the Product with the specified ID, or returns null if it doesn't exist.
                var product = context.Products.SingleOrDefault(x => x.Id == id);

                // If the product is found then return a 200 status code along with the product.
                if (product != null)
                {
                    return Ok(product);
                }
                // If the product is not found then return a 404 status code.
                else
                {
                    return NotFound();
                }
            }
        }

    }
}
