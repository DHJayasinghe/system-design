import { Injectable } from '@angular/core';
import { Router, RouterStateSnapshot } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { map, take } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard {
  constructor(private oidcSecurityService: OidcSecurityService, private router: Router) { }

  canActivate(state: RouterStateSnapshot) {
    return this.oidcSecurityService.isAuthenticated$.pipe(
      take(1),
      map(({ isAuthenticated }) => {
        if (isAuthenticated) return true;

        this.router.navigate(['/sign-in'], {
          queryParams: { returnUrl: state.url },
        });
        return false;
      })
    );
  }
}
