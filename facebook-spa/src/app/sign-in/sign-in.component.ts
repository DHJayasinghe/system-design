import ***REMOVED*** Component, OnInit ***REMOVED*** from '@angular/core';
import ***REMOVED*** Router ***REMOVED*** from '@angular/router';
import ***REMOVED*** LoginResponse, OidcSecurityService ***REMOVED*** from 'angular-auth-oidc-client';

@Component(***REMOVED***
  selector: '***REMOVED***-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
***REMOVED***)
export class SignInComponent implements OnInit ***REMOVED***

  constructor(public oidcSecurityService: OidcSecurityService, private router:Router) ***REMOVED*** ***REMOVED***
  ngOnInit() ***REMOVED***

    this.oidcSecurityService.checkAuth().subscribe((loginResponse: LoginResponse) => ***REMOVED***
      console.log(loginResponse);
***REMOVED***;

    this.oidcSecurityService.isAuthenticated().subscribe(isAuthenticated => ***REMOVED***
      if (isAuthenticated) this.router.navigate(['dashboard'])
***REMOVED***
  ***REMOVED***

***REMOVED***
