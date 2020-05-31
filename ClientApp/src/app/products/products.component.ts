import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {
  public products: Product[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) { 
    http.get<Product[]>(baseUrl + 'api/products').subscribe(result => {
      this.products = result;
    }, error => console.error(error));
  }

  ngOnInit() {
  }

}

interface Product {
  Id: number;
  Name: string;
  Description: string;
  Seller: string;
}