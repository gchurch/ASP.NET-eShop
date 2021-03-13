import { Component, Input, OnInit } from '@angular/core';
import { Product } from '../../product';

@Component({
  selector: 'app-product-update',
  templateUrl: './product-update.component.html',
  styleUrls: ['./product-update.component.scss']
})
export class ProductUpdateComponent implements OnInit {

  public constructor() { }

  public ngOnInit(): void {
  }

  @Input() product: Product;

}
