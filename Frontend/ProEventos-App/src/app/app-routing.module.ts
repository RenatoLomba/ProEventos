import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ContactsComponent } from './components/contacts/contacts.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { EventDetailComponent } from './components/events/event-detail/event-detail.component';
import { EventListComponent } from './components/events/event-list/event-list.component';
import { EventsComponent } from './components/events/events.component';
import { ProfileComponent } from './components/user/profile/profile.component';
import { SpeakersComponent } from './components/speakers/speakers.component';
import { LoginComponent } from './components/user/login/login.component';
import { RegisterComponent } from './components/user/register/register.component';
import { UserComponent } from './components/user/user.component';
import { AuthGuard } from './guards/auth.guard';
import { HomeComponent } from './components/home/home.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full',
  },

  /**
   * ROTAS AUTENTICADAS
   */
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: 'user/profile', component: ProfileComponent },
      { path: 'events', redirectTo: 'events/list' },
      {
        path: 'events',
        component: EventsComponent,
        children: [
          { path: 'detail/:id', component: EventDetailComponent },
          { path: 'detail', component: EventDetailComponent },
          { path: 'list', component: EventListComponent },
        ],
      },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'contacts', component: ContactsComponent },
      { path: 'speakers', component: SpeakersComponent },
    ],
  },

  { path: 'user', redirectTo: 'user/login' },
  {
    path: 'user',
    component: UserComponent,
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
    ],
  },
  { path: 'home', component: HomeComponent },
  { path: '**', redirectTo: 'home', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
