using BnA.IAM.Presentation.API.Common;
using IdentityModel;
***REMOVED***
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
***REMOVED***
using Microsoft.AspNetCore.Mvc;
***REMOVED***
using System.Linq;
using System.Threading.Tasks;

namespace BnA.IAM.Presentation.API.Controllers.Account;

[SecurityHeaders]
[AllowAnonymous]
public sealed class AccountController : Controller
***REMOVED***
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IClientStore _clientStore;
    private readonly IAuthenticationSchemeProvider _schemeProvider;
    private readonly IEventService _events;

    public AccountController(
        IIdentityServerInteractionService interaction,
        IClientStore clientStore,
        IAuthenticationSchemeProvider schemeProvider,
        IEventService events)
***REMOVED***
        _interaction = interaction;
        _clientStore = clientStore;
        _schemeProvider = schemeProvider;
        _events = events;
    ***REMOVED***

    /// <summary>
    /// En***REMOVED*** point into the login workflow
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Login(string returnUrl)
***REMOVED***
        // build a model so we know what to show on the login page
        var vm = await BuildLoginViewModelAsync(returnUrl);

        if (vm.IsExternalLoginOnly)
    ***REMOVED***
            // we only have one option for logging in and it's an external provider
            return RedirectToAction("Challenge", "External", new ***REMOVED*** scheme = vm.ExternalLoginScheme, returnUrl ***REMOVED***);
        ***REMOVED***

        return View(vm);
    ***REMOVED***

    /// <summary>
    /// Handle postback from username/password login
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginInputModel model, string button)
***REMOVED***
        // check if we are in the context of an authorization request
        var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

        // the user clicked the "cancel" button
        if (button != "login")
    ***REMOVED***
            if (context != null)
        ***REMOVED***
                // if the user cancels, send a result back into IdentityServer as if they 
                // denied the consent (even if this client does not require consent).
                // this will send back an access denied OIDC error response to the client.
                await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                if (context.IsNativeClient())
            ***REMOVED***
                    // The client is native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage("Redirect", model.ReturnUrl);
                ***REMOVED***

                return Redirect(model.ReturnUrl);
            ***REMOVED***
            else
        ***REMOVED***
                // since we don't have a valid context, then we just go back to the home page
                return Redirect("~/");
            ***REMOVED***
        ***REMOVED***

        if (ModelState.IsValid)
    ***REMOVED***
            var results = await SignInAsync(model.Username, model.Password);
            //var user = await _userManager.FindByNameAsync(model.Username);
            if (results == SignInResponse.Success)
        ***REMOVED***

                string subjectId = "123";
                await _events.RaiseAsync(new UserLoginSuccessEvent(username: "john.doe@gmail.com", subjectId: subjectId, name: "John Doe", clientId: context?.Client.ClientId));

                // only set explicit expiration here if user chooses "remember me". 
                // otherwise we rely upon expiration configured in cookie middleware.
                AuthenticationProperties props = null;
                if (AccountOptions.AllowRememberLogin && model.RememberLogin)
            ***REMOVED***
                    props = new AuthenticationProperties
                ***REMOVED***
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
            ***REMOVED***
        ***REMOVED***

                // issue authentication cookie with subject ID and username
                var isuser = new IdentityServerUser(subjectId)
            ***REMOVED***
                    DisplayName = "john.doe@gmail.com"
        ***REMOVED***

                await HttpContext.SignInAsync(isuser, props);

                if (context != null)
            ***REMOVED***
                    if (context.IsNativeClient())
                ***REMOVED***
                        // The client is native, so this change in how to
                        // return the response is for better UX for the end user.
                        return this.LoadingPage("Redirect", model.ReturnUrl);
                    ***REMOVED***

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    return Redirect(model.ReturnUrl);
                ***REMOVED***

                // request for a local page
                if (Url.IsLocalUrl(model.ReturnUrl))
            ***REMOVED***
                    return Redirect(model.ReturnUrl);
                ***REMOVED***
                else if (string.IsNullOrEmpty(model.ReturnUrl))
            ***REMOVED***
                    return Redirect("~/");
                ***REMOVED***
                else
            ***REMOVED***
                    // user might have clicked on a malicious link - should be logged
                    throw new Exception("invalid return URL");
                ***REMOVED***
            ***REMOVED***

            if (results == SignInResponse.InvalidCredential)
        ***REMOVED***

                await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials", clientId: context?.Client.ClientId));
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            ***REMOVED***
            if (results == SignInResponse.AccountLockOut)
        ***REMOVED***
                await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "account is temporally locked", clientId: context?.Client.ClientId));
                ModelState.AddModelError(string.Empty, AccountOptions.AccountLockedErrorMessage);
            ***REMOVED***
        ***REMOVED***

        // something went wrong, show form with error
        var vm = await BuildLoginViewModelAsync(model);
        return View(vm);
    ***REMOVED***

    private async Task<SignInResponse> SignInAsync(string username, string password)
