import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { DashboardComponent } from "./dashboard/dashboard.component";
import { SignInCheckComponent } from "./sign-in-check/sign-in-check.component";
import { SignInComponent } from "./sign-in/sign-in.component";
import { TimelineComponent } from "./timeline/timeline.component";

const routes: Routes = [
    { path: '', component: SignInCheckComponent },
    { path: 'dashboard', component: DashboardComponent },
    { path: 'timeline', component: TimelineComponent },
    { path: 'sign-in', component: SignInComponent }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
