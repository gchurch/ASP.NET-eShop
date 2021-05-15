import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from '../../product';

@Injectable({
  providedIn: 'root'
})
export class ProductServiceMock {

  constructor() { }

  public getAllProducts() {
    return new Observable<Product[]>();
  }
}