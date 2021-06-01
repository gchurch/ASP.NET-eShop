using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IBasketService
    {
        public void CreateBasket();

        public void AddProductToBasket();

        public void RemoveProductFromBasket();

        public Dictionary<int, int> GetBasketInfo();
    }
}
