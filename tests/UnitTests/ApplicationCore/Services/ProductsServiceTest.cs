using Ganges.ApplicationCore.Entities;
using Ganges.ApplicationCore.Interfaces;
using Ganges.ApplicationCore.Services;
using Ganges.Infrastructure.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ganges.UnitTests.Web.Services
{
    [TestClass]
    public class ProductsServiceTest
    {

        [TestMethod]
        public async Task GetProductsAsync_ShouldCallGetProductsAsyncInProductRepository()
        {
            // Arrange
            var productServiceStub = new Mock<IProductRepository>();
            productServiceStub.Setup(pr => pr.GetProductsAsync())
                .ReturnsAsync(new List<Product>())
                .Verifiable();
            var productService = new ProductService(productServiceStub.Object);

            // Act
            var result = await productService.GetProductsAsync();

            // Assert
            productServiceStub.Verify();
        }

        [TestMethod]
        public async Task GetProductAsync_GivenAProductId_ShouldCallGetProductAsyncInProductRepository()
        {
            // Arrange
            int id = 0;
            var productServiceStub = new Mock<IProductRepository>();
            productServiceStub.Setup(pr => pr.GetProductAsync(It.IsAny<int>()))
                .ReturnsAsync(new Product())
                .Verifiable();
            var productService = new ProductService(productServiceStub.Object);

            // Act
            var result = await productService.GetProductAsync(id);

            // Assert
            productServiceStub.Verify();
        }


        [TestMethod]
        public async Task BuyProductAsync_GivenAProductId_ShouldCallBuyProductAsyncInProductRepository()
        {
            // Arrange
            int id = 0;
            var productServiceStub = new Mock<IProductRepository>();
            productServiceStub.Setup(pr => pr.BuyProductAsync(It.IsAny<int>()))
                .ReturnsAsync(new Product())
                .Verifiable();
            var productService = new ProductService(productServiceStub.Object);

            // Act
            var result = await productService.BuyProductAsync(id);

            // Assert
            productServiceStub.Verify();
        }

        [TestMethod]
        public async Task AddProductAsync_GivenAProduct_ShouldCallAddProductAsyncInProductRepository()
        {
            // Arrange
            var product = new Product();
            var productServiceStub = new Mock<IProductRepository>();
            productServiceStub.Setup(pr => pr.AddProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(1)
                .Verifiable();
            var productService = new ProductService(productServiceStub.Object);

            // Act
            var result = await productService.AddProductAsync(product);

            // Assert
            productServiceStub.Verify();
        }

        [TestMethod]
        public async Task DeleteProductAsync_GivenAProductId_ShouldCallDeleteProductAsyncInProductRepository()
        {
            // Arrange
            var id = 0;
            var productServiceStub = new Mock<IProductRepository>();
            productServiceStub.Setup(pr => pr.DeleteProductAsync(It.IsAny<int>()))
                .ReturnsAsync(true)
                .Verifiable();
            var productService = new ProductService(productServiceStub.Object);

            // Act
            var result = await productService.DeleteProductAsync(id);

            // Assert
            productServiceStub.Verify();
        }

        [TestMethod]
        public async Task UpdateProductAsync_GivenAProduct_ShouldCallUpdateProductAsyncInProductRepository()
        {
            // Arrange
            var id = 0;
            var product = new Product();
            var productServiceStub = new Mock<IProductRepository>();
            productServiceStub.Setup(pr => pr.UpdateProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(new Product())
                .Verifiable();
            var productService = new ProductService(productServiceStub.Object);

            // Act
            var result = await productService.UpdateProductAsync(product);

            // Assert
            productServiceStub.Verify();
        }

    }
}
