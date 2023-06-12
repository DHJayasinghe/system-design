import ***REMOVED*** NgModule ***REMOVED*** from '@angular/core';
import ***REMOVED*** BrowserModule ***REMOVED*** from '@angular/platform-browser';
import ***REMOVED*** HttpClientModule ***REMOVED*** from '@angular/common/http';

import ***REMOVED*** AppRoutingModule ***REMOVED*** from './***REMOVED***-routing.module';

import ***REMOVED*** AppComponent ***REMOVED*** from './***REMOVED***.component';
import ***REMOVED*** PostComponent ***REMOVED*** from './post/post.component';

@NgModule(***REMOVED***
  declarations: [	
    AppComponent,
      PostComponent
   ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
***REMOVED***)
export class AppModule ***REMOVED*** ***REMOVED***
