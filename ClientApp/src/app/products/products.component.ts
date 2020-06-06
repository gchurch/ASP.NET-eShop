import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {
  products: Product[];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { 
    this.baseUrl = baseUrl;
  }

  ngOnInit() {
    this.http.get<Product[]>(this.baseUrl + 'api/products').subscribe(result => {
      this.products = result;
    }, error => console.error(error));
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