import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-event-detail',
  templateUrl: './event-detail.component.html',
  styleUrls: ['./event-detail.component.scss'],
})
export class EventDetailComponent implements OnInit {
  form!: FormGroup;

  get controls(): any {
    return this.form.controls;
  }

  constructor(private readonly formBuilder: FormBuilder) {}

  ngOnInit() {
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
}
