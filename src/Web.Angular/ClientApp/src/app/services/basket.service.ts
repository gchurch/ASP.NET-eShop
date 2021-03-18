import { Injectable } from '@angular/core';
import { Product } from '../product';
import { forkJoin, ReplaySubject } from 'rxjs';
import { ProductService } from './product.service';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  private productMap: Map<number, number> = new Map<number, number>();
  private products$ = new ReplaySubject<Product[]>(1);
  private totalCost$ = new ReplaySubject<number>(1);
  private totalNumberOfProducts$ = new ReplaySubject<number>(1);

  public constructor(private productService: ProductService) {
    this.loadMapFromLocalStore();
    this.fetchProductsInBasket();
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

  private saveMapToLocalStorage(): void {
    var string: string = JSON.stringify(Array.from(this.productMap));
    localStorage.setItem('mape', string);
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
          this.updateBasketInformation(products);
        }
    );
  }

  private updateBasketInformation(products: Product[]): void {
    this.updateProductsObservable(products);
    this.updateTotalCostObservable(products);
    this.updateTotalNumberOfProductsObservable(products);
  }

  private updateProductsObservable(products: Product[]): void{
    this.products$.next(products);
  }

  private updateTotalCostObservable(products: Product[]): void {
    var totalCost: number = 0;
    for(var product of products) {
      totalCost += product.price * product.quantity;
    }
    this.totalCost$.next(totalCost);
  }

  private updateTotalNumberOfProductsObservable(products: Product[]): void {
    var numberOfProducts: number = 0;
    for(var product of products) {
      numberOfProducts += product.quantity;
    }
    this.totalNumberOfProducts$.next(numberOfProducts);
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
