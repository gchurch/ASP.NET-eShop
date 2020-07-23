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
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(x => x.GetProductsAsync())
                .ReturnsAsync(new List<Product>())
                .Verifiable();
            var productsController = new ProductsController(productServiceMock.Object);

            // Act
            var actionResult = await productsController.GetProductsAsync();

            // Assert
            actionResult.ShouldBeOfType<ActionResult<IEnumerable<Product>>>();
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
            productServiceMock.Verify();
        }


        [TestMethod]
        public async Task GetProductAsync_GivenExistingProductId_ShouldReturnOk()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            // Return a product, as if the ID exists
            productServiceMock.Setup(x => x.GetProductAsync(It.IsAny<int>()))
                .ReturnsAsync(new Product())
                .Verifiable();
            var productsController = new ProductsController(productServiceMock.Object);

            // Act
            var actionResult = await productsController.GetProductAsync(0);

            // Assert
            actionResult.ShouldBeOfType<ActionResult<Product>>();
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
            productServiceMock.Verify();
        }

        [TestMethod]
        public async Task GetProductAsync_GivenNonExistentProductId_ShouldReturnNotFound()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            // Return null, as if the product doesn't exist
            productServiceMock.Setup(x => x.GetProductAsync(It.IsAny<int>()))
                .ReturnsAsync((Product) null)
                .Verifiable();
            var productsController = new ProductsController(productServiceMock.Object);

            // Act
            var result = await productsController.GetProductAsync(0);

            // Assert
            result.ShouldBeOfType<ActionResult<Product>>();
            result.Result.ShouldBeOfType<NotFoundResult>();
            productServiceMock.Verify();
        }

        [TestMethod]
        public async Task BuyProduct_GivenExistingProductId_ShouldReturnOk()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(x => x.BuyProductAsync(It.IsAny<int>()))
                .ReturnsAsync(new Product())
                .Verifiable();
            var productsController = new ProductsController(productServiceMock.Object);

            // Act
            var actionResult = await productsController.BuyProduct(0);
            var value = (actionResult.Result as OkObjectResult).Value;

            // Assert
            actionResult.ShouldBeOfType<ActionResult<int>>();
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
            productServiceMock.Verify();
        }

        [TestMethod]
        public async Task BuyProduct_GivenNonExistentProductId_ShouldReturnNotFound()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(x => x.BuyProductAsync(It.IsAny<int>()))
                .ReturnsAsync((Product) null)
                .Verifiable();
            var productsController = new ProductsController(productServiceMock.Object);

            // Act
            var actionResult = await productsController.BuyProduct(0);

            // Assert
            actionResult.ShouldBeOfType<ActionResult<int>>();
            actionResult.Result.ShouldBeOfType<NotFoundResult>();
            productServiceMock.Verify();
        }

        [TestMethod]
        public async Task AddProductAsync_GivenProduct_ShouldReturnCreated()
        {
            // Arrange
            var product = new Product();
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(x => x.AddProductAsync(product))
                .ReturnsAsync(1)
                .Verifiable();
            var productsController = new ProductsController(productServiceMock.Object);

            // Act
            var actionResult = await productsController.AddProductAsync(product);

            // Assert
            actionResult.ShouldBeOfType<ActionResult<Product>>();
            actionResult.Result.ShouldBeOfType<CreatedAtRouteResult>();
            productServiceMock.Verify();
        }

        [TestMethod]
        public async Task DeleteProductAsync_GivenExistingId_ShouldReturnOk()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            // Return true, as if the ID exists
            productServiceMock.Setup(x => x.DeleteProductAsync(It.IsAny<int>()))
                .ReturnsAsync(true)
                .Verifiable();
            var productsController = new ProductsController(productServiceMock.Object);

            // Act
            var actionResult = await productsController.DeleteProductAsync(0);

            // Assert
            actionResult.ShouldBeOfType<OkResult>();
            productServiceMock.Verify();
            
        }

        [TestMethod]
        public async Task DeleteProductAsync_GivenNoneExistantId_ShouldReturnNotFound()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            // Return false, as if the ID doesn't exist
            productServiceMock.Setup(x => x.DeleteProductAsync(It.IsAny<int>()))
                .ReturnsAsync(false)
                .Verifiable();
            var productsController = new ProductsController(productServiceMock.Object);

            // Act
            var actionResult = await productsController.DeleteProductAsync(0);

            // Assert
            actionResult.ShouldBeOfType<NotFoundResult>();
            productServiceMock.Verify();

        }

        [TestMethod]
        public async Task UpdateProductAsync_GivenProductAndExistingId_ShouldReturnOk()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            // UpdateProductAsync returns the updated product when the id exists
            productServiceMock.Setup(x => x.UpdateProductAsync(It.IsAny<int>(), It.IsAny<Product>()))
                .ReturnsAsync(new Product())
                .Verifiable();
            var productsController = new ProductsController(productServiceMock.Object);

            // Act
            var actionResult = await productsController.UpdateProductAsync(0, new Product());

            // Assert
            actionResult.ShouldBeOfType<ActionResult<Product>>();
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
            productServiceMock.Verify();
        }

        [TestMethod]
        public async Task UpdateProductAsync_GivenProductAndNonExistentId_ShouldReturnNotFound()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            // UpdateProductAsync returns null when the id doesn't exist.
            productServiceMock.Setup(x => x.UpdateProductAsync(It.IsAny<int>(), It.IsAny<Product>()))
                .ReturnsAsync((Product)null)
                .Verifiable();
            var productsController = new ProductsController(productServiceMock.Object);

            // Act
            var actionResult = await productsController.UpdateProductAsync(0, new Product());

            // Assert
            actionResult.ShouldBeOfType<ActionResult<Product>>();
            actionResult.Result.ShouldBeOfType<NotFoundResult>();
            productServiceMock.Verify();
        }
    }
}
