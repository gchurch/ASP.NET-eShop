import { Component, OnInit } from '@angular/core';
import { BasketService } from '../../services/basket.service';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent implements OnInit {

  private isExpanded: boolean = false;
  private numberOfProductsInBasket$: Subject<number>;

  constructor(private basketService: BasketService) { }

  public ngOnInit(): void {
    this.numberOfProductsInBasket$ = this.basketService.getNumberOfProducts$();
  }

  public collapse(): void {
    this.isExpanded = false;
  }

  public toggle(): void {
    this.isExpanded = !this.isExpanded;
  }

  public getIsExpanded(): boolean {
    return this.isExpanded;
  }

  public getNumberOfProductsInBasket$(): Subject<number> {
    return this.numberOfProductsInBasket$;
  }
}
