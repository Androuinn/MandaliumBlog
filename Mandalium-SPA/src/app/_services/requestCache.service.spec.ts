/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { RequestCacheService } from './requestCache.service';

describe('Service: RequestCache', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RequestCacheService]
    });
  });

  it('should ...', inject([RequestCacheService], (service: RequestCacheService) => {
    expect(service).toBeTruthy();
  }));
});
