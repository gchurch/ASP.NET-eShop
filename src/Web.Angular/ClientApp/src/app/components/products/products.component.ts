import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { Observable } from 'rxjs';
import { Product } from '../../product';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent implements OnInit {

  products$: Observable<Product[]>;

  constructor(private productService: ProductService) {}

  ngOnInit() {
    this.products$ = this.productService.getProducts();
  }

}