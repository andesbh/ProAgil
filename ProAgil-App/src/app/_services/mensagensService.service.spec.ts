/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { MensagensServiceService } from './mensagensService.service';

describe('Service: MensagensService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [MensagensServiceService]
    });
  });

  it('should ...', inject([MensagensServiceService], (service: MensagensServiceService) => {
    expect(service).toBeTruthy();
  }));
});
