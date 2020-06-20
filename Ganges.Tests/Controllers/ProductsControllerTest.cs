using Ganges.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System.Threading.Tasks;

namespace Ganges.UnitTests
{
    [TestClass]
    public class ProductsControllerTest
    {
        [TestMethod]
        public void GetProductAsync_ReceivesExistingProductId_ShouldReturnOk()
        {
            // Arrange
            var id = 1;
            var product = new Product()
            {
                Id = id
            };
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(x => x.GetProductAsync(1)).Returns(Task.FromResult<Product>(product));
            var productController = new ProductsController(productServiceMock.Object);

            // Act
            var result = productController.GetProductAsync(id);

            // Assert
            result.ShouldBeOfType<OkObjectResult>();
        }

        [TestMethod]
        public void GetProductAsync_ReceivesNonExistentProductId_ShouldReturnNotFound()
        {
            // Arrange
            var id = 0;
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(x => x.GetProductAsync(id)).Returns<Product>(null);
            var productController = new ProductsController(productServiceMock.Object);

            // Act
            var result = productController.GetProductAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

    }
}
