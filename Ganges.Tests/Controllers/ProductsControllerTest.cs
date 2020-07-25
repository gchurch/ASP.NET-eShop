using Ganges.Controllers;
using Ganges.Models;
using Ganges.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;

// Helpful pages for unit testing:
// https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
// https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/testing?view=aspnetcore-3.1

namespace Ganges.UnitTests
{
    [TestClass]
    public class ProductsControllerTest
    {

        [TestMethod]
        public async Task GetProductsAsync_ShouldReturnTypeOkObjectResultWithIEnumerableProduct()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            productServiceStub.Setup(ps => ps.GetProductsAsync())
                .ReturnsAsync(new List<Product>())
                .Verifiable();
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.GetProductsAsync();
            var value = (actionResult.Result as OkObjectResult).Value;

            // Assert
            actionResult.ShouldBeOfType<ActionResult<IEnumerable<Product>>>();
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
            value.ShouldBeOfType<List<Product>>();
            productServiceStub.Verify();
        }


        [TestMethod]
        public async Task GetProductAsync_GivenProductIdThatExists_ShouldReturnTypeOkObjectResultWithProduct()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productIdThatExists = 0;
            productServiceStub.Setup(ps => ps.GetProductAsync(productIdThatExists))
                .ReturnsAsync(new Product())
                .Verifiable();
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.GetProductAsync(productIdThatExists);
            var value = (actionResult.Result as OkObjectResult).Value;

            // Assert
            actionResult.ShouldBeOfType<ActionResult<Product>>();
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
            value.ShouldBeOfType<Product>();
            productServiceStub.Verify();
        }

        [TestMethod]
        public async Task GetProductAsync_GivenProductIdThatDoesNotExist_ShouldReturnTypeNotFoundResult()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productIdThatDoesNotExist = 0;
            productServiceStub.Setup(ps => ps.GetProductAsync(productIdThatDoesNotExist))
                .ReturnsAsync((Product) null)
                .Verifiable();
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var result = await productsController.GetProductAsync(productIdThatDoesNotExist);

            // Assert
            result.ShouldBeOfType<ActionResult<Product>>();
            result.Result.ShouldBeOfType<NotFoundResult>();
            productServiceStub.Verify();
        }

        [TestMethod]
        public async Task BuyProduct_GivenProductIdThatExists_ShouldReturnTypeOkObjectResultWithInt()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productIdThatExists = 0;
            productServiceStub.Setup(ps => ps.BuyProductAsync(productIdThatExists))
                .ReturnsAsync(new Product())
                .Verifiable();
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.BuyProduct(productIdThatExists);
            var value = (actionResult.Result as OkObjectResult).Value;

            // Assert
            actionResult.ShouldBeOfType<ActionResult<int>>();
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
            value.ShouldBeOfType<int>();
            productServiceStub.Verify();
        }

        [TestMethod]
        public async Task BuyProduct_GivenProductIdThatDoesNotExist_ShouldReturnTypeNotFoundResult()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productIdThatDoesNotExist = 0;
            productServiceStub.Setup(ps => ps.BuyProductAsync(productIdThatDoesNotExist))
                .ReturnsAsync((Product) null)
                .Verifiable();
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.BuyProduct(productIdThatDoesNotExist);

            // Assert
            actionResult.ShouldBeOfType<ActionResult<int>>();
            actionResult.Result.ShouldBeOfType<NotFoundResult>();
            productServiceStub.Verify();
        }

        [TestMethod]
        public async Task AddProductAsync_GivenProduct_ShouldReturnTypeCreatedAtRouteResultWithProduct()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            productServiceStub.Setup(ps => ps.AddProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(1)
                .Verifiable();
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.AddProductAsync(new Product());
            var value = (actionResult.Result as CreatedAtRouteResult).Value;

            // Assert
            actionResult.ShouldBeOfType<ActionResult<Product>>();
            actionResult.Result.ShouldBeOfType<CreatedAtRouteResult>();
            value.ShouldBeOfType<Product>();
            productServiceStub.Verify();
        }

        [TestMethod]
        public async Task DeleteProductAsync_GivenProductIdThatExists_ShouldReturnTypeOkResult()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productIdThatExists = 0;
            productServiceStub.Setup(ps => ps.DeleteProductAsync(productIdThatExists))
                .ReturnsAsync(true)
                .Verifiable();
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.DeleteProductAsync(productIdThatExists);

            // Assert
            actionResult.ShouldBeOfType<OkResult>();
            productServiceStub.Verify();
            
        }

        [TestMethod]
        public async Task DeleteProductAsync_GivenProductIdThatDoesNotExist_ShouldReturnTypeNotFoundResult()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productIdThatDoesNotExist = 0;
            productServiceStub.Setup(ps => ps.DeleteProductAsync(productIdThatDoesNotExist))
                .ReturnsAsync(false)
                .Verifiable();
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.DeleteProductAsync(productIdThatDoesNotExist);

            // Assert
            actionResult.ShouldBeOfType<NotFoundResult>();
            productServiceStub.Verify();

        }

        [TestMethod]
        public async Task UpdateProductAsync_GivenProductIdThatExistsAndProduct_ShouldReturnTypeOkObjectResultAndProduct()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            // UpdateProductAsync returns the updated product when the id exists
            var productIdThatExists = 0;
            productServiceStub.Setup(ps => ps.UpdateProductAsync(productIdThatExists, It.IsAny<Product>()))
                .ReturnsAsync(new Product())
                .Verifiable();
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.UpdateProductAsync(productIdThatExists, new Product());
            var value = (actionResult.Result as OkObjectResult).Value;

            // Assert
            actionResult.ShouldBeOfType<ActionResult<Product>>();
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
            value.ShouldBeOfType<Product>();
            productServiceStub.Verify();
        }

        [TestMethod]
        public async Task UpdateProductAsync_GivenProductIdThatDoesNotExistAndProduct_ShouldReturnTypeNotFoundResult()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productIdThatDoesNotExist = 0;
            productServiceStub.Setup(ps => ps.UpdateProductAsync(productIdThatDoesNotExist, It.IsAny<Product>()))
                .ReturnsAsync((Product)null)
                .Verifiable();
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.UpdateProductAsync(productIdThatDoesNotExist, new Product());

            // Assert
            actionResult.ShouldBeOfType<ActionResult<Product>>();
            actionResult.Result.ShouldBeOfType<NotFoundResult>();
            productServiceStub.Verify();
        }
    }
}
