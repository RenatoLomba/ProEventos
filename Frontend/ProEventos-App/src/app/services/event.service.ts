import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Event } from '../models/Event';
import { map, take } from 'rxjs/operators';
import { Constants } from '../utils/constants';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../models/Pagination/PaginatedResult';
import { Pagination } from '../models/Pagination/Pagination';

@Injectable()
export class EventService {
  private _baseUrl = `${Constants.API_URL}/events`;

  constructor(private http: HttpClient) {}

  getEvents(
    page?: number,
    itemsPerPage?: number,
    term?: string,
  ): Observable<PaginatedResult<Event[]>> {
    let params = new HttpParams();

    if (page && itemsPerPage) {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    if (term) {
      params = params.append('term', term);
    }

    return this.http
      .get<Event[]>(this._baseUrl, { observe: 'response', params })
      .pipe(
        take(1),
        map((response) => {
          const result = response.body || [];
          const paginatedResult = new PaginatedResult(result);

          if (response.headers.has('pagination')) {
            const pagination: Pagination = JSON.parse(
              response.headers.get('pagination') as string,
            );

            paginatedResult.pagination = pagination;
          }

          return paginatedResult;
        }),
      );
  }

  getEventsByTheme(theme: string) {
    return this.http
      .get<Event[]>(`${this._baseUrl}?term=${theme}`)
      .pipe(take(1));
  }

  getEventById(id: number) {
    return this.http.get<Event>(`${this._baseUrl}/${id}`).pipe(take(1));
  }

  post(event: Event) {
    return this.http.post<Event>(this._baseUrl, event).pipe(take(1));
  }

  put(event: Event) {
    return this.http
      .put<Event>(`${this._baseUrl}/${event.id}`, event)
      .pipe(take(1));
  }

  deleteEvent(id: number) {
    return this.http.delete<boolean>(`${this._baseUrl}/${id}`).pipe(take(1));
  }

  uploadImage(eventId: number, file: File): Observable<Event> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http
      .post<Event>(`${this._baseUrl}/upload-image/${eventId}`, formData)
      .pipe(take(1));
  }
}
