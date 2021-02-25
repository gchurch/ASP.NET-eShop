using ApplicationCore.Interfaces;
using ClassLibrary;

namespace Web.Angular.Controllers
{
    public class ProductsController : ApiProductsController
    {
        public ProductsController(IProductService productService) : base(productService)
        {
        }
    }
}
