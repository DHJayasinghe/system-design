import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { DashboardComponent } from "./dashboard/dashboard.component";
import { TimelineComponent } from "./timeline/timeline.component";

const routes: Routes = [
    { path: '', component: DashboardComponent },
    { path: 'timeline', component: TimelineComponent }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
