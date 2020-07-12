import { Injectable } from '@angular/core';
import { Product } from './product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  // The basket property is a map from product ID to product. This so that it is easy to track the quantity 
  // of each product in the basket.
  private basket = {};

  constructor() { 
    this.loadBasket();
  }

  // Load the basket from localStorage if one has been saved
  loadBasket() : void {
    var storedBasket = localStorage.getItem('basket');
    if(storedBasket) {
      this.basket = JSON.parse(storedBasket);
    }
    else {
      this.basket = {};
    }
  }

  // Save the basket to localStorage
  storeBasket() : void {
    localStorage.setItem('basket', JSON.stringify(this.basket));
  }

  // If the product is not already in the basket then add it. If the product is already in the basket then
  // increment the quantity.
  addProduct(product: Product) : void {
    if(!this.basket[product.id]) {
      product.quantity = 1;
      this.basket[product.id] = product;
    }
    else {
      this.basket[product.id].quantity++;
    }
    this.storeBasket();
  }

  // Create an array of products out of the basket property
  getProducts() : Product[] {
    var products = [];
    for(var id in this.basket) {
      if(this.basket[id]) {
        products.push(this.basket[id]);
      }
    }
    return products;
  }

  // Decrease the product quantity if it is greater than 1. Otherwise remove the product from the basket.
  removeProduct(product: Product) : void {
    if(this.basket[product.id] && this.basket[product.id].quantity > 1) {
      this.basket[product.id].quantity--;
    }
    else {
      delete this.basket[product.id];
    }
    this.storeBasket();
  }

  getTotalCost() : number {
    var totalCost: number = 0;
    for(var id in this.basket) {
      totalCost += this.basket[id].price * this.basket[id].quantity;
    }
    return totalCost;
  }

  getNumberOfItems() : number {
    var numberOfItems: number = 0;
    for(var id in this.basket) {
      numberOfItems += this.basket[id].quantity;
    }
    return numberOfItems;
  }
}
