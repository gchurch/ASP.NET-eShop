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
    public class ProductServiceTests
    {

        [TestMethod]
        public async Task GetAllProductsAsync_ShouldCallGetProductsAsyncInProductRepository()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(pr => pr.GetAllProductsAsync())
                .ReturnsAsync(new List<Product>())
                .Verifiable();
            var productService = new ProductService(productRepositoryMock.Object);

            // Act
            var result = await productService.GetAllProductsAsync();

            // Assert
            productRepositoryMock.Verify();
        }

        [TestMethod]
        public async Task GetProductByIdAsync_ShouldCallGetProductByIdAsyncInProductRepository()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(pr => pr.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Product())
                .Verifiable();
            var productService = new ProductService(productRepositoryMock.Object);
            int productId = 0;

            // Act
            var result = await productService.GetProductByIdAsync(productId);

            // Assert
            productRepositoryMock.Verify();
        }

        [TestMethod]
        public async Task AddProductAsync_ShouldCallAddProductAsyncInProductRepository()
        {
            // Arrange
            var product = new Product();
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(pr => pr.AddProductAsync(It.IsAny<Product>()))
                .Verifiable();
            var productService = new ProductService(productRepositoryMock.Object);

            // Act
            await productService.AddProductAsync(product);

            // Assert
            productRepositoryMock.Verify();
        }

        [TestMethod]
        public async Task AddProductAsync_ShouldChangeTheGivenProductsIdToZero()
        {
            // Arrange
            var product = new Product()
            {
                ProductId = 25
            };
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(pr => pr.AddProductAsync(It.IsAny<Product>()));
            var productService = new ProductService(productRepositoryMock.Object);

            // Act
            await productService.AddProductAsync(product);

            // Assert
            product.ProductId.ShouldBe(0);
        }

        [TestMethod]
        public async Task DeleteProductByIdAsync_ShouldCallDeleteProductAsyncInProductRepository()
        {
            // Arrange
            var productId = 1;
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(pr => pr.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Product());
            productRepositoryMock.Setup(pr => pr.DeleteProductByIdAsync(It.IsAny<int>()))
                .Verifiable();
            var productService = new ProductService(productRepositoryMock.Object);

            // Act
            await productService.DeleteProductByIdAsync(productId);

            // Assert
            productRepositoryMock.Verify(pr => pr.DeleteProductByIdAsync(productId), Times.Once());
        }

        [TestMethod]
        public async Task UpdateProductAsync_ShouldCallUpdateProductAsyncInProductRepository()
        {
            // Arrange
            int productId = 1;
            var product = new Product()
            {
                ProductId = productId
            };
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(pr => pr.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(product);
            productRepositoryMock.Setup(pr => pr.UpdateProductAsync(It.IsAny<Product>()))
                .Verifiable();
            var productService = new ProductService(productRepositoryMock.Object);

            // Act
            await productService.UpdateProductAsync(product);

            // Assert
            productRepositoryMock.Verify(pr => pr.UpdateProductAsync(product), Times.Once());
        }

        [TestMethod]
        public async Task DoesProductIdExist_GivenAProductIdThatExists_ShouldReturnTrue()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            int productIdThatExists = 1;
            var product = new Product()
            {
                ProductId = productIdThatExists
            };
            productRepositoryMock.Setup(pr => pr.GetProductByIdAsync(productIdThatExists))
                .ReturnsAsync(product);
            var productService = new ProductService(productRepositoryMock.Object);

            // Act
            bool result = await productService.DoesProductIdExist(productIdThatExists);

            // Assert
            result.ShouldBeTrue();
        }

        [TestMethod]
        public async Task DoesProductIdExist_GivenAProductIdThatDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            int productIdThatDoesNotExist = 1;
            var product = new Product()
            {
                ProductId = 0
            };
            productRepositoryMock.Setup(pr => pr.GetProductByIdAsync(productIdThatDoesNotExist))
                .ReturnsAsync(product);
            var productService = new ProductService(productRepositoryMock.Object);

            // Act
            bool result = await productService.DoesProductIdExist(productIdThatDoesNotExist);

            // Assert
            result.ShouldBeFalse();
        }
    }
}
