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
            List<Product> productsToAdd = new List<Product>() {
                new Product()
                {
                    Title = "Product 1"
                }
            };
            foreach (var product in productsToAdd)
            {
                ctx.Products.Add(product);
            }
            ctx.SaveChanges();

            // Add new basket to database
            string ownerId = "george";
            basketRepository.CreateBasket(ownerId);

            // Add products to basket
            Basket basket = basketRepository.GetBasketByOwnerId(ownerId);
            foreach (var product in productsToAdd)
            {
                basketRepository.AddProductToBasket(product.ProductId, basket.OwnerID);
            }
            Basket retrievedBasket = basketRepository.GetBasketByOwnerId(ownerId);

            // Assert
            Assert.AreEqual(1, retrievedBasket.BasketItems.Count);
            Assert.AreEqual(1, retrievedBasket.BasketItems[0].ProductId);
        }

        [TestMethod]
        public void GetBasketByOwnerId_GivenThatTheBasketExists_ShouldReturnTheProductObjectOfEachBasketItem()
        {
            // Arrange
            InMemoryProductDbContext ctx = CreateTestDatabase();
            BasketRepository basketRepository = new BasketRepository(ctx);

            // Act
            List<Product> productsToAdd = new List<Product>() {
                new Product()
                {
                    Title = "Product 1"
                }
            };
            foreach (var product in productsToAdd)
            {
                ctx.Products.Add(product);
            }
            ctx.SaveChanges();

            // Add new basket to database
            string ownerId = "george";
            basketRepository.CreateBasket(ownerId);

            // Add products to basket
            Basket basket = basketRepository.GetBasketByOwnerId(ownerId);
            foreach (var product in productsToAdd)
            {
                basketRepository.AddProductToBasket(product.ProductId, basket.OwnerID);
            }
            basket = basketRepository.GetBasketByOwnerId(ownerId);

            // Assert
            Assert.AreNotEqual(null, basket.BasketItems[0].Product);
        }

        [TestMethod]
        public void AddProductToBasket_GivenProductAndBasketExist_ShouldAddTheProductToTheBasket()
        {
            // Arrange
            InMemoryProductDbContext ctx = CreateTestDatabase();
            BasketRepository basketRepository = new BasketRepository(ctx);

            // Act
            // Add new product to database
            Product productToAdd = new Product()
            {
                Title = "New Product"
            };
            ctx.Products.Add(productToAdd);
            ctx.SaveChanges();

            // Add new basket to database
            string ownerId = "george";
            basketRepository.CreateBasket(ownerId);

            // Add product to basket
            Basket basket = basketRepository.GetBasketByOwnerId(ownerId);
            basketRepository.AddProductToBasket(productToAdd.ProductId, basket.OwnerID);
            basket = basketRepository.GetBasketByOwnerId(ownerId);

            // Assert
            Assert.AreEqual(1, basket.BasketItems.Count);
            Assert.AreEqual(1, basket.BasketItems[0].ProductQuantity);
            Assert.AreEqual(productToAdd.ProductId, basket.BasketItems[0].ProductId);
            Assert.AreEqual(basket.BasketId, basket.BasketItems[0].BasketId);
        }


        [TestMethod]
        public void RemoveProductFromBasket_GivenProductAndBasketExist_ShouldRemoveTheProductToTheBasket()
        {
            // Arrange
            InMemoryProductDbContext ctx = CreateTestDatabase();
            BasketRepository basketRepository = new BasketRepository(ctx);

            // Act
            // Add new product to database
            List<Product> productsToAdd = new List<Product>() {
                new Product()
                {
                    Title = "Product 1"
                },
                new Product()
                {
                    Title = "Product 2"
                },
                new Product()
                {
                    Title = "Product 3"
                }
            };
            foreach (var product in productsToAdd)
            {
                ctx.Products.Add(product);
            }
            ctx.SaveChanges();

            // Add new basket to database
            string ownerId = "george";
            basketRepository.CreateBasket(ownerId);

            // Add products to basket
            Basket basket = basketRepository.GetBasketByOwnerId(ownerId);
            foreach (var product in productsToAdd)
            {
                basketRepository.AddProductToBasket(product.ProductId, basket.OwnerID);
            }

            // Delete product from basket
            basketRepository.RemoveProductFromBasket(productsToAdd[1].ProductId, ownerId);
            basket = basketRepository.GetBasketByOwnerId(ownerId);

            // Assert
            Assert.AreEqual(2, basket.BasketItems.Count);
            Assert.AreEqual(1, basket.BasketItems[0].ProductId);
            Assert.AreEqual(3, basket.BasketItems[1].ProductId);
        }

        [TestMethod]
        public void IncrementProductQuantityInBasket_GivenProductExistsInBasket_ShouldIncrementProductQuantity()
        {
            // Arrange
            InMemoryProductDbContext ctx = CreateTestDatabase();
            BasketRepository basketRepository = new BasketRepository(ctx);

            // Act
            List<Product> productsToAdd = new List<Product>() {
                new Product()
                {
                    Title = "Product 1"
                },
                new Product()
                {
                    Title = "Product 2"
                }
            };
            foreach (var product in productsToAdd)
            {
                ctx.Products.Add(product);
            }
            ctx.SaveChanges();

            // Add new basket to database
            string ownerId = "george";
            basketRepository.CreateBasket(ownerId);

            // Add products to basket
            Basket basket = basketRepository.GetBasketByOwnerId(ownerId);
            foreach (var product in productsToAdd)
            {
                basketRepository.AddProductToBasket(product.ProductId, basket.OwnerID);
            }

            basketRepository.IncrementProductQuantityInBasket(2, ownerId);
            basket = basketRepository.GetBasketByOwnerId(ownerId);

            // Assert
            Assert.AreEqual(1, basket.BasketItems[0].ProductQuantity);
            Assert.AreEqual(2, basket.BasketItems[1].ProductQuantity);
        }

        [TestMethod]
        public void IsProductInBasket_GivenProductIsInBasket_ShouldReturnTrue()
        {
            // Arrange
            InMemoryProductDbContext ctx = CreateTestDatabase();
            BasketRepository basketRepository = new BasketRepository(ctx);

            // Act
            List<Product> productsToAdd = new List<Product>() {
                new Product()
                {
                    Title = "Product 1"
                },
                new Product()
                {
                    Title = "Product 2"
                }
            };
            foreach (var product in productsToAdd)
            {
                ctx.Products.Add(product);
            }
            ctx.SaveChanges();

            // Add new basket to database
            string ownerId = "george";
            basketRepository.CreateBasket(ownerId);

            // Add products to basket
            Basket basket = basketRepository.GetBasketByOwnerId(ownerId);
            foreach (var product in productsToAdd)
            {
                basketRepository.AddProductToBasket(product.ProductId, basket.OwnerID);
            }
            int productIdToSearchFor = 1;
            bool isProductInBasket = basketRepository.IsProductInBasket(productIdToSearchFor, ownerId);

            // Assert
            Assert.AreEqual(true, isProductInBasket);
        }

        [TestMethod]
        public void IsProductInBasket_GivenProductIsNotInBasket_ShouldReturnFalse()
        {
            // Arrange
            InMemoryProductDbContext ctx = CreateTestDatabase();
            BasketRepository basketRepository = new BasketRepository(ctx);

            // Act
            List<Product> productsToAdd = new List<Product>() {
                new Product()
                {
                    Title = "Product 1"
                },
                new Product()
                {
                    Title = "Product 2"
                }
            };
            foreach (var product in productsToAdd)
            {
                ctx.Products.Add(product);
            }
            ctx.SaveChanges();

            // Add new basket to database
            string ownerId = "george";
            basketRepository.CreateBasket(ownerId);

            // Add products to basket
            Basket basket = basketRepository.GetBasketByOwnerId(ownerId);
            foreach (var product in productsToAdd)
            {
                basketRepository.AddProductToBasket(product.ProductId, basket.OwnerID);
            }

            int productIdToSearchFor = 3;
            bool isProductInBasket = basketRepository.IsProductInBasket(productIdToSearchFor, ownerId);

            // Assert
            Assert.AreEqual(false, isProductInBasket);
        }

        [TestMethod]
        public void DecrementProductQuantityInBasket_GivenProductExistsInBasket_ShouldDecrementProductQuantity()
        {
            // Arrange
            InMemoryProductDbContext ctx = CreateTestDatabase();
            BasketRepository basketRepository = new BasketRepository(ctx);

            // Act
            List<Product> productsToAdd = new List<Product>() {
                new Product()
                {
                    Title = "Product 1"
                },
                new Product()
                {
                    Title = "Product 2"
                }
            };
            foreach (var product in productsToAdd)
            {
                ctx.Products.Add(product);
            }
            ctx.SaveChanges();

            // Add new basket to database
            string ownerId = "george";
            basketRepository.CreateBasket(ownerId);

            // Add products to basket
            Basket basket = basketRepository.GetBasketByOwnerId(ownerId);
            foreach (var product in productsToAdd)
            {
                basketRepository.AddProductToBasket(product.ProductId, basket.OwnerID);
            }

            int idOfProductToDecrement = 2;
            basketRepository.DecrementProductQuantityInBasket(idOfProductToDecrement, ownerId);
            basket = basketRepository.GetBasketByOwnerId(ownerId);

            // Assert
            Assert.AreEqual(1, basket.BasketItems[0].ProductId);
            Assert.AreEqual(1, basket.BasketItems[0].ProductQuantity);
            Assert.AreEqual(2, basket.BasketItems[1].ProductId);
            Assert.AreEqual(0, basket.BasketItems[1].ProductQuantity);
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
