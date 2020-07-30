using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ganges.Infrastructure.Data;

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
