using ApplicationCore.Interfaces;
using Api;

namespace Web.React.Controllers
{
    public class ProductsController : Api.ProductsController
    {
        public ProductsController(IProductService productService) : base(productService)
        {
        }
    }
}
