using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IBasketRepository
    {
        public bool DoesBasketExist(string ownerId);

        public void CreateBasket(string OwnerId);

        public Basket GetBasketByOwnerId(string ownerId);

        public string GetProductQuantitiesInBasketAsAJsonString(string ownerId);

        public void AddTestProductToBasket(string OwnerId);

        public void AddProductToBasket(int productId, string ownerId);

        public void RemoveProductFromBasket(int productId, string ownerId);

        public void IncrementProductQuantityInBasket(int productId, string ownerId);

        public bool IsProductInBasket(int productId, string ownerId);

        public void DecrementProductQuantityInBasket(int productId, string ownerId);

        public int GetProductQuantityInBasket(int productId, string ownerId);
    }
}
