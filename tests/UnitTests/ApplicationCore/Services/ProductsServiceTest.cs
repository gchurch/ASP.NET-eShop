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
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(pr => pr.GetProductsAsync())
                .ReturnsAsync(new List<Product>())
                .Verifiable();
            var productService = new ProductService(productRepositoryMock.Object);

            // Act
            var result = await productService.GetProductsAsync();

            // Assert
            productRepositoryMock.Verify();
        }

        [TestMethod]
        public async Task GetProductAsync_GivenAProductId_ShouldCallGetProductAsyncInProductRepository()
        {
            // Arrange
            int id = 0;
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(pr => pr.GetProductAsync(It.IsAny<int>()))
                .ReturnsAsync(new Product())
                .Verifiable();
            var productService = new ProductService(productRepositoryMock.Object);

            // Act
            var result = await productService.GetProductAsync(id);

            // Assert
            productRepositoryMock.Verify();
        }

    }
}
