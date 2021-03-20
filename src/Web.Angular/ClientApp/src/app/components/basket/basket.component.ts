import { Component, OnInit } from '@angular/core';
import { BasketService } from '../../services/basket.service';
import { Product } from '../../product';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.scss']
})
export class BasketComponent implements OnInit {

  private products$: Subject<Product[]>;
  private totalCost$: Subject<number>;
  private numberOfProducts$: Subject<number>;

  public constructor(private basketService: BasketService) { }

  public ngOnInit(): void {
    this.products$ = this.basketService.getProducts$();
    this.totalCost$ = this.basketService.getTotalCost$();
    this.numberOfProducts$ = this.basketService.getNumberOfProducts$();
  }

  public removeFromBasket(product: Product) : void {
    console.log("Removing product '" + product.title + "' from basket.");
    this.basketService.removeProduct(product.productId);
  }

  public getProducts$(): Subject<Product[]> {
    return this.products$;
  }
  
  public getTotalCost$(): Subject<Number> {
    return this.totalCost$;
  }

  public getNumberOfProducts$(): Subject<Number> {
    return this.numberOfProducts$;
  }
}
