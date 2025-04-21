import { TestBed } from '@angular/core/testing';

import { OpenBankingService } from './open-banking.service';

describe('OpenBankingService', () => {
  let service: OpenBankingService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OpenBankingService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
