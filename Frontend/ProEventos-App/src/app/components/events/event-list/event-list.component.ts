import { Component, OnDestroy, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

import { EventService } from '../../../services/event.service';
import { Event } from '../../../models/Event';
import { Router } from '@angular/router';
import { environment } from '../../../../environments/environment';
import { Pagination } from 'src/app/models/Pagination/Pagination';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';
import { Subject, Subscription } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-event-list',
  templateUrl: './event-list.component.html',
  styleUrls: ['./event-list.component.scss'],
})
export class EventListComponent implements OnInit, OnDestroy {
  events: Event[] = [];

  widthImg = 150;
  marginImg = 2;
  showImg = true;
  modalRef?: BsModalRef;
  eventId?: number;
  pagination = {} as Pagination;
  filterParam = '';

  private _getEventsSubscription: Subscription | null = null;
  private _filterParamChanged = new Subject<string>();

  constructor(
    private readonly eventService: EventService,
    private readonly modalService: BsModalService,
    private readonly toastrService: ToastrService,
    private readonly spinner: NgxSpinnerService,
    private readonly router: Router,
  ) {}

  public ngOnInit(): void {
    this.pagination = new Pagination(1, 3, 1);
    this.getEvents();
  }

  public ngOnDestroy(): void {
    this._getEventsSubscription?.unsubscribe();
  }

  public getEvents(filterParam?: string): void {
    this._getEventsSubscription?.unsubscribe();
    this.spinner.show();

    const { currentPage, itemsPerPage } = this.pagination;

    this._getEventsSubscription = this.eventService
      .getEvents(currentPage, itemsPerPage, filterParam)
      .subscribe(
        (res) => {
          this.events = res.result.map((ev) => {
            const imageUri = ev.imageUri
              ? `${environment.apiUrl}/resources/images/${ev.imageUri}`
              : 'assets/no-image.png';

            return {
              ...ev,
              imageUri,
            };
          });

          this.pagination = res.pagination || this.pagination;
        },
        (error) => {
          console.log(error);
          this.toastrService.error('Erro ao carregar eventos.', 'Erro');
        },
      )
      .add(() => this.spinner.hide());
  }

  detailEvent(id: number) {
    this.router.navigate([`/events/detail/${id}`]);
  }

  public filterEvents() {
    if (this._filterParamChanged.observers.length === 0) {
      this._filterParamChanged
        .pipe(debounceTime(1000))
        .subscribe((filterBy) => {
          this.getEvents(filterBy);
        });
    }

    this._filterParamChanged.next(this.filterParam);
  }

  public openModal(
    event: any,
    template: TemplateRef<any>,
    eventId: number,
  ): void {
    event.stopPropagation();
    this.eventId = eventId;
    this.modalRef = this.modalService.show(template, { class: 'modal-md' });
  }

  public pageChanged(event: PageChangedEvent): void {
    this.pagination.currentPage = event.page;
    this.pagination.itemsPerPage = event.itemsPerPage;
    this.getEvents(this.filterParam);
  }

  public confirm(): void {
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

  public decline(): void {
    this.modalRef?.hide();
  }
}
