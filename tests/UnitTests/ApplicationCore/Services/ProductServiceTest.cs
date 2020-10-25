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
        public async Task GetProductByIdAsync_GivenAProductId_ShouldCallGetProductAsyncInProductRepository()
        {
            // Arrange
            var productRepositoryStub = new Mock<IProductRepository>();
            productRepositoryStub.Setup(pr => pr.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Product())
                .Verifiable();
            var productService = new ProductService(productRepositoryStub.Object);
            int id = 0;

            // Act
            var result = await productService.GetProductByIdAsync(id);

            // Assert
            productRepositoryStub.Verify();
        }

        [TestMethod]
        public async Task BuyProductByIdAsync_GivenAProductIdThatExists_ShouldCallUpdateProductAsyncInProductRepository()
        {
            // Arrange
            var productIdThatExists = 1;
            var productRepositoryStub = new Mock<IProductRepository>();
            productRepositoryStub.Setup(pr => pr.GetProductByIdAsync(productIdThatExists))
                .ReturnsAsync(new Product());
            productRepositoryStub.Setup(pr => pr.UpdateProductAsync(It.IsAny<Product>()))
                .Verifiable();
            var productService = new ProductService(productRepositoryStub.Object);

            // Act
            await productService.BuyProductByIdAsync(productIdThatExists);

            // Assert
            productRepositoryStub.Verify(pr => pr.UpdateProductAsync(It.IsAny<Product>()), Times.Once());
        }

        [TestMethod]
        public async Task BuyProductByIdAsync_GivenAProductIdThatDoesNotExist_ShouldNotCallUpdateProductAsyncInProductRepository()
        {
            // Arrange
            var productIdThatDoesNotExist = 1;
            var productRepositoryStub = new Mock<IProductRepository>();
            productRepositoryStub.Setup(pr => pr.GetProductByIdAsync(productIdThatDoesNotExist))
                .ReturnsAsync((Product)null);
            productRepositoryStub.Setup(pr => pr.UpdateProductAsync(It.IsAny<Product>()))
                .Verifiable();
            var productService = new ProductService(productRepositoryStub.Object);

            // Act
            await productService.BuyProductByIdAsync(productIdThatDoesNotExist);

            // Assert
            productRepositoryStub.Verify(pr => pr.UpdateProductAsync(It.IsAny<Product>()), Times.Never());
        }

        [TestMethod]
        public async Task AddProductAsync_GivenAProduct_ShouldCallAddProductAsyncInProductRepository()
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
        public async Task AddProductAsync_GivenAProduct_ShouldGiveThatProductAnIdOfZero()
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
        public async Task DeleteProductByIdAsync_GivenAProductIdThatExists_ShouldCallDeleteProductAsyncInProductRepository()
        {
            // Arrange
            var productIdThatExists = 1;
            var productRepositoryStub = new Mock<IProductRepository>();
            productRepositoryStub.Setup(pr => pr.GetProductByIdAsync(productIdThatExists))
                .ReturnsAsync(new Product());
            productRepositoryStub.Setup(pr => pr.DeleteProductByIdAsync(It.IsAny<Product>()))
                .Verifiable();
            var productService = new ProductService(productRepositoryStub.Object);

            // Act
            await productService.DeleteProductByIdAsync(productIdThatExists);

            // Assert
            productRepositoryStub.Verify(pr => pr.DeleteProductByIdAsync(It.IsAny<Product>()), Times.Once());
        }

        [TestMethod]
        public async Task DeleteProductByIdAsync_GivenAProductIdThatExists_ShouldNotCallDeleteProductAsyncInProductRepository()
        {
            // Arrange
            var productIdThatDoesNotExist = 1;
            var productRepositoryStub = new Mock<IProductRepository>();
            productRepositoryStub.Setup(pr => pr.GetProductByIdAsync(productIdThatDoesNotExist))
                .ReturnsAsync((Product)null);
            productRepositoryStub.Setup(pr => pr.DeleteProductByIdAsync(It.IsAny<Product>()))
                .Verifiable();
            var productService = new ProductService(productRepositoryStub.Object);

            // Act
            await productService.DeleteProductByIdAsync(productIdThatDoesNotExist);

            // Assert
            productRepositoryStub.Verify(pr => pr.DeleteProductByIdAsync(It.IsAny<Product>()), Times.Never());
        }

        [TestMethod]
        public async Task UpdateProductAsync_GivenAnExistingProduct_ShouldCallUpdateProductAsyncInProductRepository()
        {
            // Arrange
            int existingProductId = 1;
            var existingProduct = new Product()
            {
                Id = existingProductId
            };
            var productRepositoryStub = new Mock<IProductRepository>();
            productRepositoryStub.Setup(pr => pr.GetProductByIdAsync(existingProductId))
                .ReturnsAsync(existingProduct);
            productRepositoryStub.Setup(pr => pr.UpdateProductAsync(It.IsAny<Product>()))
                .Verifiable();
            var productService = new ProductService(productRepositoryStub.Object);

            // Act
            await productService.UpdateProductAsync(existingProduct);

            // Assert
            productRepositoryStub.Verify();
        }

        [TestMethod]
        public async Task UpdateProductAsync_GivenANullProduct_ShouldNotCallUpdateProductAsyncInProductRepository()
        {
            // Arrange
            var nullProduct = (Product)null;
            var productRepositoryStub = new Mock<IProductRepository>();
            productRepositoryStub.Setup(pr => pr.UpdateProductAsync(It.IsAny<Product>()))
                .Verifiable();
            var productService = new ProductService(productRepositoryStub.Object);

            // Act
            await productService.UpdateProductAsync(nullProduct);

            // Assert
            productRepositoryStub.Verify(pr => pr.UpdateProductAsync(It.IsAny<Product>()), Times.Never());
        }
    }
}
