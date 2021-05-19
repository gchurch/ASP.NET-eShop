import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from '../../services/product.service';
import { Observable, of } from 'rxjs';
import { Product } from '../../product';
import { catchError, mergeMap, tap } from 'rxjs/operators';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { BasketService } from '../../services/basket.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss']
})
export class ProductComponent implements OnInit {

  public product$: Observable<Product>;
  public productUpdateForm: FormGroup;
  public editingProduct: boolean = false;

  private productNotFound: Product = {
    productId: 0,
    title: "Product not found.",
    description: "",
    seller: "",
    price: 0,
    quantity: 0,
    imageUrl: ""
  };

  public constructor(
    private route: ActivatedRoute, 
    private productService: ProductService, 
    private basketService: BasketService, 
    private router: Router
  ) {}

  public ngOnInit(): void {
    this.createProductObservable();
    this.createProductUpdateForm();
  }

  private createProductObservable(): void {
    this.product$ = this.route.paramMap
    .pipe(mergeMap(params => this.productService.getProductById(+params.get('id'))))
    .pipe(catchError(err => of(this.productNotFound)))
    .pipe(tap(product => this.productUpdateForm.patchValue(product)));
  }

  private createProductUpdateForm(): void {
    this.productUpdateForm = new FormGroup({
      title: new FormControl('', Validators.required),
      description: new FormControl('', Validators.required),
      seller: new FormControl('', Validators.required),
      price: new FormControl([Validators.required, Validators.min(0.01)]),
      quantity: new FormControl([Validators.required, Validators.min(1)])
    });
  }

  public onDelete(id: number) : void {
    console.log("Deleting product " + id);
    this.productService.deleteProductById(id).subscribe({
      next(response) {
        console.log(response);
        this.router.navigate(['/products']);
      },
      error(msg) {
        console.log(msg);
      }
    });
  }

  public onUpdate(id: number): void {
    var product = this.createProductObjectFromFormData(id);
    console.log(product);
    this.updateProduct(product);
    this.editingProduct = false;
  }

  private createProductObjectFromFormData(id: number): Product {
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

  private updateProduct(product: Product): void {
    this.product$ = this.productService.updateProduct(product)
        .pipe(catchError(err => of(this.productNotFound)))
        .pipe(tap(product => this.productUpdateForm.patchValue(product)));
    console.log("Product updated.");
  }

  private cloneProduct(originalProduct: Product): Product {
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

  public addProductToBasket(product: Product): void {
    console.log("Adding product '" + product.title + "' to the basket.");
    this.basketService.addProduct(product.productId);
  }

  public toggleEdit(): void {
    this.editingProduct = !this.editingProduct;
  }
}