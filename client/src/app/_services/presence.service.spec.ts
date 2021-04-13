/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { PresenceService } from './presence.service';

describe('Service: Presence', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PresenceService]
    });
  });

  it('should ...', inject([PresenceService], (service: PresenceService) => {
    expect(service).toBeTruthy();
  }));
});
