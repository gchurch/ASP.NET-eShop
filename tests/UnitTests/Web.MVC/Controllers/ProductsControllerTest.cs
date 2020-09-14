using Ganges.ApplicationCore.Entities;
using Ganges.ApplicationCore.Interfaces;
using Ganges.Web.MVC.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ganges.UnitTests.Web.MVC.Controllers
{
    [TestClass]
    public class ProductsControllerTest
    {
        [TestMethod]
        public async Task Index_ShouldReturnTypeViewResult()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            productServiceStub.Setup(ps => ps.GetProductsAsync())
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
            productServiceStub.Setup(ps => ps.GetProductAsync(productIdThatExists))
                .ReturnsAsync(new Product());
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
            productServiceStub.Setup(ps => ps.GetProductAsync(productIdThatDoesNotExist))
                .ReturnsAsync((Product)null);
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
    }
}
