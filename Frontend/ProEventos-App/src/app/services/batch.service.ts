import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { take } from 'rxjs/operators';

import { Batch } from '../models/Batch';
import { Constants } from '../utils/constants';

@Injectable()
export class BatchService {
  private _baseUrl = `${Constants.API_URL}/batches`;

  constructor(private http: HttpClient) {}

  getBatchesByEventId(eventId: number) {
    return this.http.get<Batch[]>(`${this._baseUrl}/${eventId}`).pipe(take(1));
  }

  saveBatch(eventId: number, batches: Batch[]) {
    console.log(batches);
    return this.http
      .put<Batch[]>(`${this._baseUrl}/${eventId}`, batches)
      .pipe(take(1));
  }

  deleteBatch(eventId: number, batchId: number) {
    return this.http
      .delete<boolean>(`${this._baseUrl}/${eventId}/${batchId}`)
      .pipe(take(1));
  }
}
