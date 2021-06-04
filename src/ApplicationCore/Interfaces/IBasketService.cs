using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IBasketService
    {
        public Basket GetBasket(string OwnerId);

        public void AddProductToBasket(int productId, string ownerId);

        public void RemoveProductFromBasket(int productId, string ownerId);
    }
}