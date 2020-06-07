import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from '../product.service';
import { Observable } from 'rxjs';
import { Product } from '../product';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {

  product$: Observable<Product>;

  constructor(private route: ActivatedRoute, private productService: ProductService) {}

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      var id: number = +params.get('id');
      this.product$ = this.productService.getProduct(id);
    });
  }

}