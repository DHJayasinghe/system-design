import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr"
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { take } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(private oidcSecurityService: OidcSecurityService) {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.baseUrl}/notifications`)
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();
    connection.start();
    this.oidcSecurityService.getUserData().pipe(take(1)).subscribe(d => {
      connection.on(d['sub'], (message) => {
        console.log(message);
      });
    });
  }
}
