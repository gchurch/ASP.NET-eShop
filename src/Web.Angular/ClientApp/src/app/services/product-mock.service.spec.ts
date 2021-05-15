import { TestBed } from '@angular/core/testing';

import { ProductMockService } from './product-mock.service';

describe('ProductMockService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ProductMockService = TestBed.get(ProductMockService);
    expect(service).toBeTruthy();
  });
});
