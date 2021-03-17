import { Injectable } from '@angular/core';
import { Product } from '../product';
import { forkJoin, ReplaySubject } from 'rxjs';
import { ProductService } from './product.service';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  private productMap: Map<number, number> = new Map<number, number>();
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
    for (var key of this.productMap.keys()) {
      observablesArray.push(this.productService.getProduct(key));
    }
    forkJoin(observablesArray).subscribe(
      (products: Product[]) =>
        {
          console.log(products);
          for (var product of products) {
            product.quantity = this.productMap.get(product.productId);
          }
          this.products = products;
          this.updateBasketInformation();
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
      this.productMap = new Map<number, number>(JSON.parse(savedMap));
    }
    else {
      this.productMap = new Map<number, number>();
    }
  }

  private saveBasketToLocalStorage(): void {
    localStorage.setItem('basket', JSON.stringify(this.products));
  }

  private saveMapToLocalStorage(): void {
    var string: string = JSON.stringify(Array.from(this.productMap));
    localStorage.setItem('mape', string);
  }

  private updateProductsObservable(): void{
    this.products$.next(this.products);
  }

  public addProduct(product: Product): void {
    this.addProductToMap(product);
    this.fetchProductsInBasket();
  }

  private addProductToMap(product: Product): void {
    if(this.productMap.get(product.productId)) {
      this.productMap.set(product.productId, this.productMap.get(product.productId) + 1);
    }
    else {
      this.productMap.set(product.productId, 1);
    }
    console.log(this.productMap);
    this.saveMapToLocalStorage();
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
    this.removeProductFromMap(product);
    this.fetchProductsInBasket();
  }

  private removeProductFromMap(product: Product): void {
    if(this.productMap.get(product.productId) > 1) {
      this.productMap.set(product.productId, this.productMap.get(product.productId) - 1);
    }
    else {
      this.productMap.delete(product.productId);
    }
    console.log(this.productMap);
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
