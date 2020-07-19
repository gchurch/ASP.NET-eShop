using Ganges.Controllers;
using Ganges.Models;
using Ganges.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System.Threading.Tasks;

// Helpful page for unit testing: https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/testing?view=aspnetcore-3.1

namespace Ganges.UnitTests
{
    [TestClass]
    public class ProductsControllerTest
    {
        [TestMethod]
        public async Task GetProductAsync_ReceivesExistingProductId_ShouldReturnOk()
        {
            // Arrange
            var id = 1;
            var product = new Product()
            {
                Id = id,
                Title = "a",
                Description = "",
                Seller = "",
                Price = 1,
                Quantity = 1,
                ImageUrl = "",
            };
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(x => x.GetProductAsync(id))
                .ReturnsAsync(product);
            var controller = new ProductsController(productServiceMock.Object);

            // Act
            var result = await controller.GetProductAsync(id);

            // Assert
            result.ShouldBeOfType<ActionResult<Product>>();
            result.Result.ShouldBeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task GetProductAsync_ReceivesNonExistentProductId_ShouldReturnNotFound()
        {
            // Arrange
            var id = 0;
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(x => x.GetProductAsync(id))
                .ReturnsAsync((Product)null);
            var productController = new ProductsController(productServiceMock.Object);

            // Act
            var result = await productController.GetProductAsync(id);

            // Assert
            result.ShouldBeOfType<ActionResult<Product>>();
            result.Result.ShouldBeOfType<NotFoundResult>();
        }

    }
}
