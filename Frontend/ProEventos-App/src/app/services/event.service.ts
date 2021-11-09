import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Event } from '../models/Event';

@Injectable()
export class EventService {
  private _baseUrl = 'https://localhost:5001/api/events';

  constructor(private http: HttpClient) {}

  getEvents(theme?: string) {
    return this.http.get<Event[]>(
      this._baseUrl + (theme ? `?theme=${theme}` : ''),
    );
  }

  getEventById(id: number) {
    return this.http.get<Event>(`${this._baseUrl}/${id}`);
  }
}
