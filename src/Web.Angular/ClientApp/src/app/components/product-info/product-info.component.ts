import { Component, OnInit, Input } from '@angular/core';
import { Product } from '../../product';

@Component({
  selector: 'app-product-info',
  templateUrl: './product-info.component.html',
  styleUrls: ['./product-info.component.scss']
})
export class ProductInfoComponent implements OnInit {

  public constructor() { }

  public ngOnInit(): void {
  }

  @Input() product: Product;

}
