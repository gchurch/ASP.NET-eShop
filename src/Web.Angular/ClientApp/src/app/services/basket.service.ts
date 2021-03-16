import { Injectable } from '@angular/core';
import { Product } from '../product';
import { forkJoin, ReplaySubject } from 'rxjs';
import { ProductService } from './product.service';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  private productIds: Map<number, number> = new Map<number, number>();
  private products: Product[] = [];
  private upToDateProducts: Product[] = [];
  private products$ = new ReplaySubject<Product[]>(1);
  private totalCost$ = new ReplaySubject<number>(1);
  private totalNumberOfProducts$ = new ReplaySubject<number>(1);

  public constructor(private productService: ProductService) {
    this.loadBasketFromLocalStorage();
    this.loadMapFromLocalStore();
    this.updateTotalCostObservable();
    this.updateTotalNumberOfProductsObservable();
    this.updateProductsObservable();
    this.fetchProductsInBasket();
  }

  private fetchProductsInBasket(): void {
    var observablesArray = [];
    for (var key of this.productIds.keys()) {
      observablesArray.push(this.productService.getProduct(key));
    }
    forkJoin(observablesArray).subscribe(results =>
      {
        console.log(results);
        this.upToDateProducts = results as Product[];
      }
    );
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

  private loadMapFromLocalStore(): void {
    var savedMap = localStorage.getItem('mape');
    if(savedMap) {
      console.log(savedMap);
      this.productIds = new Map<number, number>(JSON.parse(savedMap));
    }
    else {
      this.productIds = new Map<number, number>();
    }
  }

  private saveBasketToLocalStorage(): void {
    localStorage.setItem('basket', JSON.stringify(this.products));
  }

  private saveMapToLocalStorage(): void {
    var string: string = JSON.stringify(Array.from(this.productIds));
    localStorage.setItem('mape', string);
  }

  private updateProductsObservable(): void{
    this.products$.next(this.products);
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
    this.addProductToMap(product);
    this.fetchProductsInBasket();
  }

  private addProductToMap(product: Product): void {
    if(this.productIds.get(product.productId)) {
      this.productIds.set(product.productId, this.productIds.get(product.productId) + 1);
    }
    else {
      this.productIds.set(product.productId, 1);
    }
    console.log(this.productIds);
    this.saveMapToLocalStorage();
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
    this.removeProductFromMap(product);
    this.fetchProductsInBasket();
  }

  private removeProductFromMap(product: Product): void {
    if(this.productIds.get(product.productId) > 1) {
      this.productIds.set(product.productId, this.productIds.get(product.productId) - 1);
    }
    else {
      this.productIds.delete(product.productId);
    }
    console.log(this.productIds);
    this.saveMapToLocalStorage();
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
