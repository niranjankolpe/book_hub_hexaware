import { TestBed } from '@angular/core/testing';

import { GetAllGenreService } from './get-all-genre.service';

describe('GetAllGenreService', () => {
  let service: GetAllGenreService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GetAllGenreService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
