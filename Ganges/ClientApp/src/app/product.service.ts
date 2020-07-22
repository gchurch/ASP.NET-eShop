import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Product } from './product';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  public constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  public getProducts() {
    return this.http.get<Product[]>(this.baseUrl + 'api/products');
  }

  public getProduct(id: number) {
    return this.http.get<Product>(this.baseUrl + 'api/products/' + id);
  }

  public buyProduct(id: number) {
    return this.http.post<number>(this.baseUrl + 'api/products/buy', id);
  }

  public addProduct(product: Product) {
    return this.http.post(this.baseUrl + 'api/products', product);
  }

  public deleteProduct(id: number) {
    return this.http.delete(this.baseUrl + 'api/products/' + id);
  }

  public updateProduct(product: Product) {
    return this.http.patch(this.baseUrl + 'api/products/' + product.id, product);
  }
}