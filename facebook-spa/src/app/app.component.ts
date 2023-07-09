import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'facebook-spa';
  notificationPaneVisibility = false;

  notificationPaneToggled(state: boolean) {
    this.notificationPaneVisibility = state;
  }
}
