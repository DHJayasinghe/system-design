import ***REMOVED*** Component, OnInit ***REMOVED*** from '@angular/core';
import ***REMOVED*** Router ***REMOVED*** from '@angular/router';
import ***REMOVED*** OidcSecurityService ***REMOVED*** from 'angular-auth-oidc-client';

@Component(***REMOVED***
  selector: '***REMOVED***-sign-in-check',
  templateUrl: './sign-in-check.component.html'
***REMOVED***)
export class SignInCheckComponent implements OnInit ***REMOVED***

  constructor(public oidcSecurityService: OidcSecurityService, private router: Router) ***REMOVED*** ***REMOVED***

  ngOnInit() ***REMOVED***
    this.oidcSecurityService.isAuthenticated().subscribe(isAuthenticated => ***REMOVED***
      if (!isAuthenticated) ***REMOVED*** this.login(); return; ***REMOVED***
      this.router.navigate(['sign-in']);
***REMOVED***
  ***REMOVED***

  private login() ***REMOVED***
    this.oidcSecurityService.authorize();
  ***REMOVED***

***REMOVED***
