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

        public void AddTestProductToBasket(string OwnerId);

        public void AddProductToBasket(int productId, string ownerId);
    }
}
