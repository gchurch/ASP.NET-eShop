import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';

import { BasketService } from './basket.service';
import { BasketServiceMock } from './mocks/mock.basket.service';
import { ProductServiceMock } from './mocks/mock.product.service';
import { ProductService } from './product.service';

describe('BasketService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      { provide: ProductService, useClass: ProductServiceMock }
    ]
  }));

  it('should be created', () => {
    const service: BasketService = TestBed.inject(BasketService);
    expect(service).toBeTruthy();
  });

  it('even number should return true', () => {
    const service: BasketService = TestBed.inject(BasketService);
    expect(service.isEven(2)).toBeTruthy();
  })

  it('odd number should return false', () => {
    const service: BasketService = TestBed.inject(BasketService);
    expect(service.isEven(3)).toBeFalsy();
  })
});
