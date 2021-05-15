import { TestBed } from '@angular/core/testing';

import { ProductServiceMock } from './mock.product.service';

describe('ProductServiceMock', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ProductServiceMock = TestBed.get(ProductServiceMock);
    expect(service).toBeTruthy();
  });
});
