***REMOVED***

namespace BnA.IAM.Presentation.API.Controllers.Account;

public class AccountOptions
***REMOVED***
    public const bool AllowLocalLogin = true;
    public const bool AllowRememberLogin = true;
    public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);

    public const bool ShowLogoutPrompt = true;
    public const bool AutomaticRedirectAfterSignOut = true;

    public const string InvalidCredentialsErrorMessage = "Invalid Email Address or Password.";
    public const string AccountLockedErrorMessage = "This account has been temporary locked for 3 minutes. Please ***REMOVED*** again in 3 minutes. If you have forgotten your password please use the forgot password link.";
***REMOVED***