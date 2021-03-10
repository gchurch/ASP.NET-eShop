import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from '../../services/product.service';
import { Observable, of } from 'rxjs';
import { Product } from '../../product';
import { catchError, tap } from 'rxjs/operators';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { BasketService } from '../../services/basket.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss']
})
export class ProductComponent implements OnInit {

  product$: Observable<Product>;
  productQuantity: number;
  productUpdateForm: FormGroup;
  editingProduct: boolean = false;

  productNotFound: Product = {
    productId: 0,
    title: "Product not found.",
    description: "",
    seller: "",
    price: 0,
    quantity: 0,
    imageUrl: ""
  };

  constructor(
    private route: ActivatedRoute, 
    private productService: ProductService, 
    private basketService: BasketService, 
    private router: Router
  ) {}

  ngOnInit() {
    this.createProductObservable();
    this.createProductUpdateForm();
  }

  createProductObservable(): void {
    this.route.paramMap.subscribe(params => {
      var id: number = +params.get('id');
      this.product$ = this.productService.getProduct(id)
        .pipe(catchError(err => of(this.productNotFound)))
        .pipe(tap(product => this.productUpdateForm.patchValue(product)));
    });
  }

  createProductUpdateForm(): void {
    this.productUpdateForm = new FormGroup({
      title: new FormControl('', Validators.required),
      description: new FormControl('', Validators.required),
      seller: new FormControl('', Validators.required),
      price: new FormControl([Validators.required, Validators.min(0.01)]),
      quantity: new FormControl([Validators.required, Validators.min(1)])
    });
  }

  onDelete(id: number) : void {
    console.log("Deleting product " + id);
    this.productService.deleteProduct(id).subscribe(output => {
      console.log(output);
      this.router.navigate(['/products']);
    });
  }

  onUpdate(id: number): void {
    var product = this.createProductObjectFromFormData(id);
    console.log(product);
    this.updateProduct(product);
    this.editingProduct = false;
  }

  createProductObjectFromFormData(id: number): Product {
    var product: Product = {
      productId: id,
      title: this.productUpdateForm.value.title,
      description: this.productUpdateForm.value.description,
      seller: this.productUpdateForm.value.seller,
      price: parseInt(this.productUpdateForm.value.price),
      quantity: parseInt(this.productUpdateForm.value.quantity),
      imageUrl: ""
    };
    return product;
  }

  updateProduct(product: Product): void {
    this.product$ = this.productService.updateProduct(product)
        .pipe(catchError(err => of(this.productNotFound)))
        .pipe(tap(product => this.productUpdateForm.patchValue(product)));
    console.log("Product updated.");
  }

  cloneProduct(originalProduct: Product): Product {
    var clonedProduct: Product = {
      productId: originalProduct.productId,
      title: originalProduct.title,
      description: originalProduct.description,
      seller: originalProduct.seller,
      price: originalProduct.price,
      quantity: originalProduct.quantity,
      imageUrl: originalProduct.imageUrl
    };
    return clonedProduct;
  }

  addProductToBasket(product: Product) {
    console.log("Adding product '" + product.title + "' to the basket.");
    this.basketService.addProduct(this.cloneProduct(product));
  }

  toggleEdit() {
    this.editingProduct = !this.editingProduct;
  }
}