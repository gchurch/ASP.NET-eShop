using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests.ApplicationCore.Services
{
    [TestClass]
    public class ProductServiceTest
    {

        [TestMethod]
        public async Task GetAllProductsAsync_ShouldCallGetProductsAsyncInProductRepository()
        {
            // Arrange
            var productRepositoryStub = new Mock<IProductRepository>();
            productRepositoryStub.Setup(pr => pr.GetAllProductsAsync())
                .ReturnsAsync(new List<Product>())
                .Verifiable();
            var productService = new ProductService(productRepositoryStub.Object);

            // Act
            var result = await productService.GetAllProductsAsync();

            // Assert
            productRepositoryStub.Verify();
        }

        [TestMethod]
        public async Task GetProductByIdAsync_ShouldCallGetProductByIdAsyncInProductRepository()
        {
            // Arrange
            var productRepositoryStub = new Mock<IProductRepository>();
            productRepositoryStub.Setup(pr => pr.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Product())
                .Verifiable();
            var productService = new ProductService(productRepositoryStub.Object);
            int productId = 0;

            // Act
            var result = await productService.GetProductByIdAsync(productId);

            // Assert
            productRepositoryStub.Verify();
        }

        [TestMethod]
        public async Task BuyProductByIdAsync_ShouldCallUpdateProductAsyncInProductRepository()
        {
            // Arrange
            var productRepositoryStub = new Mock<IProductRepository>();
            productRepositoryStub.Setup(pr => pr.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Product());
            productRepositoryStub.Setup(pr => pr.UpdateProductAsync(It.IsAny<Product>()))
                .Verifiable();
            var productService = new ProductService(productRepositoryStub.Object);
            int productId = 0;

            // Act
            await productService.BuyProductByIdAsync(productId);

            // Assert
            productRepositoryStub.Verify(pr => pr.UpdateProductAsync(It.IsAny<Product>()), Times.Once());
        }

        [TestMethod]
        public async Task AddProductAsync_ShouldCallAddProductAsyncInProductRepository()
        {
            // Arrange
            var product = new Product();
            var productRepositoryStub = new Mock<IProductRepository>();
            productRepositoryStub.Setup(pr => pr.AddProductAsync(It.IsAny<Product>()))
                .Verifiable();
            var productService = new ProductService(productRepositoryStub.Object);

            // Act
            await productService.AddProductAsync(product);

            // Assert
            productRepositoryStub.Verify();
        }

        [TestMethod]
        public async Task AddProductAsync_ShouldChangeTheGivenProductsIdToZero()
        {
            // Arrange
            var product = new Product()
            {
                Id = 25
            };
            var productRepositoryStub = new Mock<IProductRepository>();
            productRepositoryStub.Setup(pr => pr.AddProductAsync(It.IsAny<Product>()));
            var productService = new ProductService(productRepositoryStub.Object);

            // Act
            await productService.AddProductAsync(product);

            // Assert
            product.Id.ShouldBe(0);
        }

        [TestMethod]
        public async Task DeleteProductByIdAsync_ShouldCallDeleteProductAsyncInProductRepository()
        {
            // Arrange
            var productId = 1;
            var productRepositoryStub = new Mock<IProductRepository>();
            productRepositoryStub.Setup(pr => pr.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Product());
            productRepositoryStub.Setup(pr => pr.DeleteProductByIdAsync(It.IsAny<int>()))
                .Verifiable();
            var productService = new ProductService(productRepositoryStub.Object);

            // Act
            await productService.DeleteProductByIdAsync(productId);

            // Assert
            productRepositoryStub.Verify(pr => pr.DeleteProductByIdAsync(productId), Times.Once());
        }

        [TestMethod]
        public async Task UpdateProductAsync_ShouldCallUpdateProductAsyncInProductRepository()
        {
            // Arrange
            int productId = 1;
            var product = new Product()
            {
                Id = productId
            };
            var productRepositoryStub = new Mock<IProductRepository>();
            productRepositoryStub.Setup(pr => pr.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(product);
            productRepositoryStub.Setup(pr => pr.UpdateProductAsync(It.IsAny<Product>()))
                .Verifiable();
            var productService = new ProductService(productRepositoryStub.Object);

            // Act
            await productService.UpdateProductAsync(product);

            // Assert
            productRepositoryStub.Verify(pr => pr.UpdateProductAsync(product), Times.Once());
        }

        [TestMethod]
        public async Task DoesProductIdExist_GivenAProductIdThatExists_ShouldReturnTrue()
        {
            // Arrange
            var productRepositoryStub = new Mock<IProductRepository>();
            int productIdThatExists = 1;
            var product = new Product()
            {
                Id = productIdThatExists
            };
            productRepositoryStub.Setup(pr => pr.GetProductByIdAsync(productIdThatExists))
                .ReturnsAsync(product);
            var productService = new ProductService(productRepositoryStub.Object);

            // Act
            bool result = await productService.DoesProductIdExist(productIdThatExists);

            // Assert
            result.ShouldBeTrue();
        }

        [TestMethod]
        public async Task DoesProductIdExist_GivenAProductIdThatDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var productRepositoryStub = new Mock<IProductRepository>();
            int productIdThatDoesNotExist = 1;
            var product = new Product()
            {
                Id = 0
            };
            productRepositoryStub.Setup(pr => pr.GetProductByIdAsync(productIdThatDoesNotExist))
                .ReturnsAsync(product);
            var productService = new ProductService(productRepositoryStub.Object);

            // Act
            bool result = await productService.DoesProductIdExist(productIdThatDoesNotExist);

            // Assert
            result.ShouldBeFalse();
        }
    }
}
