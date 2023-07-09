import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { CreatePostComponent } from './create-post/create-post.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DisplayPostComponent } from './display-post/display-post.component';
import { TimelineComponent } from './timeline/timeline.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ViewCommentsComponent } from './view-comments/view-comments.component';
import { AddReactionComponent } from './add-reaction/add-reaction.component';
import { AuthInterceptor, AuthModule, LogLevel } from 'angular-auth-oidc-client';
import { environment } from 'src/environments/environment';
import { SignInComponent } from './sign-in/sign-in.component';
import { SignInCheckComponent } from './sign-in-check/sign-in-check.component';
import { PeopleYouMayKnowComponent } from './people-you-may-know/people-you-may-know.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatGridListModule } from '@angular/material/grid-list'
import { MatCardModule } from '@angular/material/card'
import { MatButtonModule } from '@angular/material/button'
import { TopToolbarComponent } from './layout/top-toolbar/top-toolbar.component';
import { NotificationPaneComponent } from './layout/notification-pane/notification-pane.component';
import { MatSidenavModule } from '@angular/material/sidenav';

@NgModule({
  declarations: [
    AppComponent,
    CreatePostComponent,
    DisplayPostComponent,
    TimelineComponent,
    DashboardComponent,
    ViewCommentsComponent,
    AddReactionComponent,
    SignInComponent,
    SignInCheckComponent,
    PeopleYouMayKnowComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AuthModule.forRoot({
      config: {
        authority: environment.idp.authority,
        redirectUrl: `${window.location.origin}/sign-in`,
        postLogoutRedirectUri: window.location.origin,
        clientId: environment.idp.clientId,
        scope: environment.idp.scope,
        responseType: 'code',
        silentRenew: true,
        useRefreshToken: true,
        logLevel: LogLevel.Error,
        secureRoutes: [environment.baseUrl]
      },
    }),
    BrowserAnimationsModule,
    MatGridListModule,
    MatCardModule,
    MatButtonModule,
    MatSidenavModule,
    TopToolbarComponent,
    NotificationPaneComponent
  ],
  providers: [{ provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }],
  bootstrap: [AppComponent]
})
export class AppModule { }
