import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from '../product.service';
import { Observable, of } from 'rxjs';
import { Product } from '../product';
import { catchError, tap } from 'rxjs/operators';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { BasketService } from '../basket.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {

  // The observable for retreiving the product from the server
  product$: Observable<Product>;

  productQuantity: number;

  productForm: FormGroup;

  constructor(private route: ActivatedRoute, private productService: ProductService, private basketService: BasketService) {}

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
      this.product$ = this.productService.getProduct(id)
        .pipe(catchError(err => of(productNotFound)))
        .pipe(tap(product => this.productForm.patchValue(product)));
    });
    
    // Create the product form. Here I am subscribing to the product observable in order to give initial values
    // to the form. I am not sure if this is good practice.
    this.productForm = new FormGroup({
      title: new FormControl('', Validators.required),
      description: new FormControl('', Validators.required),
      seller: new FormControl('', Validators.required),
      price: new FormControl([Validators.required, Validators.min(0.01)]),
      quantity: new FormControl([Validators.required, Validators.min(1)])
    });
  }

  onBuyNow(id: number) : void {
    this.productService.buyProduct(id).subscribe(output => {
      this.productQuantity = output
      console.log("Updated product quantity to: " + output);
    });
  }

  onDelete(id: number) : void {
    console.log("Deleting product " + id);
    this.productService.deleteProduct(id).subscribe(output => {
      console.log(output)
    });
  }

  onSubmit(id: number): void {

    // Create a Product object from the form data.
    var product: Product = {
      id: id,
      title: this.productForm.value.title,
      description: this.productForm.value.description,
      seller: this.productForm.value.seller,
      price: parseInt(this.productForm.value.price),
      quantity: parseInt(this.productForm.value.quantity),
      imageUrl: ""
    };

    console.log(product);

    // Send the updated product to server
    console.log("Sending updated product to the server.");
    this.productService.updateProduct(product).subscribe(
      output => {
        console.log(output);
      }
    );
  }

  addToBasket(product: Product) {
    console.log("Adding product '" + product.title + "' to the basket.");
    this.basketService.addProduct(product);
  }

}