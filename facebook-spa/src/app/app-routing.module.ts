import ***REMOVED*** NgModule ***REMOVED*** from "@angular/core";
import ***REMOVED*** RouterModule, Routes ***REMOVED*** from "@angular/router";
import ***REMOVED*** DashboardComponent ***REMOVED*** from "./dashboard/dashboard.component";
import ***REMOVED*** TimelineComponent ***REMOVED*** from "./timeline/timeline.component";

const routes: Routes = [
    ***REMOVED*** path: '', component: DashboardComponent ***REMOVED***,
    ***REMOVED*** path: 'timeline', component: TimelineComponent ***REMOVED***
];

@NgModule(***REMOVED***
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
***REMOVED***)
export class AppRoutingModule ***REMOVED*** ***REMOVED***
