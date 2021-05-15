import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';

import { BasketService } from './basket.service';
import { ProductServiceMock } from './mocks/mock.product.service';
import { ProductService } from './product.service';

describe('BasketService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      { provide: ProductService, useClass: ProductServiceMock }
    ]
  }));

  it('should be created', () => {
    const service: BasketService = TestBed.get(BasketService);
    expect(service).toBeTruthy();
  });
});
