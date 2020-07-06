import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { Product } from '../product'
import { ProductService } from '../product.service';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent implements OnInit {

  //https://angular.io/guide/reactive-forms
  productForm = new FormGroup({
    title: new FormControl(''),
    description: new FormControl(''),
    seller: new FormControl(''),
    price: new FormControl(0),
    quantity: new FormControl(0),
    imageUrl: new FormControl(''),
  });

  // Injecting the ProductService dependency. The property for the parameter is created for us.
  constructor(private productService: ProductService) { }

  ngOnInit() {
  }

  onSubmit() {
    // Create a product object from the values in the form.
    var product: Product = {
      id: 0,
      title: this.productForm.value.title,
      description: this.productForm.value.description,
      seller: this.productForm.value.seller,
      price: parseInt(this.productForm.value.price),
      quantity: parseInt(this.productForm.value.quantity),
      imageUrl: this.productForm.value.imageUrl
    }
    // Print the product.
    console.log(product);
    // Send the product to the server.
    this.productService.addProduct(product).subscribe(
      output => console.log(output)
    );
  }

}
