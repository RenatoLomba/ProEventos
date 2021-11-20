import { Component, OnInit, TemplateRef } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

import { Event } from '../../../models/Event';
import { Batch } from '../../../models/Batch';
import { EventService } from '../../../services/event.service';
import { BatchService } from '../../../services/batch.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-event-detail',
  templateUrl: './event-detail.component.html',
  styleUrls: ['./event-detail.component.scss'],
})
export class EventDetailComponent implements OnInit {
  form!: FormGroup;
  event?: Event;
  eventId?: number;
  modalRef?: BsModalRef;
  batchToDelete = { id: 0, name: '', index: 0 };

  private _saveStateMode: 'post' | 'put' = 'post';
  private _bsConfig = {
    isAnimated: true,
    adaptivePosition: true,
    dateInputFormat: 'DD/MM/YYYY hh:mm A',
    containerClass: 'theme-default',
    showWeekNumbers: false,
    locale: 'pt-br',
  };

  get isEditMode() {
    return this._saveStateMode === 'put';
  }

  get controls(): any {
    return this.form.controls;
  }

  get bsConfig() {
    return this._bsConfig;
  }

  get bsConfigBatch() {
    return { ...this._bsConfig, dateInputFormat: 'DD/MM/YYYY' };
  }

  get batches(): FormArray {
    return this.form.get('batches') as FormArray;
  }

  get hasBatches(): boolean {
    return this.batches.length > 0;
  }

  get isBatchesValid(): boolean {
    return this.batches.valid;
  }

  constructor(
    private formBuilder: FormBuilder,
    private activedRoute: ActivatedRoute,
    private router: Router,
    private eventService: EventService,
    private batchService: BatchService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private modalService: BsModalService,
  ) {}

  ngOnInit(): void {
    this._loadEvent();
    this._validation();
  }

  private _validation(): void {
    this.form = this.formBuilder.group({
      theme: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(50),
        ],
      ],
      place: ['', Validators.required],
      eventDate: ['', Validators.required],
      peopleQty: ['', [Validators.required, Validators.max(120000)]],
      phone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      imageUri: ['', Validators.required],
      batches: this.formBuilder.array([]),
    });
  }

  public addBatch(): void {
    this.batches.push(
      this._createBatch({
        id: 0,
        price: 0,
        startDate: new Date(),
        endDate: new Date(),
      } as Batch),
    );
  }

  private _createBatch(batch: Batch): FormGroup {
    return this.formBuilder.group({
      id: [batch.id],
      name: [batch.name, Validators.required],
      qty: [batch.qty, Validators.required],
      price: [batch.price, Validators.required],
      startDate: [batch.startDate],
      endDate: [batch.endDate],
    });
  }

  public resetForm(event: any): void {
    event.preventDefault();
    this.form.reset();
  }

  public getBatchControl(
    index: number,
    controlName: string,
  ): FormControl | null {
    return this.batches.get(index + `.${controlName}`) as FormControl;
  }

  public cssValidator(control: FormControl | null) {
    return {
      'is-invalid': control?.errors && control?.touched,
    };
  }

  private _loadEvent(): void {
    const eventIdParam = this.activedRoute.snapshot.paramMap.get('id');

    if (!eventIdParam) return;
    this.eventId = +eventIdParam;

    this.spinner.show();

    this._saveStateMode = 'put';

    this.eventService.getEventById(this.eventId).subscribe({
      next: (event) => {
        this.event = { ...event };
        this.form.patchValue(this.event);

        this.event.batches.forEach((bt) =>
          this.batches.push(this._createBatch(bt)),
        );
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error(`Detalhes: ${error.message}`, 'Erro');
        console.log(error);
      },
      complete: () => this.spinner.hide(),
    });
  }

  public saveEvent(): void {
    if (!this.form.valid) return;

    this.spinner.show();

    this.event =
      this._saveStateMode === 'post'
        ? { ...this.form.value }
        : { ...this.form.value, id: this.event?.id };

    this.eventService[this._saveStateMode](this.event as Event)
      .subscribe(
        (event) => {
          this.toastr.success(`Evento ${event.id} salvo.`, 'Sucesso!');

          const reloadRoute = this.isEditMode
            ? this.router.url
            : `${this.router.url}/${event.id}`;

          this.router.navigate([reloadRoute]);
        },
        (error: any) => {
          console.log(error);
          this.toastr.error(`Detalhes: ${error.message}`, 'Erro!');
        },
      )
      .add(() => this.spinner.hide());
  }

  public saveBatches(): void {
    if (!this.isBatchesValid || !this.eventId) return;

    this.spinner.show();

    this.batchService
      .saveBatch(this.eventId, this.batches.value)
      .subscribe(
        () => {
          this.toastr.success('Lotes salvos.', 'Sucesso!');
        },
        (error: any) => {
          console.log(error);
          this.toastr.error(`Detalhes: ${error.message}`, 'Error!');
        },
      )
      .add(() => this.spinner.hide());
  }

  public removeBatch(template: TemplateRef<any>, index: number): void {
    this.batchToDelete.id = this.getBatchControl(index, 'id')?.value;
    this.batchToDelete.name = this.getBatchControl(index, 'name')?.value;
    this.batchToDelete.index = index;

    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  public confirmDeleteBatch(): void {
    if (!this.eventId) return;
    this.modalRef?.hide();

    if (this.batchToDelete.id === 0) {
      this.batches.removeAt(this.batchToDelete.index);
      return;
    }

    this.spinner.show();

    this.batchService
      .deleteBatch(this.eventId, this.batchToDelete.id)
      .subscribe(
        (res) => {
          if (!res) {
            this.toastr.error(`Detalhes: Não informado.`, 'Error!');
            return;
          }
          this.batches.removeAt(this.batchToDelete.index);
          this.toastr.success('Lote deletado.', 'Sucesso!');
        },
        (error: any) => {
          console.log(error);
          this.toastr.error(`Detalhes: ${error.message}`, 'Error!');
        },
      )
      .add(() => this.spinner.hide());
  }

  public declineDeleteBatch(): void {
    this.modalRef?.hide();
  }
}
