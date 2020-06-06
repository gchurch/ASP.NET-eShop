import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from '../product.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {

  product: Product;
  responseReceived: boolean = false;

  constructor(private route: ActivatedRoute, private productService: ProductService) {}

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      var id: number = +params.get('id');
      this.productService.getProduct(id).subscribe(
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