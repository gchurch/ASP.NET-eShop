import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Product } from '../product';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  public constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  public getProducts() : Observable<Product[]> {
    return this.http.get<Product[]>(this.baseUrl + 'api/products');
  }

  public getProduct(id: number) : Observable<Product> {
    return this.http.get<Product>(this.baseUrl + 'api/products/' + id);
  }

  public buyProduct(id: number) : Observable<number> {
    return this.http.post<number>(this.baseUrl + 'api/products/buy', id);
  }

  public addProduct(product: Product) : Observable<Product> {
    return this.http.post<Product>(this.baseUrl + 'api/products', product);
  }

  public deleteProduct(id: number) : Observable<Object> {
    return this.http.delete(this.baseUrl + 'api/products/' + id);
  }

  public updateProduct(product: Product) : Observable<Product> {
    return this.http.put<Product>(this.baseUrl + 'api/products/', product);
  }
}