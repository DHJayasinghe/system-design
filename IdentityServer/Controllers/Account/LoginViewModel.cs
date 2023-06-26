using System.Collections.Generic;
using System.Linq;

namespace BnA.IAM.Presentation.API.Controllers.Account;

public class LoginViewModel : LoginInputModel
***REMOVED***
    public bool AllowRememberLogin ***REMOVED*** get; set; ***REMOVED*** = true;
    public bool EnableLocalLogin ***REMOVED*** get; set; ***REMOVED*** = false;

    public IEnumerable<ExternalProvider> ExternalProviders ***REMOVED*** get; set; ***REMOVED*** = Enumerable.Empty<ExternalProvider>();
    public IEnumerable<ExternalProvider> VisibleExternalProviders => ExternalProviders.Where(x => !string.IsNullOrWhiteSpace(x.DisplayName));

    public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;
    public string ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;
    public string LandingPage ***REMOVED*** get; set; ***REMOVED***
***REMOVED***