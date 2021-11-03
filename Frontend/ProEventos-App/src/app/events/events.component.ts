import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

type Event = {
  batch: string;
  eventDate: string;
  eventId: number;
  imageUri: string;
  peopleQty: number;
  place: string;
  theme: string;
}

@Component({
  selector: 'app-events',
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.scss']
})
export class EventsComponent implements OnInit {
  private _events: Event[] = [];
  private _eventsFilter = '';

  filteredEvents: Event[] = this._events;
  widthImg = 150;
  marginImg = 2;
  showImg = true;

  get eventsFilter() {
    return this._eventsFilter;
  }

  set eventsFilter(value: string) {
    this._eventsFilter = value;
    this._filterEvents(value);
  }

  constructor(private http: HttpClient) { }

  // ngOnInit é chamado sempre antes do HTML ser escrito
  ngOnInit(): void {
    this.getEvents();
  }

  getEvents(): void {
    this.http.get<Event[]>('https://localhost:5001/api/events').subscribe(
      (response) => {
        this._events = response;
        this.filteredEvents = this._events;
      },
      (error) => console.log(error)
    );
  }

  private _filterEvents(param: string): void {
    if (!param) {
      this.filteredEvents = this._events;
      return;
    }

    param = param.toLocaleLowerCase();
    this.filteredEvents = this._events.filter(
      (ev) => ev.theme.toLocaleLowerCase().includes(param) ||
        ev.place.toLocaleLowerCase().includes(param)
    );
  }

}
