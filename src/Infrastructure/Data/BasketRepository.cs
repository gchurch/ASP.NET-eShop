using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class BasketRepository : IBasketRepository
    {
        private readonly ProductDbContext _context;

        public BasketRepository(ProductDbContext context)
        {
            _context = context;
        }

        public Basket GetBasket(string ownerId)
        {
            if(DoesBasketExist(ownerId) != true)
            {
                CreateBasket(ownerId);
            }
            return GetBasketByOwnerId(ownerId);
        }

        private bool DoesBasketExist(string OwnerId)
        {
            var query = from basket in _context.Baskets where basket.OwnerID == OwnerId select basket;
            Basket retrievedBasket = query.AsNoTracking().FirstOrDefault();
            if (retrievedBasket != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void CreateBasket(string OwnerId)
        {
            Basket newBasket = new Basket()
            {
                OwnerID = OwnerId
            };
            _context.Baskets.Add(newBasket);
            AddTestProductToBasket(OwnerId);
            _context.SaveChanges();
        }

        private void AddTestProductToBasket(string OwnerId)
        {
            var query = from product in _context.Products select product;
            Product firstProduct = query.AsNoTracking().FirstOrDefault();
            if(firstProduct != null)
            {
                AddProductToBasket(firstProduct.ProductId, OwnerId);
            }
        }

        private Basket GetBasketByOwnerId(string ownerId)
        {
            var query = from basket in _context.Baskets where basket.OwnerID == ownerId select basket;
            Basket retrievedBasket = query.AsNoTracking().Include(b => b.BasketItems).Single();
            return retrievedBasket;
        }

        public void AddProductToBasket(int productId, string ownerId)
        {
            var basketQuery = from basket in _context.Baskets where basket.OwnerID == ownerId select basket;
            Basket retrievedBasket = basketQuery.Single();
            var productQuery = from product in _context.Products where product.ProductId == productId select product;
            Product retrievedProduct = productQuery.Single();
            BasketItem basketItem = new BasketItem()
            {
                Product = retrievedProduct,
                ProductQuantity = 1,
                basket = retrievedBasket
            };
            retrievedBasket.BasketItems.Add(basketItem);
            _context.SaveChanges();
        }
    }
}
