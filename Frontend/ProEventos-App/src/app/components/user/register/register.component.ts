import { Component, OnInit } from '@angular/core';
import {
  AbstractControlOptions,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { ValidatorField } from 'src/app/helpers/validator-field';
import { User } from 'src/app/models/Identity/User';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  user = {} as User;
  form!: FormGroup;

  get controls(): any {
    return this.form.controls;
  }

  constructor(
    private readonly router: Router,
    private readonly formBuilder: FormBuilder,
    private readonly accountService: AccountService,
    private readonly toastr: ToastrService,
    private readonly spinner: NgxSpinnerService,
  ) {}

  ngOnInit() {
    this._validation();
  }

  private _validation() {
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.mustMatch('password', 'confirmPassword'),
    };

    this.form = this.formBuilder.group(
      {
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        userName: ['', Validators.required],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', Validators.required],
      },
      formOptions,
    );
  }

  cssValidator(control: FormControl) {
    return { 'is-invalid': control?.errors && control?.touched };
  }

  public register(): void {
    this.spinner.show();

    this.user = { ...this.form.value };
    this.accountService
      .register(this.user)
      .subscribe(
        () => this.router.navigateByUrl('/dashboard'),
        (error: any) =>
          this.toastr.error(`Details: ${error.error}`, 'Error!'),
      )
      .add(() => this.spinner.hide());
  }
}
