import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { CreatePostComponent } from "./post/create-post.component";
import { TimelineComponent } from "./timeline/timeline.component";

const routes: Routes = [
    { path: '', component: CreatePostComponent },
    { path: 'timeline', component: TimelineComponent }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
