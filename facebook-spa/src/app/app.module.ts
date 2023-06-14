import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { CreatePostComponent } from './post/create-post.component';
import { FormsModule } from '@angular/forms';
import { DisplayPostComponent } from './display-post/display-post.component';
import { TimelineComponent } from './timeline/timeline.component';

@NgModule({
  declarations: [			
    AppComponent,
      CreatePostComponent,
      DisplayPostComponent,
      TimelineComponent
   ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
