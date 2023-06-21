import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Component({
  selector: 'app-sign-in-check',
  templateUrl: './sign-in-check.component.html'
})
export class SignInCheckComponent implements OnInit {

  constructor(public oidcSecurityService: OidcSecurityService, private router: Router) { }

  ngOnInit() {
    this.oidcSecurityService.isAuthenticated().subscribe(isAuthenticated => {
      if (!isAuthenticated) { this.login(); return; }
      this.router.navigate(['sign-in']);
    })
  }

  private login() {
    this.oidcSecurityService.authorize();
  }

}
