import { Injectable } from '@angular/core';
import { Product } from './product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  // The basket property is a map from product ID to product. This so that it is easy to track the quantity 
  // of each product in the basket.
  private basket = {};

  constructor() { }

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
  }

  // Create an array of products out of the basket property
  getProducts() {
    var products = [];
    for(var id in this.basket) {
      if(this.basket[id]) {
        products.push(this.basket[id]);
      }
    }
    return products;
  }

  // Decrease the product quantity if it is greater than 1. Otherwise remove the product from the basket.
  removeProduct(product: Product) {
    if(this.basket[product.id] && this.basket[product.id].quantity > 1) {
      this.basket[product.id].quantity--;
    }
    else {
      delete this.basket[product.id];
    }
  }
}
