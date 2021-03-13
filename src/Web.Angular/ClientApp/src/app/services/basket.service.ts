import { Injectable } from '@angular/core';
import { Product } from '../product';
import { ReplaySubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  private products: Product[] = [];
  private products$ = new ReplaySubject<Product[]>(1);
  private totalCost$ = new ReplaySubject<number>(1);
  private totalNumberOfProducts$ = new ReplaySubject<number>(1);

  public constructor() {
    this.loadBasketFromLocalStorage();
    this.updateTotalCostObservable();
    this.updateTotalNumberOfProductsObservable();
    this.updateProductsObservable();
  }

  private loadBasketFromLocalStorage(): void {
    var savedBasket = localStorage.getItem('basket');
    if(savedBasket) {
      this.products = JSON.parse(savedBasket);
    }
    else {
      this.products = [];
    }
  }

  private updateProductsObservable(): void{
    this.products$.next(this.products);
  }

  private saveBasketToLocalStorage(): void {
    localStorage.setItem('basket', JSON.stringify(this.products));
  }

  public addProduct(product: Product): void {
    if(this.isProductInProductsArray(product) == true) {
      var index: number = this.findProductIndexInProductsArray(product);
      this.products[index].quantity++;
    } else {
      product.quantity = 1;
      this.products.push(product);
    }
    this.updateBasketInformation();
  }

  private isProductInProductsArray(product: Product): boolean {
    for(var i: number = 0; i < this.products.length; i++) {
      if(product.productId == this.products[i].productId) {
        return true;
      }
    }
    return false;
  }

  private findProductIndexInProductsArray(product: Product): number {
    for(var i: number = 0; i < this.products.length; i++) {
      if(product.productId == this.products[i].productId) {
        return i;
      }
    }
    return -1;
  }

  private updateBasketInformation(): void {
    this.saveBasketToLocalStorage();
    this.updateProductsObservable();
    this.updateTotalCostObservable();
    this.updateTotalNumberOfProductsObservable();
  }

  private updateTotalCostObservable(): void {
    var totalCost: number = 0;
    for(var product of this.products) {
      totalCost += product.price * product.quantity;
    }
    this.totalCost$.next(totalCost);
  }

  private updateTotalNumberOfProductsObservable(): void {
    var numberOfProducts: number = 0;
    for(var product of this.products) {
      numberOfProducts += product.quantity;
    }
    this.totalNumberOfProducts$.next(numberOfProducts);
  }

  public removeProduct(product: Product): void {
    var productIndex: number = this.findProductIndexInProductsArray(product);
    if(this.products[productIndex].quantity > 1) {
      this.products[productIndex].quantity--;
    }
    else {
      this.products.splice(productIndex, 1);
    }
    this.updateBasketInformation();
  }

  public getProducts$(): ReplaySubject<Product[]> {
    return this.products$;
  }

  public getTotalCost$(): ReplaySubject<number> {
    return this.totalCost$;
  }

  public getNumberOfProducts$(): ReplaySubject<number> {
    return this.totalNumberOfProducts$;
  }
}
