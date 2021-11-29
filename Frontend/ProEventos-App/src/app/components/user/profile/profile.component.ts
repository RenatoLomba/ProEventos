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
import { UserUpdate } from 'src/app/models/Identity/UserUpdate';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit {
  userUpdate = {} as UserUpdate;
  form!: FormGroup;
  titles: string[] = [];
  functions: string[] = [];

  get controls(): any {
    return this.form?.controls;
  }

  get fullName(): string {
    const firstName = this.controls.firstName.value;
    const lastName = this.controls.lastName.value;
    return `${firstName} ${lastName}`;
  }

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly accountService: AccountService,
    private readonly toastr: ToastrService,
    private readonly router: Router,
    private readonly spinner: NgxSpinnerService,
  ) {}

  ngOnInit() {
    this._validations();
    this._loadTitlesList();
    this._loadFunctionsList();
    this._loadUser();
  }

  private _loadTitlesList(): void {
    this.accountService.getTitles().subscribe(
      (titles) => (this.titles = titles),
      (error: any) => this.toastr.error(`Details: ${error.error}`, 'Error!'),
    );
  }

  private _loadFunctionsList(): void {
    this.accountService.getFunctions().subscribe(
      (functions) => (this.functions = functions),
      (error: any) => this.toastr.error(`Details: ${error.error}`, 'Error!'),
    );
  }

  private _loadUser(): void {
    this.spinner.show();

    this.accountService
      .getUser()
      .subscribe(
        (user) => {
          this.userUpdate = user;
          this.form.patchValue(this.userUpdate);
        },
        (error: any) => {
          this.toastr.error(`Details: ${error.error}`, 'Error!');
          this.router.navigate(['/dashboard']);
        },
      )
      .add(() => this.spinner.hide());
  }

  private _validations() {
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.mustMatch('password', 'confirmPassword'),
    };

    this.form = this.formBuilder.group(
      {
        userName: [''],
        title: ['NaoInformado', Validators.required],
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        phoneNumber: ['', Validators.required],
        function: ['NaoInformado', Validators.required],
        description: ['', Validators.required],
        password: ['', [Validators.minLength(6), Validators.nullValidator]],
        confirmPassword: ['', Validators.nullValidator],
      },
      formOptions,
    );
  }

  onSubmit(): void {
    if (this.form?.invalid) {
      return;
    }

    this._updateUser();
  }

  private _updateUser(): void {
    this.userUpdate = {
      ...this.form.value,
    };
    this.spinner.show();
    this.accountService
      .updateUser(this.userUpdate)
      .subscribe(
        () => {
          this.toastr.success('Usuário atualizado', 'Sucesso!');
          // this.userUpdate = user;
          // this.form.patchValue(this.userUpdate);
        },
        (error: any) => {
          this.toastr.error(`Details: ${error.error}`, 'Error!');
        },
      )
      .add(() => this.spinner.hide());
  }

  resetForm(event: any) {
    event.preventDefault();
    this.form?.reset();
  }

  cssValidator(control: FormControl) {
    return { 'is-invalid': control?.errors && control?.touched };
  }
}
