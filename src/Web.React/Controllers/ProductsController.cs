using ApplicationCore.Interfaces;
using ClassLibrary;

namespace Web.React.Controllers
{
    public class ProductsController : ApiProductsController
    {
        public ProductsController(IProductService productService) : base(productService)
        {
        }
    }
}
