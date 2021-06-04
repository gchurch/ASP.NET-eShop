using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class BasketService : IBasketService
    {

        private readonly IBasketRepository _basketRepository;

        public BasketService(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public Basket GetBasket(string ownerId)
        {
            if (_basketRepository.DoesBasketExist(ownerId) == false)
            {
                _basketRepository.CreateBasket(ownerId);
            }
            return _basketRepository.GetBasketByOwnerId(ownerId);
        }

        public void AddProductToBasket(int productId, string ownerId)
        {
            if(_basketRepository.DoesBasketExist(ownerId) == false)
            {
                _basketRepository.CreateBasket(ownerId);
            }

            if (_basketRepository.IsProductInBasket(productId, ownerId) == false)
            {
                _basketRepository.AddProductToBasket(productId, ownerId);
            }
            else
            {
                _basketRepository.IncrementProductQuantityInBasket(productId, ownerId);
            }
        }

        public void RemoveProductFromBasket(int productId, string ownerId)
        {
            if (_basketRepository.DoesBasketExist(ownerId) == false)
            {
                _basketRepository.CreateBasket(ownerId);
            }

            if (_basketRepository.IsProductInBasket(productId, ownerId))
            {
                if (_basketRepository.GetProductQuantityInBasket(productId, ownerId) > 1)
                {
                    _basketRepository.DecrementProductQuantityInBasket(productId, ownerId);
                }
                else
                {
                    _basketRepository.RemoveProductFromBasket(productId, ownerId);
                }
            }
        }

        public string GetProductQuantitiesInBasketAsJsonString(string ownerId)
        {
            return _basketRepository.GetProductQuantitiesInBasketAsAJsonString(ownerId);
        }
    }
}
