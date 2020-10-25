using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Web.MVC.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Web.MVC.Controllers
{
    [TestClass]
    public class ProductsControllerTest
    {
        [TestMethod]
        public async Task Index_ShouldReturnTypeViewResult()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            productServiceStub.Setup(ps => ps.GetAllProductsAsync())
                .ReturnsAsync(new List<Product>());
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.Index();

            // Assert
            actionResult.ShouldBeOfType<ViewResult>();
        }

        [TestMethod]
        public async Task Details_GivenProductIdThatExists_ShouldReturnTypeViewResult()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productIdThatExists = 1;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.Details(productIdThatExists);

            // Assert
            actionResult.ShouldBeOfType<ViewResult>();
        }

        [TestMethod]
        public async Task Details_GivenProductIdThatDoesNotExist_ShouldReturnTypeNotFound()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productIdThatDoesNotExist = 0;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatDoesNotExist))
                .ReturnsAsync(false);
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.Details(productIdThatDoesNotExist);

            // Assert
            actionResult.ShouldBeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void Create_ShouldReturnTypeViewResult()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = productsController.Create();

            // Assert
            actionResult.ShouldBeOfType<ViewResult>();
        }

        [TestMethod]
        public async Task CreatePost_GivenAProduct_ShouldReturnTypeRedirectToActionResult()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var product = new Product();
            productServiceStub.Setup(ps => ps.AddProductAsync(It.IsAny<Product>()));
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.CreatePost(product);

            // Assert
            actionResult.ShouldBeOfType<RedirectToActionResult>();
        }

        [TestMethod]
        public async Task Create_GivenAProduct_ShouldCallAddProductAsyncInProductService()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var product = new Product();
            productServiceStub.Setup(ps => ps.AddProductAsync(It.IsAny<Product>()))
                .Verifiable();
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.CreatePost(product);

            // Assert
            productServiceStub.Verify(ps => ps.AddProductAsync(product), Times.Once());
        }

        [TestMethod]
        public async Task Delete_GivenAProductIdThatExists_ShouldReturnTypeViewResult()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productIdThatExists = 1;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.Delete(productIdThatExists);

            // Assert
            actionResult.ShouldBeOfType<ViewResult>();
        }

        [TestMethod]
        public async Task Delete_GivenAProductIdThatDoesNotExist_ShouldReturnNotFoundResult()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productIdThatDoesNotExist = 0;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatDoesNotExist))
                .ReturnsAsync(false);
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.Delete(productIdThatDoesNotExist);

            // Assert
            actionResult.ShouldBeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task DeletePost_GivenAProductIdThatExists_ShouldReturnTypeRedirectToActionResult()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productIdThatExists = 1;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.DeletePost(productIdThatExists);

            // Assert
            actionResult.ShouldBeOfType<RedirectToActionResult>();
        }

        [TestMethod]
        public async Task DeletePost_GivenAProductIdThatDoesNotExist_ShouldReturnNotFoundResult()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productIdThatDoesNotExist = 0;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatDoesNotExist))
                .ReturnsAsync(false);
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.DeletePost(productIdThatDoesNotExist);

            // Assert
            actionResult.ShouldBeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task Edit_GivenAProductIdThatExists_ShouldReturnTypeViewResult()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productIdThatExists = 1;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.Edit(productIdThatExists);

            // Assert
            actionResult.ShouldBeOfType<ViewResult>();
        }

        [TestMethod]
        public async Task Edit_GivenAProductIdThatDoesNotExist_ShouldReturnNotFoundResult()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productIdThatDoesNotExist = 0;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatDoesNotExist))
                .ReturnsAsync(false);
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.Edit(productIdThatDoesNotExist);

            // Assert
            actionResult.ShouldBeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task EditPost_GivenAProductIdThatExists_ShouldReturnTypeRedirectToActionResult()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productIdThatExists = 1;
            var productWithIdThatExists = new Product() {
                Id = productIdThatExists
            };
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);
            productServiceStub.Setup(ps => ps.GetProductByIdAsync(productIdThatExists))
                .ReturnsAsync(productWithIdThatExists);
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.EditPost(productWithIdThatExists);

            // Assert
            actionResult.ShouldBeOfType<RedirectToActionResult>();
        }

        [TestMethod]
        public async Task EditPost_GivenAProductIdThatDoesNotExist_ShouldReturnTypeNotFoundResult()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productIdThatDoesNotExist = 0;
            var productWithIdThatDoesNotExist = new Product()
            {
                Id = productIdThatDoesNotExist
            };
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatDoesNotExist))
                .ReturnsAsync(false);
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            var actionResult = await productsController.EditPost(productWithIdThatDoesNotExist);

            // Assert
            actionResult.ShouldBeOfType<NotFoundResult>();
        }

    }
}
