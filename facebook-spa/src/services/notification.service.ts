import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr"
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { filter, of, switchMap, take } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  unreadNotifications = 0;

  constructor(private oidcSecurityService: OidcSecurityService) {
    this.oidcSecurityService.isAuthenticated$.pipe(filter(d => d.isAuthenticated), switchMap(d => of(d))).subscribe(d => {
      const connection = new signalR.HubConnectionBuilder()
        .withUrl(`${environment.baseUrl}/notifications`)
        .withAutomaticReconnect()
        .configureLogging(signalR.LogLevel.Information)
        .build();
      connection.start();
      this.oidcSecurityService.getUserData().pipe(take(1)).subscribe(d => {
        connection.on(d['sub'], (message) => {
          this.unreadNotifications++;
          console.log(message);
        });
      });
    });
  }

  markAsRead(){
    this.unreadNotifications = 0;
  }
}
