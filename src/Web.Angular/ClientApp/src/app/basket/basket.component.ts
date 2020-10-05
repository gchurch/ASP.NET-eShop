import { Component, OnInit } from '@angular/core';
import { BasketService } from '../basket.service';
import { Product } from '../product';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.css']
})
export class BasketComponent implements OnInit {

  private products$: Subject<Product[]>;
  private totalCost$: Subject<number>;
  private numberOfProducts$: Subject<number>;

  constructor(private basketService: BasketService) { }

  ngOnInit() {
    this.products$ = this.basketService.getProducts();
    this.totalCost$ = this.basketService.getTotalCost();
    this.numberOfProducts$ = this.basketService.getNumberOfProducts();
  }

  removeFromBasket(product: Product) : void {
    console.log("Removing product '" + product.title + "' from basket.");
    this.basketService.removeProduct(product);
  }

  getProducts$(): Subject<Product[]> {
    return this.products$;
  }
  
  getTotalCost$(): Subject<Number> {
    return this.totalCost$;
  }

  getNumberOfProducts$(): Subject<Number> {
    return this.numberOfProducts$;
  }
}
