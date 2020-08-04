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
        public async Task GetProductAsync_GivenProductId1_ShouldReturnCorrectProduct()
        {
            using (var context = new GangesDbContext(_dbOptions))
            {
                // Arrange
                CreateDatabase();
                var productRepository = new ProductRepository(context);
                var id = 1;

                // Act
                var product = await productRepository.GetProductAsync(id);

                // Assert
                product.Id.ShouldBe(id);
                product.Title.ShouldBe("Toy");
                product.Description.ShouldBe("Plastic");
                product.Seller.ShouldBe("Michael");
                product.Price.ShouldBe(50);
                product.Quantity.ShouldBe(2);
                product.ImageUrl.ShouldBe("toy.png");
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
                var id = 4;

                // Act
                var product = await productRepository.GetProductAsync(id);

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
        public async Task AddProductAsync_GivenValidProduct_ShouldAddProductToDatabase()
        {
            using (var context = new GangesDbContext(_dbOptions))
            {
                // Arrange
                CreateDatabase();
                var productRepository = new ProductRepository(context);
                var product = new Product()
                {
                    Id = 4,
                    Title = "Pen",
                    Description = "Blue ink",
                    Seller = "Daniel",
                    Price = 2,
                    Quantity = 10,
                    ImageUrl = "pen.png"
                };

                // Act
                var result = await productRepository.AddProductAsync(product);

                // Assert
                result.ShouldBe(1);
            }

        }

        [TestMethod]
        public async Task AddProductAsync_GivenNullProduct_ShouldAddProductToDatabase()
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
    }
}
