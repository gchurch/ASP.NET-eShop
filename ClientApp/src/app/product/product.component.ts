import { Component, OnInit, Inject } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {

  id: number;
  product: Product;
  responseReceived: boolean = false;

  constructor(private route: ActivatedRoute, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {}

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.id = +params.get('id');
      this.http.get<Product>(this.baseUrl + 'api/products/' + this.id).subscribe(
        result => {
          this.product = result;
          this.responseReceived = true;
        }, 
        error => {
          console.error(error);
          this.responseReceived = true;
        }
      );
    });
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