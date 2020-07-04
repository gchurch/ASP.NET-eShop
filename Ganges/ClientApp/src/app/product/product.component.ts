import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from '../product.service';
import { Observable, of } from 'rxjs';
import { Product } from '../product';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {

  // The observable for retreiving the product from the server
  product$: Observable<Product>;
  productQuantity: number;

  constructor(private route: ActivatedRoute, private productService: ProductService) {}

  ngOnInit() {
    // This product is used is the requested product does not exist.
    var productNotFound: Product = {
      id: 0,
      title: "Product not found.",
      description: "",
      seller: "",
      price: 0,
      quantity: 0,
      imageUrl: ""
    }
    // Create the observable for retrieving the product from the server
    this.route.paramMap.subscribe(params => {
      var id: number = +params.get('id');
      this.product$ = this.productService.getProduct(id).pipe(catchError(err => of(productNotFound)));
    });
  }

  onBuyNow(id: number) {
    this.productService.buyProduct(id).subscribe(output => {
      this.productQuantity = output
      console.log("Updated product quantity to: " + output);
    });
  }

}