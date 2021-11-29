import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Constants } from '../utils/constants';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(
    private readonly router: Router,
    private readonly toastr: ToastrService,
  ) {}

  canActivate(): boolean {
    if (localStorage.getItem(Constants.USER_STORE)) {
      return true;
    }

    this.router.navigate(['/user/login']);
    this.toastr.info('Usuário não autenticado!');
    return false;
  }
}
