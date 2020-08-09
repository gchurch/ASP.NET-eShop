using Ganges.ApplicationCore.Entities;
using Ganges.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ganges.UnitTests.Infrastructure.Data
{
    [TestClass]
    public class ProductRepositoryTest
    {

        private DbContextOptions<GangesDbContext> _dbOptions;

        public ProductRepositoryTest()
        {
            _dbOptions = new DbContextOptionsBuilder<GangesDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        // This method creates a fresh test database every time it is called.
        // This method should be called at the start of every unit test for the
        // ProductRepository class.
        private void CreateDatabase()
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
                CreateDatabase();
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
                CreateDatabase();
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
            using(var context = new GangesDbContext(_dbOptions))
            {
                // Arrange
                CreateDatabase();
                var productRepository = new ProductRepository(context);

                // Act
                var products = await productRepository.GetProductsAsync() as List<Product>;

                // Assert
                products.Count.ShouldBe(3);
            }
        }

        [TestMethod]
        public async Task BuyProductAsync_GivenProductIdThatExists_ShouldReturnProductWithThatId()
        {
            using (var context = new GangesDbContext(_dbOptions))
            {
                // Arrange
                CreateDatabase();
                var productRepository = new ProductRepository(context);
                var productIdThatExists = 1;

                // Act
                var result = await productRepository.BuyProductAsync(productIdThatExists);

                // Assert
                result.Id.ShouldBe(productIdThatExists);
            }
        }

        [TestMethod]
        public async Task BuyProductAsync_GivenProductIdThatDoesNotExist_ShouldReturnNull()
        {
            using (var context = new GangesDbContext(_dbOptions))
            {
                // Arrange
                CreateDatabase();
                var productRepository = new ProductRepository(context);
                var productIdThatDoesNotExist = 0;

                // Act
                var result = await productRepository.BuyProductAsync(productIdThatDoesNotExist);

                // Assert
                result.ShouldBe(null);
            }
        }

        [TestMethod]
        public async Task AddProductAsync_GivenValidProduct_ShouldReturn1()
        {
            using (var context = new GangesDbContext(_dbOptions))
            {
                // Arrange
                CreateDatabase();
                var productRepository = new ProductRepository(context);
                var product = new Product();

                // Act
                var result = await productRepository.AddProductAsync(product);

                // Assert
                result.ShouldBe(1);
            }

        }

        [TestMethod]
        public async Task AddProductAsync_GivenNullProduct_ShouldReturn0()
        {
            using (var context = new GangesDbContext(_dbOptions))
            {
                // Arrange
                CreateDatabase();
                var productRepository = new ProductRepository(context);
                var product = (Product)null;

                // Act
                var result = await productRepository.AddProductAsync(product);

                // Assert
                result.ShouldBe(0);
            }
        }

        [TestMethod]
        public async Task DeleteProductAsync_GivenProductIdThatExists_ShouldReturnTrue()
        {
            using (var context = new GangesDbContext(_dbOptions))
            {
                // Arrange
                CreateDatabase();
                var productRepository = new ProductRepository(context);
                var productIdThatExists = 1;

                // Act
                var result = await productRepository.DeleteProductAsync(productIdThatExists);

                // Assert
                result.ShouldBe(true);
            }
        }

        [TestMethod]
        public async Task DeleteProductAsync_GivenProductIdThatDoesNotExist_ShouldReturnFalse()
        {
            using (var context = new GangesDbContext(_dbOptions))
            {
                // Arrange
                CreateDatabase();
                var productRepository = new ProductRepository(context);
                var productIdThatDoesNotExist = 0;

                // Act
                var result = await productRepository.DeleteProductAsync(productIdThatDoesNotExist);

                // Assert
                result.ShouldBe(false);
            }
        }

        [TestMethod]
        public async Task UpdateProductAsync_GivenProductIdThatExistsAndAProduct_ShouldReturnProductWithSameId()
        {
            using (var context = new GangesDbContext(_dbOptions))
            {
                // Arrange
                CreateDatabase();
                var productRepository = new ProductRepository(context);
                var productIdThatExists = 1;
                var product = new Product();

                // Act
                var result = await productRepository.UpdateProductAsync(productIdThatExists, product);

                // Assert
                result.Id.ShouldBe(productIdThatExists);
            }
        }

        [TestMethod]
        public async Task UpdateProductAsync_GivenProductIdThatDoesNotExistAndAProduct_ShouldReturnNull()
        {
            using (var context = new GangesDbContext(_dbOptions))
            {
                // Arrange
                CreateDatabase();
                var productRepository = new ProductRepository(context);
                var productIdThatDoesNotExist = 0;
                var product = new Product();

                // Act
                var result = await productRepository.UpdateProductAsync(productIdThatDoesNotExist, product);

                // Assert
                result.ShouldBe(null);
            }
        }
    }
}
