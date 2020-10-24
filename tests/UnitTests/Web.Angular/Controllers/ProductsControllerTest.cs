using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Web.Angular.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;

// Helpful pages for unit testing:
// https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
// https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/testing?view=aspnetcore-3.1

namespace UnitTests.Web.Angular.Controllers
{
    [TestClass]
    public class ProductsControllerTest
    {

        [TestMethod]
        public async Task GetAllProductsAsync_ShouldReturnTypeOkObjectResultWithIEnumerableProduct()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            productServiceStub.Setup(ps => ps.GetAllProductsAsync())
                .ReturnsAsync(new List<Product>());
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.GetAllProductsAsync();
            var value = (actionResult.Result as OkObjectResult).Value;

            // Assert
            actionResult.ShouldBeOfType<ActionResult<IEnumerable<Product>>>();
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
            value.ShouldBeOfType<List<Product>>();
        }


        [TestMethod]
        public async Task GetProductAsync_GivenProductIdThatExists_ShouldReturnTypeOkObjectResultWithProduct()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productIdThatExists = 0;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);
            productServiceStub.Setup(ps => ps.GetProductAsync(productIdThatExists))
                .ReturnsAsync(new Product());
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.GetProductAsync(productIdThatExists);
            var value = (actionResult.Result as OkObjectResult).Value;

            // Assert
            actionResult.ShouldBeOfType<ActionResult<Product>>();
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
            value.ShouldBeOfType<Product>();
        }

        [TestMethod]
        public async Task GetProductAsync_GivenProductIdThatDoesNotExist_ShouldReturnTypeNotFoundResult()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productIdThatDoesNotExist = 0;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatDoesNotExist))
                .ReturnsAsync(false);
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var result = await productsController.GetProductAsync(productIdThatDoesNotExist);

            // Assert
            result.ShouldBeOfType<ActionResult<Product>>();
            result.Result.ShouldBeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task BuyProductAsync_GivenProductIdThatExists_ShouldReturnTypeOkObjectResultWithInt()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productIdThatExists = 0;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);
            productServiceStub.Setup(ps => ps.BuyProductAsync(productIdThatExists));
            productServiceStub.Setup(ps => ps.GetProductAsync(productIdThatExists))
                .ReturnsAsync(new Product());
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.BuyProductAsync(productIdThatExists);
            var value = (actionResult.Result as OkObjectResult).Value;

            // Assert
            actionResult.ShouldBeOfType<ActionResult<int>>();
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
            value.ShouldBeOfType<int>();
        }

        [TestMethod]
        public async Task BuyProductAsync_GivenProductIdThatDoesNotExist_ShouldReturnTypeNotFoundResult()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productIdThatDoesNotExist = 0;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatDoesNotExist))
                .ReturnsAsync(false);
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.BuyProductAsync(productIdThatDoesNotExist);

            // Assert
            actionResult.ShouldBeOfType<ActionResult<int>>();
            actionResult.Result.ShouldBeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task AddProductAsync_GivenProduct_ShouldReturnTypeCreatedAtRouteResultWithProduct()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            productServiceStub.Setup(ps => ps.AddProductAsync(It.IsAny<Product>()))
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
        public async Task AddProductAsync_GivenNullProduct_ShouldReturnTypeBadRequestObjectResult()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productsController = new ProductsController(productServiceStub.Object);
            var nullProduct = (Product)null;

            // Act
            var actionResult = await productsController.AddProductAsync(nullProduct);
            var value = (actionResult.Result as BadRequestObjectResult).Value;

            // Assert
            actionResult.ShouldBeOfType<ActionResult<Product>>();
            actionResult.Result.ShouldBeOfType<BadRequestObjectResult>();
            value.ShouldBe("The product cannot be null.");
        }
        

        [TestMethod]
        public async Task DeleteProductAsync_GivenProductIdThatExists_ShouldReturnTypeOkResult()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productIdThatExists = 0;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);
            productServiceStub.Setup(ps => ps.DeleteProductAsync(productIdThatExists));
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.DeleteProductAsync(productIdThatExists);

            // Assert
            actionResult.ShouldBeOfType<OkResult>();
        }

        [TestMethod]
        public async Task DeleteProductAsync_GivenProductIdThatDoesNotExist_ShouldReturnTypeNotFoundResult()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productIdThatDoesNotExist = 0;
            productServiceStub.Setup(ps => ps.GetProductAsync(productIdThatDoesNotExist))
                .ReturnsAsync((Product)null);
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.DeleteProductAsync(productIdThatDoesNotExist);

            // Assert
            actionResult.ShouldBeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task UpdateProductAsync_GivenProductWithKnownId_ShouldReturnTypeOkObjectResultAndProduct()
        {
            // Arrange
            var productIdThatExists = 1;
            var productWithKnownId = new Product()
            {
                Id = productIdThatExists
            };
            var productServiceStub = new Mock<IProductService>();
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);
            productServiceStub.Setup(ps => ps.UpdateProductAsync(It.IsAny<Product>()));
            productServiceStub.Setup(ps => ps.GetProductAsync(productIdThatExists))
                .ReturnsAsync(new Product());
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.UpdateProductAsync(productWithKnownId);
            var value = (actionResult.Result as OkObjectResult).Value;

            // Assert
            actionResult.ShouldBeOfType<ActionResult<Product>>();
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
            value.ShouldBeOfType<Product>();
        }

        [TestMethod]
        public async Task UpdateProductAsync_GivenProductWithUnknownId_ShouldReturnTypeNotFoundResult()
        {
            // Arrange
            var productIdThatDoesNotExist = 0;
            var productWithUnknownId = new Product()
            {
                Id = productIdThatDoesNotExist
            };
            var productServiceStub = new Mock<IProductService>();
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatDoesNotExist))
                .ReturnsAsync(false);
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.UpdateProductAsync(productWithUnknownId);

            // Assert
            actionResult.ShouldBeOfType<ActionResult<Product>>();
            actionResult.Result.ShouldBeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task UpdateProductAsync_GivenNullProduct_ShouldReturnTypeBadRequestObjectResult()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productsController = new ProductsController(productServiceStub.Object);
            var nullProduct = (Product)null;

            // Act
            var actionResult = await productsController.UpdateProductAsync(nullProduct);
            var value = (actionResult.Result as BadRequestObjectResult).Value;

            // Assert
            actionResult.ShouldBeOfType<ActionResult<Product>>();
            actionResult.Result.ShouldBeOfType<BadRequestObjectResult>();
            value.ShouldBe("The product cannot be null.");
        }
    }
}
