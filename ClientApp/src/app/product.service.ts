import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

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
}

interface Product {
  id: number;
  title: string;
  description: string;
  seller: string;
  price: number;
  quantity: number;
  imageUrl: string;
}
