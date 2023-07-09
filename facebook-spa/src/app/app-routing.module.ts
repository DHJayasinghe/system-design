import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthGuard } from "src/services/auth-guard.service";
import { DashboardComponent } from "./dashboard/dashboard.component";
import { SignInCheckComponent } from "./sign-in-check/sign-in-check.component";
import { SignInComponent } from "./sign-in/sign-in.component";
import { TimelineComponent } from "./timeline/timeline.component";

const routes: Routes = [
    { path: '', component: SignInCheckComponent },
    { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
    { path: 'timeline', component: TimelineComponent, canActivate: [AuthGuard] },
    { path: 'sign-in', component: SignInComponent}
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
