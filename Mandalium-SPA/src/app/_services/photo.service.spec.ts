/* tslint:disable:no-unused-variable */

import { TestBed, inject, waitForAsync } from '@angular/core/testing';
import { PhotoService } from './photo.service';

describe('Service: Photo', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PhotoService]
    });
  });

  it('should ...', inject([PhotoService], (service: PhotoService) => {
    expect(service).toBeTruthy();
  }));
});
