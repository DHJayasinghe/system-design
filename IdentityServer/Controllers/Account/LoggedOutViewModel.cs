namespace BnA.IAM.Presentation.API.Controllers.Account;

public class LoggedOutViewModel
***REMOVED***
    public string PostLogoutRedirectUri ***REMOVED*** get; set; ***REMOVED***
    public string ClientName ***REMOVED*** get; set; ***REMOVED***
    public string SignOutIframeUrl ***REMOVED*** get; set; ***REMOVED***

    public bool AutomaticRedirectAfterSignOut ***REMOVED*** get; set; ***REMOVED***

    public string LogoutId ***REMOVED*** get; set; ***REMOVED***
    public bool TriggerExternalSignout => ExternalAuthenticationScheme != null;
    public string ExternalAuthenticationScheme ***REMOVED*** get; set; ***REMOVED***
***REMOVED***