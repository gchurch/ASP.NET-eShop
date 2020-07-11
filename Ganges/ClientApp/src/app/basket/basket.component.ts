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

  constructor(private basketService: BasketService) { }

  ngOnInit() {
    this.products = this.basketService.getProducts();
    this.totalCost = this.basketService.calculateCost();
  }

  removeFromBasket(product: Product) : void {
    console.log("Removing product '" + product.title + "' from basket.");
    this.basketService.removeProduct(product);
    this.products = this.basketService.getProducts();
    this.totalCost = this.basketService.calculateCost();
  }

}
