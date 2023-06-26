using IdentityModel;
***REMOVED***
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
***REMOVED***
using Microsoft.AspNetCore.Mvc;
***REMOVED***
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
***REMOVED***
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BnA.IAM.Presentation.API.Controllers.Account;

[SecurityHeaders]
[AllowAnonymous]
public class ExternalController : Controller
***REMOVED***
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IClientStore _clientStore;
    private readonly ILogger<ExternalController> _logger;
    private readonly IEventService _events;
    private readonly HttpContext _httpContext;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _userServiceBaseUrl;

    public ExternalController(
        IHttpClientFactory httpClientFactory,
        IIdentityServerInteractionService interaction,
        IClientStore clientStore,
        IEventService events,
        ILogger<ExternalController> logger,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration)
***REMOVED***
        _interaction = interaction;
        _clientStore = clientStore;
        _logger = logger;
        _events = events;
        _httpContext = httpContextAccessor.HttpContext;
        _httpClientFactory = httpClientFactory;
        _userServiceBaseUrl = configuration["UserServiceBaseUrl"];
    ***REMOVED***

    /// <summary>
    /// initiate roundtrip to external authentication provider
    /// </summary>
    [HttpGet]
    public IActionResult Challenge(string scheme, string returnUrl)
***REMOVED***
        if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

        // validate returnUrl - either it is a valid OIDC URL or back to a local page
        if (Url.IsLocalUrl(returnUrl) == false && _interaction.IsValidReturnUrl(returnUrl) == false)
    ***REMOVED***
            // user might have clicked on a malicious link - should be logged
            throw new Exception("invalid return URL");
        ***REMOVED***

        // start challenge and roundtrip the return URL and scheme 
        var props = new AuthenticationProperties
    ***REMOVED***
            RedirectUri = Url.Action(nameof(Callback)),
            Items =
            ***REMOVED***
                ***REMOVED*** "returnUrl", returnUrl ***REMOVED***,
                ***REMOVED*** "scheme", scheme ***REMOVED***,
                ***REMOVED***
***REMOVED***


        _httpContext.Session.SetString("session_id", Guid.NewGuid().ToString());
        return Challenge(props, scheme);

    ***REMOVED***

    /// <summary>
    /// Post processing of external authentication
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Callback()
***REMOVED***
        // read external identity from the temporary cookie
        var result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
        if (result?.Succeeded != true) throw new Exception("External authentication error");

        if (_logger.IsEnabled(LogLevel.Debug))
    ***REMOVED***
            var externalClaims = result.Principal.Claims.Select(c => $"***REMOVED***c.Type***REMOVED***: ***REMOVED***c.Value***REMOVED***");
            _logger.LogDebug("External claims: ***REMOVED***@claims***REMOVED***", externalClaims);
        ***REMOVED***

        var userAccount = GetUserAccountDetailsFromExternalProvider(result);
        var userId = await AutoProvisionUser(userAccount);
        //***REMOVED***

        // this allows us to collect any additional claims or properties
        // for the specific protocols used and store them in the local auth cookie.
        // this is typically used to store data needed for signout from those protocols.
        var additionalLocalClaims = new List<Claim>();
        var localSignInProps = new AuthenticationProperties();
        ProcessLoginCallback(result, additionalLocalClaims, localSignInProps);

        // issue authentication cookie for user
        var isuser = new IdentityServerUser(userId)
    ***REMOVED***
            DisplayName = userAccount.Email,
            IdentityProvider = userAccount.Provider,
            AdditionalClaims = additionalLocalClaims
***REMOVED***

        await HttpContext.SignInAsync(isuser, localSignInProps);

        // delete temporary cookie used during external authentication
        await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

        // retrieve return URL
        var returnUrl = result.Properties.Items["returnUrl"] ?? "~/";

        // check if external login is in the context of an OIDC request
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
        await _events.RaiseAsync(new UserLoginSuccessEvent(userAccount.Provider, userAccount.NameIdentifier, userId, userAccount.Email, true, context?.Client.ClientId));

        if (context != null)
    ***REMOVED***
            if (context.IsNativeClient())
        ***REMOVED***
                // The client is native, so this change in how to
                // return the response is for better UX for the end user.
                return this.LoadingPage("Redirect", returnUrl);
            ***REMOVED***
        ***REMOVED***

        return Redirect(returnUrl);
    ***REMOVED***

    private static UserAccount GetUserAccountDetailsFromExternalProvider(AuthenticateResult result)
***REMOVED***
        var externalUser = result.Principal;

        return new UserAccount
    ***REMOVED***
            NameIdentifier = (externalUser.FindFirst(JwtClaimTypes.Subject) ??
                          externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
                          throw new Exception("Unknown userid")).Value,
            Email = externalUser.FindFirst(ClaimTypes.Email).Value,
            Name = externalUser.FindFirst(ClaimTypes.Name).Value,
            GivenName = externalUser.FindFirst(ClaimTypes.GivenName).Value,
            Surname = externalUser.FindFirst(ClaimTypes.Surname).Value,
            Provider = result.Properties.Items["scheme"]
***REMOVED***
    ***REMOVED***

    private async Task<string> AutoProvisionUser(UserAccount userAccount)
***REMOVED***
        var client = _httpClientFactory.CreateClient();
        var response = await client.PutAsync($"***REMOVED***_userServiceBaseUrl***REMOVED***/users", new StringContent(JsonConvert.SerializeObject(userAccount)));
        response.EnsureSuccessStatusCode();
        var userId = await response.Content.ReadAsStringAsync();
        return userId;
    ***REMOVED***

    // if the external login is OIDC-based, there are certain things we need to preserve to make logout work
    // this will be different for WS-Fed, SAML2p or other protocols
    private static void ProcessLoginCallback(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
***REMOVED***
        // if the external system sent a session id claim, copy it over
        // so we can use it for single sign-out
        var sid = externalResult.Principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
        if (sid != null)
    ***REMOVED***
            localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
        ***REMOVED***

        // if the external provider issued an id_token, we'll keep it for signout
        var idToken = externalResult.Properties.GetTokenValue("id_token");
        if (idToken != null)
    ***REMOVED***
            localSignInProps.StoreTokens(new[] ***REMOVED*** new AuthenticationToken ***REMOVED*** Name = "id_token", Value = idToken ***REMOVED*** ***REMOVED***);
        ***REMOVED***
    ***REMOVED***
***REMOVED***

public record UserAccount
***REMOVED***
    public string NameIdentifier ***REMOVED*** get; init; ***REMOVED***
    public string Email ***REMOVED*** get; init; ***REMOVED***
    public string Name ***REMOVED*** get; init; ***REMOVED***
    public string GivenName ***REMOVED*** get; init; ***REMOVED***
    public string Surname ***REMOVED*** get; init; ***REMOVED***
    public string Provider ***REMOVED*** get; init; ***REMOVED***
***REMOVED***
