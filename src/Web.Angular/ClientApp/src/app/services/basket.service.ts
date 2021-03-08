import { Injectable } from '@angular/core';
import { Product } from '../product';
import { ReplaySubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  // The basket property is a map from product ID to product. This so that it is easy to track the quantity 
  // of each product in the basket.
  private products: Product[] = [];

  // https://rxjs-dev.firebaseapp.com/guide/subject
  // Here I am using subjects for the total cost of the products and the number of products in the basket.
  // Components will need to subscribe to these to get the latest value.
  private totalCost$ = new ReplaySubject<number>(1);
  private numberOfProducts$ = new ReplaySubject<number>(1);
  private products$ = new ReplaySubject<Product[]>(1);

  constructor() {
    this.loadBasket();
    this.calculateTotalCost();
    this.calculateNumberOfProducts();
  }

  // Load the basket from localStorage if one has been saved
  private loadBasket() : void {
    var savedBasket = localStorage.getItem('basket');
    if(savedBasket) {
      this.products = JSON.parse(savedBasket);
    }
    else {
      this.products = [];
    }
    this.products$.next(this.products);
  }

  // Save the basket to localStorage
  private saveBasket() : void {
    localStorage.setItem('basket', JSON.stringify(this.products));
  }

  // If the product is not already in the basket then add it. If the product is already in the basket then
  // increment the quantity.
  addProduct(product: Product) : void {
    // Check if the product is already in the basket and if so just increase the quantity of the product by one.
    var productAlreadyInBasket: boolean = false;
    for(var i: number = 0; i < this.products.length; i++) {
      if(product.productId == this.products[i].productId) {
        this.products[i].quantity++;
        productAlreadyInBasket = true;
        break;
      }
    }
    // If the product is not already in the basket then add it.
    if(!productAlreadyInBasket) {
      product.quantity = 1;
      this.products.push(product);
    }
    this.products$.next(this.products);
    this.saveBasket();
    this.calculateTotalCost();
    this.calculateNumberOfProducts();
  }

  // Decrease the product quantity if it is greater than 1. Otherwise remove the product from the basket.
  removeProduct(product: Product) : void {

    for(var i: number = 0; i < this.products.length; i++) {
      if(this.products[i].productId == product.productId) {
        if(this.products[i].quantity > 1) {
          this.products[i].quantity--;
        }
        else {
          this.products.splice(i, 1);
        }
        break;
      }
    }
    this.products$.next(this.products);
    this.saveBasket();
    this.calculateTotalCost();
    this.calculateNumberOfProducts();
  }

  calculateTotalCost() : void {
    var totalCost: number = 0;
    for(var product of this.products) {
      totalCost += product.price * product.quantity;
    }
    this.totalCost$.next(totalCost);
  }

  calculateNumberOfProducts() : void {
    var numberOfProducts: number = 0;
    for(var product of this.products) {
      numberOfProducts += product.quantity;
    }
    this.numberOfProducts$.next(numberOfProducts);
  }

  // Create an array of products out of the basket property
  getProducts() : ReplaySubject<Product[]> {
    return this.products$;
  }

  getTotalCost() : ReplaySubject<number> {
    return this.totalCost$;
  }

  getNumberOfProducts(): ReplaySubject<number> {
    return this.numberOfProducts$;
  }
}
