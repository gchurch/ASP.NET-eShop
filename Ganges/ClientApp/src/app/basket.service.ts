import { Injectable } from '@angular/core';
import { Product } from './product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  private products: Product[] = [];

  constructor() { }

  addProduct(product: Product) {
    this.products.push(product);
  }

  getProducts() : Product[] {
    return this.products;
  }
}
