import ***REMOVED*** NgModule ***REMOVED*** from "@angular/core";
import ***REMOVED*** RouterModule, Routes ***REMOVED*** from "@angular/router";
import ***REMOVED*** PostComponent ***REMOVED*** from "./post/post.component";

const routes: Routes = [
    ***REMOVED*** path: 'post', component: PostComponent ***REMOVED***
];

@NgModule(***REMOVED***
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
***REMOVED***)
export class AppRoutingModule ***REMOVED*** ***REMOVED***
