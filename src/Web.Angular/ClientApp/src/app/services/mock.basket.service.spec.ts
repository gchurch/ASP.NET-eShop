import { TestBed } from '@angular/core/testing';

import { BasketServiceMock } from './mock.basket.service';

describe('BasketServiceMock', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BasketServiceMock = TestBed.get(BasketServiceMock);
    expect(service).toBeTruthy();
  });
});
