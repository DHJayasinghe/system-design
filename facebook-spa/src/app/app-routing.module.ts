import ***REMOVED*** NgModule ***REMOVED*** from "@angular/core";
import ***REMOVED*** RouterModule, Routes ***REMOVED*** from "@angular/router";
import ***REMOVED*** DashboardComponent ***REMOVED*** from "./dashboard/dashboard.component";
import ***REMOVED*** SignInCheckComponent ***REMOVED*** from "./sign-in-check/sign-in-check.component";
import ***REMOVED*** SignInComponent ***REMOVED*** from "./sign-in/sign-in.component";
import ***REMOVED*** TimelineComponent ***REMOVED*** from "./timeline/timeline.component";

const routes: Routes = [
***REMOVED*** path: '', component: SignInCheckComponent ***REMOVED***,
***REMOVED*** path: 'dashboard', component: DashboardComponent ***REMOVED***,
***REMOVED*** path: 'timeline', component: TimelineComponent ***REMOVED***,
***REMOVED*** path: 'sign-in', component: SignInComponent ***REMOVED***
];

@NgModule(***REMOVED***
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
***REMOVED***)
export class AppRoutingModule ***REMOVED*** ***REMOVED***
