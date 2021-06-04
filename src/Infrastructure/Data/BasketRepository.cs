using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

        public void CreateBasket(string OwnerId)
        {
            Basket newBasket = new Basket()
            {
                OwnerID = OwnerId,
                BasketItems = new List<BasketItem>()
            };
            _context.Baskets.Add(newBasket);
            _context.SaveChanges();
        }

        public bool DoesBasketExist(string ownerId)
        {
            var query = from basket in _context.Baskets where basket.OwnerID == ownerId select basket;
            Basket retrievedBasket = query.AsNoTracking().FirstOrDefault();
            if (retrievedBasket == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public Basket GetBasketByOwnerId(string ownerId)
        {
            var query = from basket in _context.Baskets where basket.OwnerID == ownerId select basket;
            Basket retrievedBasket = query.AsNoTracking().Include(b => b.BasketItems).ThenInclude(b => b.Product).FirstOrDefault();
            return retrievedBasket;
        }

        public Basket GetBasketByOwnerIdTracked(string ownerId)
        {
            var query = from basket in _context.Baskets where basket.OwnerID == ownerId select basket;
            Basket retrievedBasket = query.Include(b => b.BasketItems).ThenInclude(b => b.Product).FirstOrDefault();
            return retrievedBasket;
        }

        public string GetProductQuantitiesInBasketAsAJsonString(string ownerId)
        {
            var basketItemsQuery = from basket in _context.Baskets where basket.OwnerID == ownerId select basket.BasketItems;
            var productQuantitiesQuery = from basketItem in basketItemsQuery.AsNoTracking().FirstOrDefault() 
                         select new { productId = basketItem.ProductId, quantity = basketItem.ProductQuantity };
            var productsList = productQuantitiesQuery.ToList();
            return JsonConvert.SerializeObject(productsList);
        }


        public void AddTestProductToBasket(string OwnerId)
        {
            var query = from product in _context.Products select product;
            Product firstProduct = query.AsNoTracking().FirstOrDefault();
            if (firstProduct != null)
            {
                AddProductToBasket(firstProduct.ProductId, OwnerId);
            }
        }

        public void AddProductToBasket(int productId, string ownerId)
        {
            Basket retrievedBasket = GetBasketByOwnerIdTracked(ownerId);
            Product retrievedProduct = GetProductByProductId(productId);
            BasketItem basketItem = new BasketItem()
            {
                Product = retrievedProduct,
                ProductQuantity = 1,
                basket = retrievedBasket
            };
            retrievedBasket.BasketItems.Add(basketItem);
            _context.SaveChanges();
        }

        private Product GetProductByProductId(int productId)
        {
            var productQuery = from product in _context.Products where product.ProductId == productId select product;
            Product retrievedProduct = productQuery.FirstOrDefault();
            return retrievedProduct;
        }

        public void RemoveProductFromBasket(int productId, string ownerId)
        {
            Basket basket = GetBasketByOwnerIdTracked(ownerId);
            for(var i = 0; i < basket.BasketItems.Count; i++)
            {
                if(basket.BasketItems[i].ProductId == productId)
                {
                    basket.BasketItems.RemoveAt(i);
                }
            }
            _context.SaveChanges();
        }

        public void IncrementProductQuantityInBasket(int productId, string ownerId)
        {
            Basket basket = GetBasketByOwnerIdTracked(ownerId);
            var query = from basketItem in basket.BasketItems where basketItem.ProductId == productId select basketItem;
            BasketItem matchingBasketItem = query.FirstOrDefault();
            if (matchingBasketItem != null)
            {
                matchingBasketItem.ProductQuantity += 1;
                _context.SaveChanges();
            }
        }

        public bool IsProductInBasket(int productId, string ownerId)
        {
            Basket basket = GetBasketByOwnerId(ownerId);
            var query = from basketItem in basket.BasketItems where basketItem.ProductId == productId select basketItem;
            bool isProductInBasket = query.Any();
            return isProductInBasket;
        }

        public void DecrementProductQuantityInBasket(int productId, string ownerId)
        {
            Basket basket = GetBasketByOwnerIdTracked(ownerId);
            var query = from basketItem in basket.BasketItems where basketItem.ProductId == productId select basketItem;
            BasketItem matchingBasketItem = query.FirstOrDefault();
            if (matchingBasketItem != null)
            {
                matchingBasketItem.ProductQuantity -= 1;
                _context.SaveChanges();
            }
        }

        public int GetProductQuantityInBasket(int productId, string ownerId)
        {
            Basket basket = GetBasketByOwnerId(ownerId);
            var query = from basketItem in basket.BasketItems where basketItem.ProductId == productId select basketItem.ProductQuantity;
            int quantity = query.FirstOrDefault();
            return quantity;
        }
    }
}
