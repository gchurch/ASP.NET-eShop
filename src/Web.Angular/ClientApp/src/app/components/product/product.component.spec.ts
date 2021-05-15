import { HttpClient, HttpClientModule } from '@angular/common/http';
import { InjectionToken, NO_ERRORS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ProductMockService } from 'src/app/services/product-mock.service';
import { ProductService } from 'src/app/services/product.service';
import { ProductInfoComponent } from '../product-info/product-info.component';

import { ProductComponent } from './product.component';

export const BASE_URL = new InjectionToken<string>('BASE_URL');

describe('ProductComponent', () => {
  let component: ProductComponent;
  let fixture: ComponentFixture<ProductComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ 
        ProductComponent,
        ProductInfoComponent
      ],
      imports: [
        FormsModule,
        ReactiveFormsModule,
        RouterModule.forRoot([]),
        HttpClientModule
      ],
      providers: [
        { provide: ProductService, useClass: ProductMockService }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
