import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { User } from '../../../app/models/Identity/User';
import { AccountService } from '../../../app/services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss'],
})
export class NavComponent implements OnInit {
  public isCollapsed = true;

  constructor(
    private readonly router: Router,
    private readonly accountService: AccountService,
  ) {}

  get currentUser(): Observable<User | null> {
    return this.accountService.currentUser$;
  }

  ngOnInit() {}

  showMenu(): boolean {
    const currUrl = this.router.url;
    return currUrl !== '/user/login';
  }

  logout(): void {
    this.accountService.logout();
    this.router.navigateByUrl('/home');
  }
}
