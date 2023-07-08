import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent implements OnInit {

  constructor(public oidcSecurityService: OidcSecurityService, private router: Router) { }
  ngOnInit() {

    this.oidcSecurityService.checkAuth().subscribe(() => {
    });

    this.oidcSecurityService.isAuthenticated().subscribe(isAuthenticated => {
      if (isAuthenticated) this.router.navigate(['dashboard'])
    })
  }

}
