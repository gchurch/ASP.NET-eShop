using ApplicationCore.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegrationTests.Repositories
{
    [TestClass]
    public class ProductRepositoryTest
    {

        private DbContextOptions<GangesDbContext> _dbOptions;

        public ProductRepositoryTest()
        {
            _dbOptions = new DbContextOptionsBuilder<GangesDbContext>()
                .UseInMemoryDatabase(databaseName: "UnitTestingDatabase")
                .Options;
        }

        // This method creates a fresh test database every time it is called.
        // This method should be called at the start of every unit test for the
        // ProductRepository class.
        private void ResetDatabase()
        {
            using (var context = new GangesDbContext(_dbOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.SaveChanges();
            }
        }

        [TestMethod]
        public async Task GetProductAsync_GivenProductIdThatExists_ShouldReturnProductWithThatId()
        {
            using (var context = new GangesDbContext(_dbOptions))
            {
                // Arrange
                ResetDatabase();
                var productRepository = new ProductRepository(context);
                var productIdThatExists = 1;

                // Act
                var product = await productRepository.GetProductAsync(productIdThatExists);

                // Assert
                product.Id.ShouldBe(productIdThatExists);
            }
        }

        [TestMethod]
        public async Task GetProductAsync_GivenProductIdThatDoesNotExist_ShouldReturnNull()
        {
            using (var context = new GangesDbContext(_dbOptions))
            {
                // Arrange
                ResetDatabase();
                var productRepository = new ProductRepository(context);
                var productIdThatDoesNotExist = 4;

                // Act
                var product = await productRepository.GetProductAsync(productIdThatDoesNotExist);

                // Assert
                product.ShouldBe(null);
            }
        }


        [TestMethod]
        public async Task GetProductsAsync_With3ProductsInTheDatabase_ShouldReturnProductCountOf3()
        {
            using (var context = new GangesDbContext(_dbOptions))
            {
                // Arrange
                ResetDatabase();
                var productRepository = new ProductRepository(context);

                // Act
                var products = await productRepository.GetAllProductsAsync() as List<Product>;

                // Assert
                products.Count.ShouldBe(3);
            }
        }
    }
}
