import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { filter, of, switchMap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/services/notification.service';

@Component({
  selector: 'app-notification-pane',
  templateUrl: './notification-pane.component.html',
  styleUrls: ['./notification-pane.component.css'],
  standalone: true,
  imports: [BrowserModule]
})
export class NotificationPaneComponent implements OnInit {
  activities: NotificationActivity[] = [];

  constructor(
    private http: HttpClient,
    private oidcSecurityService: OidcSecurityService,
    private notificationService: NotificationService) { }

  ngOnInit() {
    this.notificationService.markAsRead();
    this.oidcSecurityService.isAuthenticated$.pipe(filter(d => d.isAuthenticated), switchMap(d => of(d))).subscribe(_ => this.getActivities());
  }

  private getActivities() {
    this.http.get<NotificationActivity[]>(`${environment.baseUrl}/notifications`).subscribe({
      next: (response) => {
        this.activities = response;
      }
    })
  }

  getPostedTime(date: Date) {
    const now = new Date();
    const postedDate = new Date(date);

    const timeDiff = now.getTime() - postedDate.getTime();
    const seconds = Math.floor(timeDiff / 1000);
    const minutes = Math.floor(seconds / 60);
    const hours = Math.floor(minutes / 60);
    const days = Math.floor(hours / 24);

    if (seconds < 60) {
      return 'Just now';
    } else if (minutes < 60) {
      return `${minutes} minutes ago`;
    } else if (hours < 24) {
      return `${hours} hours ago`;
    } else if (days === 1) {
      return 'Yesterday';
    } else {
      return `${days} days ago`;
    }
  }

}

export interface NotificationActivity {
  content: string;
  createdAt: Date;
}
