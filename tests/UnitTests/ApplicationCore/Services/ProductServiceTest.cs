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

namespace Ganges.UnitTests.Web.Services
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
        public async Task DeleteProductAsync_GivenAProductId_ShouldCallDeleteProductAsyncInProductRepository()
        {
            // Arrange
            var id = 0;
            var productRepositoryStub = new Mock<IProductRepository>();
            productRepositoryStub.Setup(pr => pr.DeleteProductAsync(It.IsAny<Product>()))
                .Verifiable();
            var productService = new ProductService(productRepositoryStub.Object);

            // Act
            var result = await productService.DeleteProductAsync(id);

            // Assert
            productRepositoryStub.Verify();
        }

        [TestMethod]
        public async Task UpdateProductAsync_GivenAProduct_ShouldCallUpdateProductAsyncInProductRepository()
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

    }
}
