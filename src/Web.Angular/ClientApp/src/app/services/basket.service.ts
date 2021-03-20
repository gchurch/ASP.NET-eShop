import { Injectable } from '@angular/core';
import { Product } from '../product';
import { forkJoin, Observable, ReplaySubject } from 'rxjs';
import { ProductService } from './product.service';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  private productIdToQuantity: Map<number, number> = new Map<number, number>();
  private products$ = new ReplaySubject<Product[]>(1);
  private totalCost$ = new ReplaySubject<number>(1);
  private totalNumberOfProducts$ = new ReplaySubject<number>(1);

  public constructor(private productService: ProductService) {
    this.loadBasketFromLocalStorage();
    this.updateBasketInformation();
  }

  private loadBasketFromLocalStorage(): void {
    var savedData = localStorage.getItem('basket');
    if(savedData) {
      console.log(savedData);
      this.productIdToQuantity = new Map<number, number>(JSON.parse(savedData));
    }
    else {
      this.productIdToQuantity = new Map<number, number>();
    }
  }

  private updateBasketInformation(): void {
    var productRequests: Observable<Product>[] = this.createProductRequests();
    if(productRequests.length == 0) {
      this.updateObservables([]);
    } else {
      this.fetchProductsAndUpdateObservables(productRequests);
    }
  }

  private createProductRequests(): Observable<Product>[] {
    var observablesArray: Observable<Product>[] = [];
    for (var key of this.productIdToQuantity.keys()) {
      var productRequestObservable: Observable<Product> = this.productService.getProductById(key);
      observablesArray.push(productRequestObservable);
    }
    return observablesArray;
  }

  private updateObservables(products: Product[]): void {
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

  private fetchProductsAndUpdateObservables(productRequests: Observable<Product>[]): void {
    if(productRequests.length == 0) {
      this.updateObservables([]);
    }
    else {
      forkJoin(productRequests).subscribe(
        (products: Product[]) =>
          {
            products = this.setProductQuantitiesToCorrectValues(products);
            this.updateObservables(products);
          }
      );
    }
  }

  private setProductQuantitiesToCorrectValues(products :Product[]): Product[] {
    for (var product of products) {
      product.quantity = this.productIdToQuantity.get(product.productId);
    }
    return products;
  }

  public addProduct(productId: number): void {
    this.incrementProductQuantity(productId);
    console.log(this.productIdToQuantity);
    this.updateBasketInformation();
    this.saveBasketToLocalStorage();
  }

  private incrementProductQuantity(productId: number) {
    if(this.productIdToQuantity.get(productId)) {
      this.productIdToQuantity.set(productId, this.productIdToQuantity.get(productId) + 1);
    }
    else {
      this.productIdToQuantity.set(productId, 1);
    }
  }

  public removeProduct(productId: number): void {
    this.decrementProductQuantity(productId);
    console.log(this.productIdToQuantity);
    this.updateBasketInformation();
    this.saveBasketToLocalStorage();
  }

  private decrementProductQuantity(productId: number) {
    if(this.productIdToQuantity.get(productId) > 1) {
      this.productIdToQuantity.set(productId, this.productIdToQuantity.get(productId) - 1);
    }
    else {
      this.productIdToQuantity.delete(productId);
    }
  }

  private saveBasketToLocalStorage(): void {
    var string: string = JSON.stringify(Array.from(this.productIdToQuantity));
    localStorage.setItem('basket', string);
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
