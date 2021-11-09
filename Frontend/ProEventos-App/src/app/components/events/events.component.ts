import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { EventService } from '../../services/event.service';
import { Event } from '../../models/Event';

@Component({
  selector: 'app-events',
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.scss'],
})
export class EventsComponent implements OnInit {
  private _events: Event[] = [];
  private _eventsFilter = '';

  filteredEvents = this._events;
  widthImg = 150;
  marginImg = 2;
  showImg = true;
  modalRef?: BsModalRef;

  get eventsFilter() {
    return this._eventsFilter;
  }

  set eventsFilter(value: string) {
    this._eventsFilter = value;
    this.filterEvents(value);
  }

  constructor(
    private readonly eventService: EventService,
    private readonly modalService: BsModalService,
    private readonly toastrService: ToastrService,
    private readonly spinnerService: NgxSpinnerService,
  ) {}

  ngOnInit(): void {
    this.getEvents();
  }

  getEvents(): void {
    this.spinnerService.show();
    this.eventService.getEvents().subscribe({
      next: (events) => {
        this._events = events;
        this.filteredEvents = this._events;
      },
      error: (error) => {
        console.log(error);
        this.spinnerService.hide();
        this.toastrService.error('Erro ao carregar eventos.', 'Erro');
      },
      complete: () => this.spinnerService.hide(),
    });
  }

  filterEvents(param: string): void {
    if (!param) {
      this.filteredEvents = this._events;
      return;
    }

    param = param.toLocaleLowerCase();
    this.filteredEvents = this._events.filter(
      (ev) =>
        ev.theme.toLocaleLowerCase().includes(param) ||
        ev.place.toLocaleLowerCase().includes(param),
    );
  }

  openModal(template: TemplateRef<any>): void {
    this.modalRef = this.modalService.show(template, { class: 'modal-md' });
  }

  confirm(): void {
    this.modalRef?.hide();
    this.toastrService.success('O Evento foi deletado.', 'Sucesso!');
  }

  decline(): void {
    this.modalRef?.hide();
  }
}
