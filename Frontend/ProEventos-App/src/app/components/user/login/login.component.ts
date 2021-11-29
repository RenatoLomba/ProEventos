import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { UserLogin } from 'src/app/models/Identity/UserLogin';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  model: UserLogin = { userName: '', password: '' };

  constructor(
    private readonly accountService: AccountService,
    private readonly router: Router,
    private readonly toastr: ToastrService,
    private readonly spinner: NgxSpinnerService,
  ) {}

  ngOnInit() {}

  public login(): void {
    this.spinner.show();

    this.accountService
      .login(this.model)
      .subscribe(
        () => {
          this.router.navigateByUrl('/dashboard');
        },
        (error) => {
          if (error.status === 401) {
            this.toastr.error(error.error, 'Unauthorized!');
          } else {
            console.log(error);
          }
        },
      )
      .add(() => this.spinner.hide());
  }
}
