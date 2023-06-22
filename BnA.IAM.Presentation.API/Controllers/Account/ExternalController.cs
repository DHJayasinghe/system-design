using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BnA.IAM.Presentation.API.Controllers.Account;

[SecurityHeaders]
[AllowAnonymous]
public class ExternalController : Controller
{
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
    {
        _interaction = interaction;
        _clientStore = clientStore;
        _logger = logger;
        _events = events;
        _httpContext = httpContextAccessor.HttpContext;
        _httpClientFactory = httpClientFactory;
        _userServiceBaseUrl = configuration["UserServiceBaseUrl"];
    }

    /// <summary>
    /// initiate roundtrip to external authentication provider
    /// </summary>
    [HttpGet]
    public IActionResult Challenge(string scheme, string returnUrl)
    {
        if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

        // validate returnUrl - either it is a valid OIDC URL or back to a local page
        if (Url.IsLocalUrl(returnUrl) == false && _interaction.IsValidReturnUrl(returnUrl) == false)
        {
            // user might have clicked on a malicious link - should be logged
            throw new Exception("invalid return URL");
        }

        // start challenge and roundtrip the return URL and scheme 
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(Callback)),
            Items =
                {
                    { "returnUrl", returnUrl },
                    { "scheme", scheme },
                }
        };


        _httpContext.Session.SetString("session_id", Guid.NewGuid().ToString());
        return Challenge(props, scheme);

    }

    /// <summary>
    /// Post processing of external authentication
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Callback()
    {
        // read external identity from the temporary cookie
        var result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
        if (result?.Succeeded != true) throw new Exception("External authentication error");

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            var externalClaims = result.Principal.Claims.Select(c => $"{c.Type}: {c.Value}");
            _logger.LogDebug("External claims: {@claims}", externalClaims);
        }

        var userAccount = GetUserAccountDetailsFromExternalProvider(result);
        var userId = await AutoProvisionUser(userAccount);
        //}

        // this allows us to collect any additional claims or properties
        // for the specific protocols used and store them in the local auth cookie.
        // this is typically used to store data needed for signout from those protocols.
        var additionalLocalClaims = new List<Claim>();
        var localSignInProps = new AuthenticationProperties();
        ProcessLoginCallback(result, additionalLocalClaims, localSignInProps);

        // issue authentication cookie for user
        var isuser = new IdentityServerUser(userId)
        {
            DisplayName = userAccount.Email,
            IdentityProvider = userAccount.Provider,
            AdditionalClaims = additionalLocalClaims
        };

        await HttpContext.SignInAsync(isuser, localSignInProps);

        // delete temporary cookie used during external authentication
        await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

        // retrieve return URL
        var returnUrl = result.Properties.Items["returnUrl"] ?? "~/";

        // check if external login is in the context of an OIDC request
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
        await _events.RaiseAsync(new UserLoginSuccessEvent(userAccount.Provider, userAccount.NameIdentifier, userId, userAccount.Email, true, context?.Client.ClientId));

        if (context != null)
        {
            if (context.IsNativeClient())
            {
                // The client is native, so this change in how to
                // return the response is for better UX for the end user.
                return this.LoadingPage("Redirect", returnUrl);
            }
        }

        return Redirect(returnUrl);
    }

    private static UserAccount GetUserAccountDetailsFromExternalProvider(AuthenticateResult result)
    {
        var externalUser = result.Principal;

        return new UserAccount
        {
            NameIdentifier = (externalUser.FindFirst(JwtClaimTypes.Subject) ??
                          externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
                          throw new Exception("Unknown userid")).Value,
            Email = externalUser.FindFirst(ClaimTypes.Email).Value,
            Name = externalUser.FindFirst(ClaimTypes.Name).Value,
            GivenName = externalUser.FindFirst(ClaimTypes.GivenName).Value,
            Surname = externalUser.FindFirst(ClaimTypes.Surname).Value,
            Provider = result.Properties.Items["scheme"]
        };
    }

    private async Task<string> AutoProvisionUser(UserAccount userAccount)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.PutAsync($"{_userServiceBaseUrl}/users", new StringContent(JsonConvert.SerializeObject(userAccount)));
        response.EnsureSuccessStatusCode();
        var userId = await response.Content.ReadAsStringAsync();
        return userId;
    }

    // if the external login is OIDC-based, there are certain things we need to preserve to make logout work
    // this will be different for WS-Fed, SAML2p or other protocols
    private static void ProcessLoginCallback(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
    {
        // if the external system sent a session id claim, copy it over
        // so we can use it for single sign-out
        var sid = externalResult.Principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
        if (sid != null)
        {
            localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
        }

        // if the external provider issued an id_token, we'll keep it for signout
        var idToken = externalResult.Properties.GetTokenValue("id_token");
        if (idToken != null)
        {
            localSignInProps.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = idToken } });
        }
    }
}

public record UserAccount
{
    public string NameIdentifier { get; init; }
    public string Email { get; init; }
    public string Name { get; init; }
    public string GivenName { get; init; }
    public string Surname { get; init; }
    public string Provider { get; init; }
}