***REMOVED***
        if (password != "Admin@#123") return SignInResponse.InvalidCredential;
        //var result = await _signInManager.PasswordSignInAsync(username, password, isPersistent: false, lockoutOnFailure: true);


        return SignInResponse.Success;
    ***REMOVED***

    /// <summary>
    /// Show logout page
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Logout(string logoutId)
***REMOVED***
        // build a model so the logout page knows what to display
        var vm = await BuildLogoutViewModelAsync(logoutId);

        if (vm.ShowLogoutPrompt == false)
    ***REMOVED***
            // if the request for logout was properly authenticated from IdentityServer, then
            // we don't need to show the prompt and can just log the user out directly.
            return await Logout(vm);
        ***REMOVED***

        return View(vm);
    ***REMOVED***

    /// <summary>
    /// Handle logout page postback
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(LogoutInputModel model)
***REMOVED***
        // build a model so the logged out page knows what to display
        var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

        if (User?.Identity.IsAuthenticated == true)
    ***REMOVED***
            // delete local authentication cookie
            await HttpContext.SignOutAsync();
            // raise the logout event
            await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
        ***REMOVED***

        // check if we need to trigger sign-out at an upstream identity provider
        if (vm.TriggerExternalSignout)
    ***REMOVED***
            // build a return URL so the upstream provider will redirect back
            // to us after the user has logged out. this allows us to then
            // complete our single sign-out processing.
            string url = Url.Action("Logout", new ***REMOVED*** logoutId = vm.LogoutId ***REMOVED***);

            // this triggers a redirect to the external provider for sign-out
            return SignOut(new AuthenticationProperties ***REMOVED*** RedirectUri = url ***REMOVED***, vm.ExternalAuthenticationScheme);
        ***REMOVED***

        vm.PostLogoutRedirectUri ??= model.PostLogoutRedirectUri;

        return View("LoggedOut", vm);
    ***REMOVED***

    [HttpGet]
    public IActionResult AccessDenied() => View();


    /*****************************************/
    /* helper APIs for the AccountController */
    /*****************************************/
    private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
***REMOVED***
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
        if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
    ***REMOVED***
            var local = context.IdP == IdentityServerConstants.LocalIdentityProvider;

            // this is meant to short circuit the UI and only trigger the one external IdP
            var vm = new LoginViewModel
        ***REMOVED***
                EnableLocalLogin = local,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
    ***REMOVED***

            if (!local)
        ***REMOVED***
                vm.ExternalProviders = new[] ***REMOVED*** new ExternalProvider ***REMOVED*** AuthenticationScheme = context.IdP ***REMOVED*** ***REMOVED***;
            ***REMOVED***

            return vm;
        ***REMOVED***

        var schemes = await _schemeProvider.GetAllSchemesAsync();

        var providers = schemes
            .Where(x => x.DisplayName != null)
            .Select(x => new ExternalProvider
        ***REMOVED***
                DisplayName = x.DisplayName ?? x.Name,
                AuthenticationScheme = x.Name
        ***REMOVED***.ToList();

        var allowLocal = true;
        if (context?.Client.ClientId != null)
    ***REMOVED***
            var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
            if (client != null)
        ***REMOVED***
                allowLocal = client.EnableLocalLogin;

                if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
            ***REMOVED***
                    providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                ***REMOVED***
            ***REMOVED***
        ***REMOVED***

        return new LoginViewModel
    ***REMOVED***
            AllowRememberLogin = AccountOptions.AllowRememberLogin,
            EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
            ReturnUrl = returnUrl,
            Username = context?.LoginHint,
            ExternalProviders = providers.ToArray()
***REMOVED***
    ***REMOVED***

    private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
***REMOVED***
        var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
        vm.Username = model.Username;
        vm.RememberLogin = model.RememberLogin;
        return vm;
    ***REMOVED***

    private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
***REMOVED***
        var vm = new LogoutViewModel ***REMOVED*** LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt ***REMOVED***;

        if (User?.Identity.IsAuthenticated != true)
    ***REMOVED***
            // if the user is not authenticated, then just show logged out page
            vm.ShowLogoutPrompt = false;
            return vm;
        ***REMOVED***

        var context = await _interaction.GetLogoutContextAsync(logoutId);
        if (context?.ShowSignoutPrompt == false)
    ***REMOVED***
            // it's safe to automatically sign-out
            vm.ShowLogoutPrompt = false;
            return vm;
        ***REMOVED***

        // show the logout prompt. this prevents attacks where the user
        // is automatically signed out by another malicious web page.
        return vm;
    ***REMOVED***

    private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
***REMOVED***
        // get context information (client name, post logout redirect URI and iframe for federated signout)
        var logout = await _interaction.GetLogoutContextAsync(logoutId);

        var vm = new LoggedOutViewModel
    ***REMOVED***
            AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
            PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
            ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
            SignOutIframeUrl = logout?.SignOutIFrameUrl,
            LogoutId = logoutId
***REMOVED***

        if (User?.Identity.IsAuthenticated == true)
    ***REMOVED***
            var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
            if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
        ***REMOVED***
                var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                if (providerSupportsSignout)
            ***REMOVED***
                    if (vm.LogoutId == null)
                ***REMOVED***
                        // if there's no current logout context, we need to create one
                        // this captures necessary info from the current logged in user
                        // before we signout and redirect away to the external IdP for signout
                        vm.LogoutId = await _interaction.CreateLogoutContextAsync();
                    ***REMOVED***

                    vm.ExternalAuthenticationScheme = idp;
                ***REMOVED***
            ***REMOVED***
        ***REMOVED***

        return vm;
    ***REMOVED***
***REMOVED***
