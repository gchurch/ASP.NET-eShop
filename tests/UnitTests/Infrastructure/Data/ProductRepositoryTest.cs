using Ganges.ApplicationCore.Entities;
using Ganges.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Infrastructure.Data
{
    [TestClass]
    public class ProductRepositoryTest
    {

        private readonly GangesDbContext _context;
        private readonly ProductRepository _productRepository;

        public ProductRepositoryTest()
        {
            var dbOptions = new DbContextOptionsBuilder<GangesDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new GangesDbContext(dbOptions);
            _productRepository = new ProductRepository(_context);
        }

        [TestMethod]
        public async Task GetProductAsync_ShouldReturnNull()
        {
            // Arrange
            var id = 1;

            // Act
            var product = await _productRepository.GetProductAsync(id);

            // Assert
            Assert.AreEqual(product, null);
        }

        [TestMethod]
        public async Task GetProductsAsync_ShouldBeEmpty()
        {
            // Arrange

            // Act
            var products = await _productRepository.GetProductsAsync() as List<Product>;

            // Assert
            Assert.AreEqual(products.Count, 0);
        }
    }
}
