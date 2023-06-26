using Duende.IdentityServer.Models;

namespace BnA.IAM.Presentation.API.Controllers.Consent
***REMOVED***
    public class ProcessConsentResult
***REMOVED***
        public bool IsRedirect => RedirectUri != null;
        public string RedirectUri ***REMOVED*** get; set; ***REMOVED***
        public Client Client ***REMOVED*** get; set; ***REMOVED***

        public bool ShowView => ViewModel != null;
        public ConsentViewModel ViewModel ***REMOVED*** get; set; ***REMOVED***

        public bool HasValidationError => ValidationError != null;
        public string ValidationError ***REMOVED*** get; set; ***REMOVED***
    ***REMOVED***
***REMOVED***
