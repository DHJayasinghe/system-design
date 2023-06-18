import ***REMOVED*** NgModule ***REMOVED*** from '@angular/core';
import ***REMOVED*** BrowserModule ***REMOVED*** from '@angular/platform-browser';
import ***REMOVED*** HttpClientModule ***REMOVED*** from '@angular/common/http';

import ***REMOVED*** AppRoutingModule ***REMOVED*** from './***REMOVED***-routing.module';

import ***REMOVED*** AppComponent ***REMOVED*** from './***REMOVED***.component';
import ***REMOVED*** CreatePostComponent ***REMOVED*** from './create-post/create-post.component';
import ***REMOVED*** FormsModule, ReactiveFormsModule ***REMOVED*** from '@angular/forms';
import ***REMOVED*** DisplayPostComponent ***REMOVED*** from './display-post/display-post.component';
import ***REMOVED*** TimelineComponent ***REMOVED*** from './timeline/timeline.component';
import ***REMOVED*** DashboardComponent ***REMOVED*** from './dashboard/dashboard.component';
import ***REMOVED*** ViewCommentsComponent ***REMOVED*** from './view-comments/view-comments.component';
import ***REMOVED*** AddReactionComponent ***REMOVED*** from './add-reaction/add-reaction.component';

@NgModule(***REMOVED***
  declarations: [						
    AppComponent,
      CreatePostComponent,
      DisplayPostComponent,
      TimelineComponent,
      DashboardComponent,
      ViewCommentsComponent,
      AddReactionComponent
   ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
***REMOVED***)
export class AppModule ***REMOVED*** ***REMOVED***
