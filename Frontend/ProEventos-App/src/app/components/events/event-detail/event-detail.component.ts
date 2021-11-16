import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

import { Event } from '../../../models/Event';
import { EventService } from '../../../services/event.service';

@Component({
  selector: 'app-event-detail',
  templateUrl: './event-detail.component.html',
  styleUrls: ['./event-detail.component.scss'],
})
export class EventDetailComponent implements OnInit {
  form!: FormGroup;
  event?: Event;

  private _saveStateMode: 'post' | 'put' = 'post';
  private _bsConfig = {
    isAnimated: true,
    adaptivePosition: true,
    dateInputFormat: 'DD/MM/YYYY hh:mm A',
    containerClass: 'theme-default',
    showWeekNumbers: false,
    locale: 'pt-br',
  };

  get controls(): any {
    return this.form.controls;
  }

  get bsConfig() {
    return this._bsConfig;
  }

  constructor(
    private formBuilder: FormBuilder,
    private router: ActivatedRoute,
    private eventService: EventService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
  ) {}

  ngOnInit() {
    this._loadEvent();
    this._validation();
  }

  private _validation() {
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
    });
  }

  resetForm(event: any) {
    event.preventDefault();
    this.form.reset();
  }

  cssValidator(control: FormControl) {
    return {
      'is-invalid': control?.errors && control?.touched,
    };
  }

  private _loadEvent() {
    const eventIdParam = this.router.snapshot.paramMap.get('id');

    if (!eventIdParam) return;

    this.spinner.show();

    this._saveStateMode = 'put';

    this.eventService.getEventById(+eventIdParam).subscribe({
      next: (event) => {
        this.event = { ...event };
        this.form.patchValue(this.event);
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error(`Detalhes: ${error.message}`, 'Erro');
        console.log(error);
      },
      complete: () => this.spinner.hide(),
    });
  }

  saveChanges() {
    this.spinner.show();

    if (!this.form.valid) return;

    this.event =
      this._saveStateMode === 'post'
        ? { ...this.form.value }
        : { ...this.form.value, id: this.event?.id };

    this.eventService[this._saveStateMode](this.event as Event).subscribe({
      next: (event) => {
        this.toastr.success(`Evento ${event.id} salvo.`, 'Sucesso!');
        this.spinner.hide();
      },
      error: (error: any) => {
        console.log(error);
        this.toastr.error(`Detalhes: ${error.message}`, 'Erro!');
        this.spinner.hide();
      },
      complete: () => this.spinner.hide(),
    });
  }
}
