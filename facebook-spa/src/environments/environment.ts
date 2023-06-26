// This file can be replaced during build by using the `fileReplacements` array.
// `ng build` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = ***REMOVED***
  production: false,
  baseUrl: "http://localhost:8082",
  idp: ***REMOVED***
    authority: "https://localhost:8000",
    clientId: '144e251b-30ff-4027-be96-0623e40cbc19',
    scope:
      'openid offline_access',
  ***REMOVED***
***REMOVED***;

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
