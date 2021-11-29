import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, ReplaySubject } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { User } from '../models/Identity/User';
import { UserLogin } from '../models/Identity/UserLogin';
import { UserUpdate } from '../models/Identity/UserUpdate';
import { Constants } from '../utils/constants';

@Injectable()
export class AccountService {
  private _currentUserSource = new ReplaySubject<User | null>(1);
  currentUser$ = this._currentUserSource.asObservable();

  private _baseUrl = `${Constants.API_URL}/account`;

  constructor(private readonly http: HttpClient) {}

  public login(model: UserLogin): Observable<void> {
    return this.http.post<User>(`${this._baseUrl}/login`, model).pipe(
      take(1),
      map((response) => {
        const user = { ...response };
        if (user) {
          this.setCurrentUser(user);
        }
      }),
    );
  }

  public register(model: User): Observable<void> {
    return this.http.post<User>(`${this._baseUrl}/register`, model).pipe(
      take(1),
      map((response) => {
        const user = { ...response };
        if (user) {
          this.setCurrentUser(user);
        }
      }),
    );
  }

  public getTitles(): Observable<string[]> {
    return this.http.get<string[]>(`${this._baseUrl}/gettitles`).pipe(take(1));
  }

  public getFunctions(): Observable<string[]> {
    return this.http
      .get<string[]>(`${this._baseUrl}/getfunctions`)
      .pipe(take(1));
  }

  public logout(): void {
    localStorage.removeItem(Constants.USER_STORE);
    this._currentUserSource.next(null);
  }

  public setCurrentUser(user: User): void {
    localStorage.setItem(Constants.USER_STORE, JSON.stringify(user));
    this._currentUserSource.next(user);
  }

  public getUser(): Observable<UserUpdate> {
    return this.http.get<UserUpdate>(`${this._baseUrl}/getuser`).pipe(take(1));
  }

  public updateUser(model: UserUpdate): Observable<void> {
    return this.http.put<UserUpdate>(`${this._baseUrl}/updateuser`, model).pipe(
      take(1),
      map((user) => {
        this.setCurrentUser(user);
      }),
    );
  }
}
