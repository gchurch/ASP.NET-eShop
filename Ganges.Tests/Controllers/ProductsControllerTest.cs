using Ganges.Controllers;
using Ganges.Models;
using Ganges.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;

// Helpful page for unit testing: https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/testing?view=aspnetcore-3.1

namespace Ganges.UnitTests
{
    [TestClass]
    public class ProductsControllerTest
    {

        [TestMethod]
        public async Task GetProductsAsync_ShouldReturnOk()
        {
            // Arrange
            var products = new List<Product>() {
                new Product(),
                new Product()
            };
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(x => x.GetProductsAsync())
                .ReturnsAsync(products);
            var productsController = new ProductsController(productServiceMock.Object);

            // Act
            var actionResult = await productsController.GetProductsAsync();
            var value = (actionResult.Result as OkObjectResult).Value as List<Product>;

            // Assert
            actionResult.ShouldBeOfType<ActionResult<IEnumerable<Product>>>();
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
            value.Count.ShouldBe(2);
        }


        [TestMethod]
        public async Task GetProductAsync_ReceivesExistingProductId_ShouldReturnOk()
        {
            // Arrange
            var id = 1;
            var product = new Product()
            {
                Id = id
            };
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(x => x.GetProductAsync(id))
                .ReturnsAsync(product);
            var productsController = new ProductsController(productServiceMock.Object);

            // Act
            var actionResult = await productsController.GetProductAsync(id);
            var value = (actionResult.Result as OkObjectResult).Value as Product;

            // Assert
            actionResult.ShouldBeOfType<ActionResult<Product>>();
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
            value.ShouldBeOfType<Product>();
        }

        [TestMethod]
        public async Task GetProductAsync_ReceivesNonExistentProductId_ShouldReturnNotFound()
        {
            // Arrange
            var id = 0;
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(x => x.GetProductAsync(id))
                .ReturnsAsync((Product)null);
            var productsController = new ProductsController(productServiceMock.Object);

            // Act
            var result = await productsController.GetProductAsync(id);

            // Assert
            result.ShouldBeOfType<ActionResult<Product>>();
            result.Result.ShouldBeOfType<NotFoundResult>();

        }

        [TestMethod]
        public async Task BuyProduct_ReceivesExistingProductId_ShouldReturnOk()
        {
            // Arrange
            var id = 1;
            var product = new Product()
            {
                Id = id
            };
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(x => x.BuyProductAsync(id))
                .ReturnsAsync(product);
            var productsController = new ProductsController(productServiceMock.Object);

            // Act
            var actionResult = await productsController.BuyProduct(id);
            var value = (actionResult.Result as OkObjectResult).Value;

            // Assert
            actionResult.ShouldBeOfType<ActionResult<int>>();
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
            value.ShouldBeOfType<int>();
        }

        [TestMethod]
        public async Task BuyProduct_ReceivesNonExistentProductId_ShouldNotFound()
        {
            // Arrange
            var id = 0;
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(x => x.BuyProductAsync(id))
                .ReturnsAsync((Product)null);
            var productsController = new ProductsController(productServiceMock.Object);

            // Act
            var actionResult = await productsController.BuyProduct(id);

            // Assert
            actionResult.ShouldBeOfType<ActionResult<int>>();
            actionResult.Result.ShouldBeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task AddProductAsync_RecievesProduct_ShouldReturnCreated()
        {
            // Arrange
            var product = new Product();
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(x => x.AddProductAsync(product))
                .ReturnsAsync(1);
            var productsController = new ProductsController(productServiceMock.Object);

            // Act
            var actionResult = await productsController.AddProductAsync(product);
            var value = (actionResult.Result as CreatedAtRouteResult).Value;

            // Assert
            actionResult.ShouldBeOfType<ActionResult<Product>>();
            actionResult.Result.ShouldBeOfType<CreatedAtRouteResult>();
            value.ShouldBeOfType<Product>();
        }
    }
}
