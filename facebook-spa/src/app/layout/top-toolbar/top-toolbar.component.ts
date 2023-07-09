import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatBadgeModule } from '@angular/material/badge';
import { NotificationService } from 'src/services/notification.service';

@Component({
  selector: 'app-top-toolbar',
  templateUrl: './top-toolbar.component.html',
  styleUrls: ['./top-toolbar.component.css'],
  standalone: true,
  imports: [MatToolbarModule, MatButtonModule, MatIconModule, MatBadgeModule]
})
export class TopToolbarComponent implements OnInit {
  @Output() notificationPaneToggled = new EventEmitter<boolean>();

  private _notificationPaneVisibility:boolean = false;
  constructor(public notificationService: NotificationService) { }

  ngOnInit() {
  }

  toggleNotificationPane(){
    this._notificationPaneVisibility = !this._notificationPaneVisibility;
    this.notificationPaneToggled.emit(this._notificationPaneVisibility);
  }
}
