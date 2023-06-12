import ***REMOVED*** enableProdMode ***REMOVED*** from '@angular/core';
import ***REMOVED*** platformBrowserDynamic ***REMOVED*** from '@angular/platform-browser-dynamic';

import ***REMOVED*** AppModule ***REMOVED*** from './***REMOVED***/***REMOVED***.module';
import ***REMOVED*** environment ***REMOVED*** from './environments/environment';

if (environment.production) ***REMOVED***
  enableProdMode();
***REMOVED***

platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));
