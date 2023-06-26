using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Duende.IdentityServer.Validation;
using System.Collections.Generic;
***REMOVED***

namespace BnA.IAM.Presentation.API.Controllers.Consent
***REMOVED***
    /// <summary>
    /// This controller processes the consent UI
    /// </summary>
    [SecurityHeaders]
    [Authorize]
    public class ConsentController : Controller
***REMOVED***
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IEventService _events;
        private readonly ILogger<ConsentController> _logger;

        public ConsentController(
            IIdentityServerInteractionService interaction,
            IEventService events,
            ILogger<ConsentController> logger)
    ***REMOVED***
            _interaction = interaction;
            _events = events;
            _logger = logger;
        ***REMOVED***

        /// <summary>
        /// Shows the consent screen
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
    ***REMOVED***
            var vm = await BuildViewModelAsync(returnUrl);
            if (vm != null)
        ***REMOVED***
                return View("Index", vm);
            ***REMOVED***

            return View("Error");
        ***REMOVED***

        /// <summary>
        /// Handles the consent screen postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ConsentInputModel model)
    ***REMOVED***
            var result = await ProcessConsent(model);

            if (result.IsRedirect)
        ***REMOVED***
                var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
                if (context?.IsNativeClient() == true)
            ***REMOVED***
                    // The client is native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage("Redirect", result.RedirectUri);
                ***REMOVED***

                return Redirect(result.RedirectUri);
            ***REMOVED***

            if (result.HasValidationError)
        ***REMOVED***
                ModelState.AddModelError(string.Empty, result.ValidationError);
            ***REMOVED***

            if (result.ShowView)
        ***REMOVED***
                return View("Index", result.ViewModel);
            ***REMOVED***

            return View("Error");
        ***REMOVED***

        /*****************************************/
        /* helper APIs for the ConsentController */
        /*****************************************/
        private async Task<ProcessConsentResult> ProcessConsent(ConsentInputModel model)
    ***REMOVED***
            var result = new ProcessConsentResult();

            // validate return url is still valid
            var request = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            if (request == null) return result;

            ConsentResponse grantedConsent = null;

            // user clicked 'no' - send back the standard 'access_denied' response
            if (model?.Button == "no")
        ***REMOVED***
                grantedConsent = new ConsentResponse ***REMOVED*** Error = AuthorizationError.AccessDenied ***REMOVED***;

                // emit event
                await _events.RaiseAsync(new ConsentDeniedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues));
            ***REMOVED***
            // user clicked 'yes' - validate the data
            else if (model?.Button == "yes")
        ***REMOVED***
                // if the user consented to some scope, build the response model
                if (model.ScopesConsented != null && model.ScopesConsented.Any())
            ***REMOVED***
                    var scopes = model.ScopesConsented;
                    if (ConsentOptions.EnableOfflineAccess == false)
                ***REMOVED***
                        scopes = scopes.Where(x => x != Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess);
                    ***REMOVED***

                    grantedConsent = new ConsentResponse
                ***REMOVED***
                        RememberConsent = model.RememberConsent,
                        ScopesValuesConsented = scopes.ToArray(),
                        Description = model.Description
            ***REMOVED***

                    // emit event
                    await _events.RaiseAsync(new ConsentGrantedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues, grantedConsent.ScopesValuesConsented, grantedConsent.RememberConsent));
                ***REMOVED***
                else
            ***REMOVED***
                    result.ValidationError = ConsentOptions.MustChooseOneErrorMessage;
                ***REMOVED***
            ***REMOVED***
            else
        ***REMOVED***
                result.ValidationError = ConsentOptions.InvalidSelectionErrorMessage;
            ***REMOVED***

            if (grantedConsent != null)
        ***REMOVED***
                // communicate outcome of consent back to identityserver
                await _interaction.GrantConsentAsync(request, grantedConsent);

                // indicate that's it ok to redirect back to authorization endpoint
                result.RedirectUri = model.ReturnUrl;
                result.Client = request.Client;
            ***REMOVED***
            else
        ***REMOVED***
                // we need to redisplay the consent UI
                result.ViewModel = await BuildViewModelAsync(model.ReturnUrl, model);
            ***REMOVED***

            return result;
        ***REMOVED***

        private async Task<ConsentViewModel> BuildViewModelAsync(string returnUrl, ConsentInputModel model = null)
    ***REMOVED***
            var request = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (request != null)
        ***REMOVED***
                return CreateConsentViewModel(model, returnUrl, request);
            ***REMOVED***
            else
        ***REMOVED***
                _logger.LogError("No consent request matching request: ***REMOVED***0***REMOVED***", returnUrl);
            ***REMOVED***

            return null;
        ***REMOVED***

        private ConsentViewModel CreateConsentViewModel(
            ConsentInputModel model, string returnUrl,
            AuthorizationRequest request)
    ***REMOVED***
            var vm = new ConsentViewModel
        ***REMOVED***
                RememberConsent = model?.RememberConsent ?? true,
                ScopesConsented = model?.ScopesConsented ?? Enumerable.Empty<string>(),
                Description = model?.Description,

                ReturnUrl = returnUrl,

                ClientName = request.Client.ClientName ?? request.Client.ClientId,
                ClientUrl = request.Client.ClientUri,
                ClientLogoUrl = request.Client.LogoUri,
                AllowRememberConsent = request.Client.AllowRememberConsent
    ***REMOVED***

            vm.IdentityScopes = request.ValidatedResources.Resources.IdentityResources.Select(x => CreateScopeViewModel(x, vm.ScopesConsented.Contains(x.Name) || model == null)).ToArray();

            var apiScopes = new List<ScopeViewModel>();
            foreach (var parsedScope in request.ValidatedResources.ParsedScopes)
        ***REMOVED***
                var apiScope = request.ValidatedResources.Resources.FindApiScope(parsedScope.ParsedName);
                if (apiScope != null)
            ***REMOVED***
                    var scopeVm = CreateScopeViewModel(parsedScope, apiScope, vm.ScopesConsented.Contains(parsedScope.RawValue) || model == null);
                    apiScopes.Add(scopeVm);
                ***REMOVED***
            ***REMOVED***
            if (ConsentOptions.EnableOfflineAccess && request.ValidatedResources.Resources.OfflineAccess)
        ***REMOVED***
                apiScopes.Add(GetOfflineAccessScope(vm.ScopesConsented.Contains(Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess) || model == null));
            ***REMOVED***
            vm.ApiScopes = apiScopes;

            return vm;
        ***REMOVED***

        private ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check)
    ***REMOVED***
            return new ScopeViewModel
        ***REMOVED***
                Value = identity.Name,
                DisplayName = identity.DisplayName ?? identity.Name,
                Description = identity.Description,
                Emphasize = identity.Emphasize,
                Required = identity.Required,
                Checked = check || identity.Required
    ***REMOVED***
        ***REMOVED***

        public ScopeViewModel CreateScopeViewModel(ParsedScopeValue parsedScopeValue, ApiScope apiScope, bool check)
    ***REMOVED***
            var displayName = apiScope.DisplayName ?? apiScope.Name;
            if (!string.IsNullOrWhiteSpace(parsedScopeValue.ParsedParameter))
        ***REMOVED***
                displayName += ":" + parsedScopeValue.ParsedParameter;
            ***REMOVED***

            return new ScopeViewModel
        ***REMOVED***
                Value = parsedScopeValue.RawValue,
                DisplayName = displayName,
                Description = apiScope.Description,
                Emphasize = apiScope.Emphasize,
                Required = apiScope.Required,
                Checked = check || apiScope.Required
    ***REMOVED***
        ***REMOVED***

        private ScopeViewModel GetOfflineAccessScope(bool check)
    ***REMOVED***
            return new ScopeViewModel
        ***REMOVED***
                Value = Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess,
                DisplayName = ConsentOptions.OfflineAccessDisplayName,
                Description = ConsentOptions.OfflineAccessDescription,
                Emphasize = true,
                Checked = check
    ***REMOVED***
        ***REMOVED***
    ***REMOVED***
***REMOVED***