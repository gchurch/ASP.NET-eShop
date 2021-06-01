using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class BasketService : IBasketService
    {

        private readonly IProductRepository _productRepository;

        public BasketService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void AddProductToBasket()
        {
            throw new NotImplementedException();
        }

        public void CreateBasket()
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, int> GetBasketInfo()
        {
            throw new NotImplementedException();
        }

        public void RemoveProductFromBasket()
        {
            throw new NotImplementedException();
        }
    }
}
