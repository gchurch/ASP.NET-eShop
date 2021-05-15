import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from '../../product';

@Injectable({
  providedIn: 'root'
})
export class BasketServiceMock {

  constructor() { }

  public getProducts$() {
    return new Observable<Product>();
  }

  public getTotalCost$() {
    return new Observable<number>();
  }

  public getNumberOfProducts$() {
    return new Observable<number>();
  }
}
