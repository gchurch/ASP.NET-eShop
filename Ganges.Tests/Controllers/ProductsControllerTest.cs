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
            var products = new List<Product>();
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(x => x.GetProductsAsync())
                .ReturnsAsync(products);
            var productsController = new ProductsController(productServiceMock.Object);

            // Act
            var result = await productsController.GetProductsAsync();

            // Assert
            result.ShouldBeOfType<ActionResult<IEnumerable<Product>>>();
            result.Result.ShouldBeOfType<OkObjectResult>();
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
            var result = await productsController.GetProductAsync(id);

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
            var result = await productsController.BuyProduct(id);

            // Assert
            result.ShouldBeOfType<OkObjectResult>();
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
            var result = await productsController.BuyProduct(id);

            // Assert
            result.ShouldBeOfType<NotFoundResult>();
        }
    }
}
