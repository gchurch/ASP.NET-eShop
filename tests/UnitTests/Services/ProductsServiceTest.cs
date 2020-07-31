using Ganges.Infrastructure.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Ganges.Tests.Services
{
    [TestClass]
    class ProductsServiceTest
    {

        [TestMethod]
        public async void GetProductsAsync_ShouldReturnTypeIEnumerableOfProduct()
        {
            // Arrange
            var context = new Mock<GangesDbContext>();

            // Act

            // Assert
        }

    }
}
