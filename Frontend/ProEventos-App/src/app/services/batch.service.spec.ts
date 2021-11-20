/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { BatchService } from './batch.service';

describe('Service: Batch', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [BatchService]
    });
  });

  it('should ...', inject([BatchService], (service: BatchService) => {
    expect(service).toBeTruthy();
  }));
});
