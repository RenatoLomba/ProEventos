import { Component, OnInit } from '@angular/core';
import { User } from './models/Identity/User';
import { AccountService } from './services/account.service';
import { Constants } from './utils/constants';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  constructor(private readonly accountService: AccountService) {}

  ngOnInit(): void {
    this._setCurrentUser();
  }

  private _setCurrentUser(): void {
    const userStored = localStorage.getItem(Constants.USER_STORE);

    if (userStored) {
      const user: User = JSON.parse(userStored);
      this.accountService.setCurrentUser(user);
    }
  }
}
