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

  constructor(private route: ActivatedRoute, private http: HttpClient, @Inject('BASE_URL') baseUrl: string) { 
    this.route.paramMap.subscribe(params => {
      this.id = +params.get('id');
      http.get<Product>(baseUrl + 'api/products/' + this.id).subscribe(result => {
        this.product = result;
      }, error => console.error(error));
    });
  }

  ngOnInit() {}

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