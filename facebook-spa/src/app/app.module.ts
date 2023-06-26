import ***REMOVED*** NgModule ***REMOVED*** from '@angular/core';
import ***REMOVED*** BrowserModule ***REMOVED*** from '@angular/platform-browser';
import ***REMOVED*** HTTP_INTERCEPTORS, HttpClientModule ***REMOVED*** from '@angular/common/http';

import ***REMOVED*** AppRoutingModule ***REMOVED*** from './***REMOVED***-routing.module';

import ***REMOVED*** AppComponent ***REMOVED*** from './***REMOVED***.component';
import ***REMOVED*** CreatePostComponent ***REMOVED*** from './create-post/create-post.component';
import ***REMOVED*** FormsModule, ReactiveFormsModule ***REMOVED*** from '@angular/forms';
import ***REMOVED*** DisplayPostComponent ***REMOVED*** from './display-post/display-post.component';
import ***REMOVED*** TimelineComponent ***REMOVED*** from './timeline/timeline.component';
import ***REMOVED*** DashboardComponent ***REMOVED*** from './dashboard/dashboard.component';
import ***REMOVED*** ViewCommentsComponent ***REMOVED*** from './view-comments/view-comments.component';
import ***REMOVED*** AddReactionComponent ***REMOVED*** from './add-reaction/add-reaction.component';
import ***REMOVED*** AuthInterceptor, AuthModule, LogLevel ***REMOVED*** from 'angular-auth-oidc-client';
import ***REMOVED*** environment ***REMOVED*** from 'src/environments/environment';
import ***REMOVED*** SignInComponent ***REMOVED*** from './sign-in/sign-in.component';
import ***REMOVED*** SignInCheckComponent ***REMOVED*** from './sign-in-check/sign-in-check.component';

@NgModule(***REMOVED***
  declarations: [								
    AppComponent,
      CreatePostComponent,
      DisplayPostComponent,
      TimelineComponent,
      DashboardComponent,
      ViewCommentsComponent,
      AddReactionComponent,
      SignInComponent,
      SignInCheckComponent
   ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AuthModule.forRoot(***REMOVED***
      config: ***REMOVED***
        authority: environment.idp.authority,
        redirectUrl: `$***REMOVED***window.location.origin***REMOVED***/sign-in`,
        postLogoutRedirectUri: window.location.origin,
        clientId: environment.idp.clientId,
        scope: environment.idp.scope,
        responseType: 'code',
        silentRenew: true,
        useRefreshToken: true,
        logLevel: LogLevel.Debug,
        secureRoutes:[environment.baseUrl]
      ***REMOVED***,
***REMOVED***
  ],
  providers: [ ***REMOVED*** provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true ***REMOVED*** ],
  bootstrap: [AppComponent]
***REMOVED***)
export class AppModule ***REMOVED*** ***REMOVED***
