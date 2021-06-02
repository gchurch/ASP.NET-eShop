using ApplicationCore.Models;
using IdentityServer4.EntityFramework.Options;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Infrastructure.Data
{
    [TestClass]
    public class BasketRepositoryTests
    {

        public InMemoryProductDbContext CreateTestDatabase()
        {
            InMemoryProductDbContext ctx = new InMemoryProductDbContext(
                new DbContextOptions<ProductDbContext>(),
                new dontknow());
            return ctx;
        }

        [TestMethod]
        public void CreateBasket_ShouldCreateBasket()
        {
            // Arrange
            InMemoryProductDbContext ctx = CreateTestDatabase();
            BasketRepository basketRepository = new BasketRepository(ctx);

            // Act
            string ownerId = "george";
            basketRepository.CreateBasket(ownerId);
            var query = from basket in ctx.Baskets where basket.OwnerID == ownerId select basket;
            Basket retreivedBasket = query.Single();

            // Assert
            Assert.AreEqual(ownerId, retreivedBasket.OwnerID);
        }

        [TestMethod]
        public void DoesBasketExist_GivenThatBasketExists_ShouldReturnTrue()
        {
            // Arrange
            InMemoryProductDbContext ctx = CreateTestDatabase();
            BasketRepository basketRepository = new BasketRepository(ctx);

            // Act
            string ownerId = "george";
            basketRepository.CreateBasket(ownerId);
            bool doesBasketExist = basketRepository.DoesBasketExist(ownerId);

            // Assert
            Assert.AreEqual(true, doesBasketExist);
        }

        [TestMethod]
        public void DoesBasketExist_GivenThatBasketDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            InMemoryProductDbContext ctx = CreateTestDatabase();
            BasketRepository basketRepository = new BasketRepository(ctx);

            // Act
            string ownerId = "george";
            bool doesBasketExist = basketRepository.DoesBasketExist(ownerId);

            // Assert
            Assert.AreEqual(false, doesBasketExist);
        }

        [TestMethod]
        public void GetBasketByOwnerId_GivenThatTheBasketExists_ShouldReturnBasket()
        {
            // Arrange
            InMemoryProductDbContext ctx = CreateTestDatabase();
            BasketRepository basketRepository = new BasketRepository(ctx);

            // Act
            string ownerId = "george";
            basketRepository.CreateBasket(ownerId);
            Basket basket = basketRepository.GetBasketByOwnerId(ownerId);

            // Assert
            Assert.AreEqual(ownerId, basket.OwnerID);
        }

        [TestMethod]
        public void GetBasketByOwnerId_GivenThatTheBasketDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            InMemoryProductDbContext ctx = CreateTestDatabase();
            BasketRepository basketRepository = new BasketRepository(ctx);

            // Act
            string ownerId = "george";
            Basket basket = basketRepository.GetBasketByOwnerId(ownerId);

            // Assert
            Assert.AreEqual(null, basket);
        }

        [TestMethod]
        public void GetBasketByOwnerId_GivenThatBasketHasBasketItems_ShouldReturnBasketWithBasketItems()
        {
            // Arrange
            InMemoryProductDbContext ctx = CreateTestDatabase();
            BasketRepository basketRepository = new BasketRepository(ctx);

            // Act
            string ownerId = "george";
            Basket basketToAdd = new Basket()
            {
                OwnerID = ownerId,
                BasketItems = new List<BasketItem>()
                {
                    new BasketItem()
                    {
                        ProductId = 1,
                        ProductQuantity = 1,
                        BasketId = 1
                    }
                }
            };
            ctx.Baskets.Add(basketToAdd);
            ctx.SaveChanges();
            Basket retrievedBasket = basketRepository.GetBasketByOwnerId(ownerId);

            // Assert
            Assert.AreEqual(1, retrievedBasket.BasketItems.Count);
            Assert.AreEqual(1, retrievedBasket.BasketItems[0].ProductId);
            Assert.AreEqual(1, retrievedBasket.BasketItems[0].ProductQuantity);
            Assert.AreEqual(1, retrievedBasket.BasketItems[0].BasketId);
        }

        [TestMethod]
        public void AddProductToBasket_GivenProductAndBasketExist_ShouldAddTheProductToTheBasket()
        {
            // Arrange
            InMemoryProductDbContext ctx = CreateTestDatabase();
            BasketRepository basketRepository = new BasketRepository(ctx);
            ProductRepository productRepository = new ProductRepository(ctx);

            // Act
            Product productToAdd = new Product()
            {
                Title = "New Product"
            };
            ctx.Products.Add(productToAdd);
            ctx.SaveChanges();

            string ownerId = "george";
            basketRepository.CreateBasket(ownerId);
            Basket basket = basketRepository.GetBasketByOwnerId(ownerId);

            basketRepository.AddProductToBasket(productToAdd.ProductId, basket.OwnerID);
            basket = basketRepository.GetBasketByOwnerId(ownerId);

            // Assert
            Assert.AreEqual(1, basket.BasketItems.Count);
            Assert.AreEqual(1, basket.BasketItems[0].ProductQuantity);
            Assert.AreEqual(productToAdd.ProductId, basket.BasketItems[0].ProductId);
            Assert.AreEqual(basket.BasketId, basket.BasketItems[0].BasketId);
        }
    }

    public class InMemoryProductDbContext : ProductDbContext
    {
        public InMemoryProductDbContext(
            DbContextOptions<ProductDbContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions
        ) : base(options, operationalStoreOptions)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
        }
    }

    public class dontknow : IOptions<OperationalStoreOptions>
    {
        public OperationalStoreOptions Value = new OperationalStoreOptions();

        OperationalStoreOptions IOptions<OperationalStoreOptions>.Value => new OperationalStoreOptions();
    }
}
