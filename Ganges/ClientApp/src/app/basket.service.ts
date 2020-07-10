import { Injectable } from '@angular/core';
import { Product } from './product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  private products = {};

  constructor() { }

  addProduct(product: Product) : void {
    if(!this.products[product.id]) {
      product.quantity = 1;
      this.products[product.id] = product;
    }
    else {
      this.products[product.id].quantity++;
    }
  }

  getProducts() {
    var productsArray = [];
    for(var id in this.products) {
      productsArray.push(this.products[id]);
    }
    return productsArray;
  }

  removeProduct(product: Product) {
    if(this.products[product.id] && this.products[product.id].quantity > 1) {
      this.products[product.id].quantity--;
    }
    else {
      delete this.products[product.id];
    }
  }
}
