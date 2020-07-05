import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { Product } from '../product'

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
    price: new FormControl(''),
    quantity: new FormControl(''),
    imageUrl: new FormControl(''),
  });

  constructor() { }

  ngOnInit() {
  }

  onSubmit() {
    // Create a product object from the values in the form
    var product: Product = {
      id: 0,
      title: this.productForm.value.title,
      description: this.productForm.value.description,
      seller: this.productForm.value.seller,
      price: this.productForm.value.price,
      quantity: this.productForm.value.quantity,
      imageUrl: this.productForm.value.imageUrl
    }
    console.log(product);
  }

}
