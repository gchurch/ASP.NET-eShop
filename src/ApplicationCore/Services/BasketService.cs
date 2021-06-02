﻿using ApplicationCore.Interfaces;
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
            if (_basketRepository.DoesBasketExist(ownerId) != true)
            {
                _basketRepository.CreateBasket(ownerId);
            }
            return _basketRepository.GetBasketByOwnerId(ownerId);
        }

        public void AddProductToBasket(int productId, string ownerId)
        {
            _basketRepository.AddProductToBasket(productId, ownerId);
        }
    }
}
