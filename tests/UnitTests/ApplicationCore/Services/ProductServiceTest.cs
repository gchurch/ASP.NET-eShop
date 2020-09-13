using Ganges.ApplicationCore.Entities;
using Ganges.ApplicationCore.Interfaces;
using Ganges.ApplicationCore.Services;
using Ganges.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ganges.UnitTests.ApplicationCore.Services
{
    [TestClass]
    public class ProductServiceTest
    {

        [TestMethod]
        public async Task GetProductsAsync_ShouldCallGetProductsAsyncInProductRepository()
        {
            // Arrange
            var productRepositoryStub = new Mock<IProductRepository>();
            productRepositoryStub.Setup(pr => pr.GetProductsAsync())
                .ReturnsAsync(new List<Product>())
                .Verifiable();
            var productService = new ProductService(productRepositoryStub.Object);

            // Act
            var result = await productService.GetProductsAsync();

            // Assert
            productRepositoryStub.Verify();
        }

        [TestMethod]
        public async Task GetProductAsync_GivenAProductId_ShouldCallGetProductAsyncInProductRepository()
        {
            // Arrange
            var productRepositoryStub = new Mock<IProductRepository>();
            productRepositoryStub.Setup(pr => pr.GetProductAsync(It.IsAny<int>()))
                .ReturnsAsync(new Product())
                .Verifiable();
            var productService = new ProductService(productRepositoryStub.Object);
            int id = 0;

            // Act
            var result = await productService.GetProductAsync(id);

            // Assert
            productRepositoryStub.Verify();
        }

        [TestMethod]
        public async Task BuyProductAsync_GivenAProductIdThatExists_ShouldReturnProductWithQuantityDecreasedByOne()
        {
            // Arrange
            var productIdThatExists = 1;
            var retrievedProduct = new Product() { 
                Id = productIdThatExists, 
                Quantity = 2 
            };
            var productRepositoryStub = new Mock<IProductRepository>();
            productRepositoryStub.Setup(pr => pr.GetProductAsync(productIdThatExists))
                .ReturnsAsync(retrievedProduct);
            productRepositoryStub.Setup(pr => pr.UpdateProductAsync(retrievedProduct));
            var productService = new ProductService(productRepositoryStub.Object);

            // Act
            var result = await productService.BuyProductAsync(productIdThatExists);

            // Assert
            result.Quantity.ShouldBe(1);
        }

        [TestMethod]
        public async Task BuyProductAsync_GivenAProductIdThatExists_ShouldCallUpdateProductAsyncInProductRepository()
        {
            // Arrange
            var productIdThatExists = 1;
            var productRepositoryStub = new Mock<IProductRepository>();
            productRepositoryStub.Setup(pr => pr.GetProductAsync(productIdThatExists))
                .ReturnsAsync(new Product());
            productRepositoryStub.Setup(pr => pr.UpdateProductAsync(It.IsAny<Product>()))
                .Verifiable();
            var productService = new ProductService(productRepositoryStub.Object);

            // Act
            var result = await productService.BuyProductAsync(productIdThatExists);

            // Assert
            productRepositoryStub.Verify(pr => pr.UpdateProductAsync(It.IsAny<Product>()), Times.Once());
        }

        [TestMethod]
        public async Task BuyProductAsync_GivenAProductIdThatDoesNotExist_ShouldNotCallUpdateProductAsyncInProductRepository()
        {
            // Arrange
            var productIdThatDoesNotExist = 1;
            var productRepositoryStub = new Mock<IProductRepository>();
            productRepositoryStub.Setup(pr => pr.GetProductAsync(productIdThatDoesNotExist))
                .ReturnsAsync((Product)null);
            productRepositoryStub.Setup(pr => pr.UpdateProductAsync(It.IsAny<Product>()))
                .Verifiable();
            var productService = new ProductService(productRepositoryStub.Object);

            // Act
            var result = await productService.BuyProductAsync(productIdThatDoesNotExist);

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
        public async Task DeleteProductAsync_GivenAProductIdThatExists_ShouldReturnTrue()
        {
            // Arrange
            var productIdThatExists = 1;
            var productRepositoryStub = new Mock<IProductRepository>();
            productRepositoryStub.Setup(pr => pr.GetProductAsync(productIdThatExists))
                .ReturnsAsync(new Product());
            productRepositoryStub.Setup(pr => pr.DeleteProductAsync(It.IsAny<Product>()));
            var productService = new ProductService(productRepositoryStub.Object);

            // Act
            var result = await productService.DeleteProductAsync(productIdThatExists);

            // Assert
            result.ShouldBe(true);
        }

        [TestMethod]
        public async Task DeleteProductAsync_GivenAProductIdThatDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var productIdThatDoesNotExist = 0;
            var productRepositoryStub = new Mock<IProductRepository>();
            productRepositoryStub.Setup(pr => pr.GetProductAsync(productIdThatDoesNotExist))
                .ReturnsAsync((Product)null);
            productRepositoryStub.Setup(pr => pr.DeleteProductAsync(It.IsAny<Product>()));
            var productService = new ProductService(productRepositoryStub.Object);

            // Act
            var result = await productService.DeleteProductAsync(productIdThatDoesNotExist);

            // Assert
            result.ShouldBe(false);
        }

        [TestMethod]
        public async Task DeleteProductAsync_GivenAProductIdThatExists_ShouldCallDeleteProductAsyncInProductRepository()
        {
            // Arrange
            var productIdThatExists = 1;
            var productRepositoryStub = new Mock<IProductRepository>();
            productRepositoryStub.Setup(pr => pr.GetProductAsync(productIdThatExists))
                .ReturnsAsync(new Product());
            productRepositoryStub.Setup(pr => pr.DeleteProductAsync(It.IsAny<Product>()))
                .Verifiable();
            var productService = new ProductService(productRepositoryStub.Object);

            // Act
            await productService.DeleteProductAsync(productIdThatExists);

            // Assert
            productRepositoryStub.Verify(pr => pr.DeleteProductAsync(It.IsAny<Product>()), Times.Once());
        }

        [TestMethod]
        public async Task DeleteProductAsync_GivenAProductIdThatExists_ShouldNotCallDeleteProductAsyncInProductRepository()
        {
            // Arrange
            var productIdThatDoesNotExist = 1;
            var productRepositoryStub = new Mock<IProductRepository>();
            productRepositoryStub.Setup(pr => pr.GetProductAsync(productIdThatDoesNotExist))
                .ReturnsAsync((Product)null);
            productRepositoryStub.Setup(pr => pr.DeleteProductAsync(It.IsAny<Product>()))
                .Verifiable();
            var productService = new ProductService(productRepositoryStub.Object);

            // Act
            await productService.DeleteProductAsync(productIdThatDoesNotExist);

            // Assert
            productRepositoryStub.Verify(pr => pr.DeleteProductAsync(It.IsAny<Product>()), Times.Never());
        }

        [TestMethod]
        public async Task UpdateProductAsync_GivenANonNullProduct_ShouldCallUpdateProductAsyncInProductRepository()
        {
            // Arrange
            var product = new Product();
            var productRepositoryStub = new Mock<IProductRepository>();
            productRepositoryStub.Setup(pr => pr.UpdateProductAsync(It.IsAny<Product>()))
                .Verifiable();
            var productService = new ProductService(productRepositoryStub.Object);

            // Act
            var result = await productService.UpdateProductAsync(product);

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
            var result = await productService.UpdateProductAsync(nullProduct);

            // Assert
            productRepositoryStub.Verify(pr => pr.UpdateProductAsync(It.IsAny<Product>()), Times.Never());
        }
    }
}
