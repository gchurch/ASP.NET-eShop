import { Component, OnInit } from '@angular/core';
import { BasketService } from '../basket.service';
import { Product } from '../product';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.css']
})
export class BasketComponent implements OnInit {

  products: Product[]
  totalCost: number;
  numberOfItems: number;

  constructor(private basketService: BasketService) { }

  ngOnInit() {
    this.products = this.basketService.getProducts();
    this.totalCost = this.basketService.getTotalCost();
    this.numberOfItems = this.basketService.getNumberOfItems();
  }

  removeFromBasket(product: Product) : void {
    console.log("Removing product '" + product.title + "' from basket.");
    this.basketService.removeProduct(product);
    this.products = this.basketService.getProducts();
    this.totalCost = this.basketService.getTotalCost();
    this.numberOfItems = this.basketService.getNumberOfItems();
  }

}
