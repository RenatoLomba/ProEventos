import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-events',
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.scss']
})
export class EventsComponent implements OnInit {

  public events: any;

  constructor(private http: HttpClient) { }

  // ngOnInit é chamado sempre antes do HTML ser escrito
  ngOnInit(): void {
    this.getEvents();
  }

  public getEvents(): void {
    this.http.get('https://localhost:5001/api/events').subscribe(
      (response) => this.events = response,
      (error) => console.log(error)
    );
  }

}
