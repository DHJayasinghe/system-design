using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
***REMOVED***
using System.Linq;
using System.Threading.Tasks;

namespace BnA.IAM.Presentation.API.Controllers.Home;

[SecurityHeaders]
[AllowAnonymous]
public class HomeController : Controller
***REMOVED***
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IWebHostEnvironment _environment;
    private readonly IClientStore _clientStore;
    private readonly ILogger _logger;

    public HomeController(
        IIdentityServerInteractionService interaction,
        IWebHostEnvironment environment,
        IClientStore clientStore,
        ILogger<HomeController> logger)
***REMOVED***
        _interaction = interaction;
        _environment = environment;
        _clientStore = clientStore;
        _logger = logger;
    ***REMOVED***

    public IActionResult Index()
***REMOVED***
        if (_environment.IsDevelopment())
    ***REMOVED***
            // only show in development
            return View();
        ***REMOVED***

        _logger.LogInformation("Homepage is disabled in production. Returning 404.");
        return NotFound();
    ***REMOVED***

    /// <summary>
    /// Shows the error page
    /// </summary>
    public async Task<IActionResult> Error(string errorId)
***REMOVED***
        var vm = new ErrorViewModel();

        // retrieve error details from identityserver
        var message = await _interaction.GetErrorContextAsync(errorId);

        if (message != null)
    ***REMOVED***
            vm.Error = message;

            if (!_environment.IsDevelopment())
        ***REMOVED***
                // only show in development
                message.ErrorDescription = null;
            ***REMOVED***
        ***REMOVED***

        vm.PostLogoutRedirectUri = await FindPostLogoutRedirectUri(message);

        return View("Error", vm);
    ***REMOVED***

    private async Task<string> FindPostLogoutRedirectUri(Duende.IdentityServer.Models.ErrorMessage message)
***REMOVED***
        var originAuthority = new Uri(message.RedirectUri).Authority;
        var client = await _clientStore.FindClientByIdAsync(message.ClientId);
        return client.PostLogoutRedirectUris.FirstOrDefault(uri => new Uri(uri).Authority == originAuthority);
    ***REMOVED***
***REMOVED***
