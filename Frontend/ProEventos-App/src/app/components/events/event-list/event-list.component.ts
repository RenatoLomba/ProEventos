import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

import { EventService } from '../../../services/event.service';
import { Event } from '../../../models/Event';
import { Router } from '@angular/router';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-event-list',
  templateUrl: './event-list.component.html',
  styleUrls: ['./event-list.component.scss'],
})
export class EventListComponent implements OnInit {
  private _events: Event[] = [];
  private _eventsFilter = '';

  filteredEvents = this._events;
  widthImg = 150;
  marginImg = 2;
  showImg = true;
  modalRef?: BsModalRef;
  eventId?: number;

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
    private readonly spinner: NgxSpinnerService,
    private readonly router: Router,
  ) {}

  ngOnInit(): void {
    this.getEvents();
  }

  getEvents(): void {
    this.spinner.show();
    this.eventService.getEvents().subscribe({
      next: (events) => {
        this._events = events.map((ev) => {
          const imageUri = ev.imageUri
            ? `${environment.apiUrl}/resources/images/${ev.imageUri}`
            : 'assets/no-image.png';

          return {
            ...ev,
            imageUri,
          };
        });
        console.log(this._events);

        this.filteredEvents = this._events;
      },
      error: (error) => {
        console.log(error);
        this.spinner.hide();
        this.toastrService.error('Erro ao carregar eventos.', 'Erro');
      },
      complete: () => this.spinner.hide(),
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

  detailEvent(id: number) {
    this.router.navigate([`/events/detail/${id}`]);
  }

  openModal(event: any, template: TemplateRef<any>, eventId: number): void {
    event.stopPropagation();
    this.eventId = eventId;
    this.modalRef = this.modalService.show(template, { class: 'modal-md' });
  }

  confirm(): void {
    this.modalRef?.hide();
    this.spinner.show();

    this.eventService
      .deleteEvent(this.eventId as number)
      .subscribe(
        (res) => {
          if (res) {
            this.toastrService.success(
              `O Evento ${this.eventId} foi deletado.`,
              'Sucesso!',
            );
            this.getEvents();
          }
        },
        (error: any) => {
          console.log(error);
          this.toastrService.error(`Detalhes: ${error.error}`, 'Erro!');
        },
      )
      .add(() => this.spinner.hide());
  }

  decline(): void {
    this.modalRef?.hide();
  }
}
