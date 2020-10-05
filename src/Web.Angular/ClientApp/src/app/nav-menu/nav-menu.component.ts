import { Component, OnInit } from '@angular/core';
import { BasketService } from '../basket.service';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  private isExpanded: boolean = false;
  
  private numberOfProductsInBasket$: Subject<number>;

  constructor(private basketService: BasketService) { }

  ngOnInit() : void {
    this.numberOfProductsInBasket$ = this.basketService.getNumberOfProducts();
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  getIsExpanded(): boolean {
    return this.isExpanded;
  }

  getNumberOfProductsInBasket$(): Subject<number> {
    return this.numberOfProductsInBasket$;
  }
}
