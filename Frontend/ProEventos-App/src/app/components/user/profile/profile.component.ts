import { Component, OnInit } from '@angular/core';
import {
  AbstractControlOptions,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ValidatorField } from 'src/app/helpers/validator-field';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit {
  form!: FormGroup;

  get controls(): any {
    return this.form?.controls;
  }

  constructor(private readonly formBuilder: FormBuilder) {}

  ngOnInit() {
    this._validations();
  }

  private _validations() {
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.mustMatch('password', 'confirmPassword'),
    };

    this.form = this.formBuilder.group(
      {
        title: ['', Validators.required],
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        phone: ['', Validators.required],
        function: ['', Validators.required],
        description: ['', Validators.required],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', Validators.required],
      },
      formOptions,
    );
  }

  onSubmit() {
    if (this.form?.invalid) {
      return;
    }
  }

  resetForm(event: any) {
    event.preventDefault();
    this.form?.reset();
  }

  cssValidator(control: FormControl) {
    return { 'is-invalid': control?.errors && control?.touched };
  }
}
