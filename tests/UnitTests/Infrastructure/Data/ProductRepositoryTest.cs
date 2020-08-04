using Ganges.ApplicationCore.Entities;
using Ganges.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ganges.UnitTests.Infrastructure.Data
{
    [TestClass]
    public class ProductRepositoryTest
    {

        private DbContextOptions<GangesDbContext> _dbOptions;
        private GangesDbContext _context;
        private ProductRepository _productRepository;

        public ProductRepositoryTest()
        {
            _dbOptions = new DbContextOptionsBuilder<GangesDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new GangesDbContext(_dbOptions);
            _productRepository = new ProductRepository(_context);
            // Calling the Seed method here ensures that the database is actually 
            // created and is seeded. The seed data currently comes from the 
            // actual GangesDbContext class.
            Seed();
        }

        private void Seed()
        {
            using (var context = new GangesDbContext(_dbOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.SaveChanges();
            }
        }

        [TestMethod]
        public async Task GetProductAsync_GettingProductWithId1_ShouldReturnToyProduct()
        {
            // Arrange
            var id = 1;

            // Act
            var product = await _productRepository.GetProductAsync(id);

            // Assert
            product.Id.ShouldBe(id);
            product.Title.ShouldBe("Toy");
            product.Description.ShouldBe("Plastic");
            product.Seller.ShouldBe("Michael");
            product.Price.ShouldBe(50);
            product.Quantity.ShouldBe(2);
            product.ImageUrl.ShouldBe("toy.png");
        }


        [TestMethod]
        public async Task GetProductsAsync_ShouldBeEmpty()
        {
            // Arrange
            Seed();

            // Act
            var products = await _productRepository.GetProductsAsync() as List<Product>;

            // Assert
            products.Count.ShouldBe(3);
        }
    }
}
