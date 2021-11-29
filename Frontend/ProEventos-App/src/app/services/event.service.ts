import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Event } from '../models/Event';
import { take } from 'rxjs/operators';
import { Constants } from '../utils/constants';
import { Observable } from 'rxjs';

@Injectable()
export class EventService {
  private _baseUrl = `${Constants.API_URL}/events`;

  constructor(private http: HttpClient) {}

  getEvents(theme?: string) {
    return this.http
      .get<Event[]>(this._baseUrl + (theme ? `?theme=${theme}` : ''))
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
