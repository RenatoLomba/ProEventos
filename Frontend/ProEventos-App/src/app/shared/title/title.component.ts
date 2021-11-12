import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-title',
  templateUrl: './title.component.html',
  styleUrls: ['./title.component.scss'],
})
export class TitleComponent implements OnInit {
  @Input() title = '';
  @Input() subtitle = 'Desde 2021';
  @Input() iconClass = 'fa fa-user';
  @Input() showListButton = true;
  @Input() buttonHref = '';

  constructor(private router: Router) {}

  ngOnInit() {}

  list() {
    this.router.navigate([this.buttonHref]);
  }
}
